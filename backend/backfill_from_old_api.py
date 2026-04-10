#!/usr/bin/env python3
"""
Backfill member fields in `imei_new` from the OLD application's API.

For every group present in the new DB, this script calls
`Member/GetMemberListSync` on the old API and generates UPDATE statements
that fill ONLY the fields that are currently NULL/empty in the new DB.

Fields backfilled:
  member_profiles : MembershipGrade, Category, CategoryId, ImeiMembershipId,
                    CompanyName, BloodGroup
  address_details : Address, City, State, Country, Pincode

Match key: member_profiles.Id == old API profileID  (verified manually).

Notes on the old data shape (discovered the hard way):
  * The old API's `memberCountry` field is hardcoded to "India" for every
    member — it is NOT a reliable country source.
  * `state_id` is overloaded:
      - For Indian members it is a numeric FK to a `states` table (e.g. "21").
      - For overseas members the old admin stuffed the country NAME into it
        (e.g. "AUSTRALIA", "HONGKONG", "UNITED KINGDOM").
    The script therefore: numeric → ignore, non-numeric → write to Country.
  * The same profileID may appear in multiple groups. We track seen IDs and
    emit each UPDATE at most once to guarantee no duplicate writes.

By default the script runs in DRY-RUN mode and writes
`backfill_from_old_api.sql` for review. Pass --apply to execute it
directly against the DB after generation.

Usage:
  python3 backfill_from_old_api.py            # dry-run (writes .sql only)
  python3 backfill_from_old_api.py --apply    # generate AND execute
  python3 backfill_from_old_api.py --groups 33344,33314   # subset
"""

import argparse
import json
import os
import subprocess
import sys
import time
import urllib.request
import urllib.error
from datetime import datetime

# ───────────────────────────────────────────────────────────────────────
# Config
# ───────────────────────────────────────────────────────────────────────
OLD_API_BASE   = "https://api.imeiconnect.com/api"
OLD_API_AUTH   = "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ=="   # Aladdin:open sesame
OLD_API_TIMEOUT = 60

DB_HOST = "101.53.148.126"
DB_USER = "admin_mysql_db"
DB_PASS = "o27AvGxQQGTBEfrlpD7G1"
DB_NAME = "imei_new"

OUT_SQL = os.path.join(
    os.path.dirname(os.path.abspath(__file__)), "backfill_from_old_api.sql"
)

# ───────────────────────────────────────────────────────────────────────
# Helpers
# ───────────────────────────────────────────────────────────────────────
def esc(val):
    """Escape value for SQL string literal. None / empty -> NULL."""
    if val is None:
        return "NULL"
    s = str(val).strip()
    if s == "" or s.lower() in ("null", "none"):
        return "NULL"
    s = s.replace("\\", "\\\\").replace("'", "\\'").replace("\n", "\\n").replace("\r", "\\r")
    return f"'{s}'"


def is_blank(v):
    if v is None:
        return True
    s = str(v).strip()
    return s == "" or s.lower() in ("null", "none")


def is_blank_or_zero(v):
    return is_blank(v) or str(v).strip() == "0"


def mysql_query(sql, *, capture=True):
    """Run a SELECT against the new DB via the mysql client and return rows
    as a list of tuples (skipping the header)."""
    proc = subprocess.run(
        [
            "mysql",
            "-h", DB_HOST,
            "-u", DB_USER,
            f"-p{DB_PASS}",
            "-N",   # no column names
            "-B",   # tab-separated
            DB_NAME,
            "-e", sql,
        ],
        capture_output=capture,
        text=True,
    )
    if proc.returncode != 0:
        sys.stderr.write(proc.stderr)
        raise SystemExit(f"mysql query failed: {sql[:120]}")
    rows = []
    for line in proc.stdout.splitlines():
        if not line:
            continue
        rows.append(tuple(line.split("\t")))
    return rows


def fetch_member_list_sync(grp_id):
    """POST to old API and return the NewMemberList array (possibly empty)."""
    payload = json.dumps({"grpID": str(grp_id), "updatedOn": "1970-01-01 00:00:00"}).encode()
    req = urllib.request.Request(
        f"{OLD_API_BASE}/Member/GetMemberListSync",
        data=payload,
        headers={
            "Content-Type": "application/json",
            "Authorization": OLD_API_AUTH,
        },
        method="POST",
    )
    try:
        with urllib.request.urlopen(req, timeout=OLD_API_TIMEOUT) as resp:
            body = resp.read()
    except urllib.error.URLError as e:
        sys.stderr.write(f"  ! API error for group {grp_id}: {e}\n")
        return []
    try:
        d = json.loads(body)
    except json.JSONDecodeError as e:
        sys.stderr.write(f"  ! JSON parse error for group {grp_id}: {e}\n")
        return []
    return (d.get("MemberDetail") or {}).get("NewMemberList") or []


# ───────────────────────────────────────────────────────────────────────
# Main
# ───────────────────────────────────────────────────────────────────────
def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true",
                    help="After generating the SQL file, run it against the DB.")
    ap.add_argument("--groups", default="",
                    help="Comma-separated subset of group IDs to process. "
                         "Default = all groups in new DB with at least one member.")
    args = ap.parse_args()

    print("─" * 70)
    print(f"Backfill from old API → {DB_NAME}")
    print(f"Mode  : {'APPLY (will execute)' if args.apply else 'DRY-RUN (writes .sql only)'}")
    print(f"DB    : {DB_USER}@{DB_HOST}/{DB_NAME}")
    print(f"OldAPI: {OLD_API_BASE}")
    print("─" * 70)

    # 1. Group list
    if args.groups.strip():
        group_ids = [g.strip() for g in args.groups.split(",") if g.strip()]
    else:
        rows = mysql_query(
            "SELECT g.Id FROM `groups` g "
            "JOIN group_members gm ON gm.GroupId = g.Id "
            "GROUP BY g.Id ORDER BY COUNT(*) DESC;"
        )
        group_ids = [r[0] for r in rows]
    print(f"Groups to process: {len(group_ids)}")

    # 2. All existing member IDs in new DB (so we never UPDATE non-existent rows)
    print("Loading existing member_profiles.Id set from new DB ...")
    rows = mysql_query("SELECT Id FROM member_profiles;")
    existing_ids = {r[0] for r in rows}
    print(f"  {len(existing_ids):,} member rows in new DB")

    # 3. Members that already have an address row
    rows = mysql_query("SELECT DISTINCT MemberProfileId FROM address_details;")
    members_with_address = {r[0] for r in rows}
    print(f"  {len(members_with_address):,} members already have an address row")

    # 4. Build SQL
    sql = []
    sql.append("-- =====================================================================")
    sql.append("-- Backfill from old API → imei_new")
    sql.append(f"-- Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    sql.append("-- Source   : https://api.imeiconnect.com/api/Member/GetMemberListSync")
    sql.append("-- Strategy : COALESCE(NULLIF(col,''), '<new value>') — never overwrites")
    sql.append("-- =====================================================================")
    sql.append(f"USE `{DB_NAME}`;")
    sql.append("SET sql_mode = '';")
    sql.append("SET autocommit = 0;")
    sql.append("START TRANSACTION;")
    sql.append("SET @now = NOW();")
    sql.append("")

    stats = {
        "groups_ok": 0,
        "groups_empty": 0,
        "members_seen": 0,
        "members_skipped_not_in_new_db": 0,
        "members_skipped_already_processed": 0,
        "profile_updates": 0,
        "address_updates": 0,
        "address_inserts": 0,
    }

    # Dedup: a profileID may appear in multiple groups (e.g. National Admin + Branch).
    # Process each profile exactly once across the whole run so no UPDATE/INSERT
    # is ever emitted twice for the same row.
    processed_profile_ids = set()

    for i, grp in enumerate(group_ids, 1):
        print(f"[{i}/{len(group_ids)}] group {grp} ...", end=" ", flush=True)
        members = fetch_member_list_sync(grp)
        if not members:
            print("empty")
            stats["groups_empty"] += 1
            continue
        stats["groups_ok"] += 1
        print(f"{len(members)} members")

        sql.append(f"-- ─── group {grp} ({len(members)} members from old API) ───")

        for m in members:
            stats["members_seen"] += 1
            pid = str(m.get("profileID", "")).strip()
            if not pid or pid not in existing_ids:
                stats["members_skipped_not_in_new_db"] += 1
                continue
            if pid in processed_profile_ids:
                stats["members_skipped_already_processed"] += 1
                continue
            processed_profile_ids.add(pid)

            # ─── member_profiles backfill ───
            grade   = m.get("MembershipGrade", "")
            cat     = m.get("Category", "")
            cat_id  = m.get("CategoryId", "")
            imei    = m.get("member_IMEI_id", "")
            company = m.get("CompanyName", "")
            blood   = m.get("blood_Group", "")

            set_parts = []
            if not is_blank(grade):
                set_parts.append(f"`MembershipGrade` = COALESCE(NULLIF(`MembershipGrade`, ''), {esc(grade)})")
            if not is_blank(cat):
                set_parts.append(f"`Category` = COALESCE(NULLIF(`Category`, ''), {esc(cat)})")
            if not is_blank_or_zero(cat_id):
                set_parts.append(f"`CategoryId` = COALESCE(NULLIF(NULLIF(`CategoryId`, ''), '0'), {esc(cat_id)})")
            if not is_blank(imei):
                set_parts.append(f"`ImeiMembershipId` = COALESCE(NULLIF(`ImeiMembershipId`, ''), {esc(imei)})")
            if not is_blank(company):
                set_parts.append(f"`CompanyName` = COALESCE(NULLIF(`CompanyName`, ''), {esc(company)})")
            if not is_blank(blood):
                set_parts.append(f"`BloodGroup` = COALESCE(NULLIF(`BloodGroup`, ''), {esc(blood)})")

            if set_parts:
                set_parts.append("`UpdatedAt` = @now")
                sql.append(
                    f"UPDATE `member_profiles` SET {', '.join(set_parts)} WHERE `Id` = {pid};"
                )
                stats["profile_updates"] += 1

            # ─── address_details backfill ───
            addr    = m.get("address", "")
            city    = m.get("city", "")
            state   = m.get("state_name", "")
            pincode = m.get("pincode", "")
            # Pincode "0" is meaningless — treat as blank.
            if str(pincode).strip() == "0":
                pincode = ""

            # `state_id` is a numeric FK for Indian members but contains the
            # COUNTRY NAME for overseas members (e.g. "AUSTRALIA", "HONGKONG").
            # Detect that case and route it to the Country column.
            country = ""
            sid_raw = str(m.get("state_id") or "").strip()
            if sid_raw and not sid_raw.isdigit():
                country = sid_raw

            has_any_addr_data = not (
                is_blank(addr) and is_blank(city) and is_blank(state)
                and is_blank(country) and is_blank(pincode)
            )
            if not has_any_addr_data:
                continue

            if pid in members_with_address:
                # UPDATE existing row(s) in place — only fill blanks
                a_set = []
                if not is_blank(addr):
                    a_set.append(f"`Address` = COALESCE(NULLIF(`Address`, ''), {esc(addr)})")
                if not is_blank(city):
                    a_set.append(f"`City` = COALESCE(NULLIF(`City`, ''), {esc(city)})")
                if not is_blank(state):
                    a_set.append(f"`State` = COALESCE(NULLIF(`State`, ''), {esc(state)})")
                if not is_blank(country):
                    a_set.append(f"`Country` = COALESCE(NULLIF(`Country`, ''), {esc(country)})")
                if not is_blank(pincode):
                    a_set.append(f"`Pincode` = COALESCE(NULLIF(`Pincode`, ''), {esc(pincode)})")
                if a_set:
                    a_set.append("`UpdatedAt` = @now")
                    sql.append(
                        f"UPDATE `address_details` SET {', '.join(a_set)} "
                        f"WHERE `MemberProfileId` = {pid};"
                    )
                    stats["address_updates"] += 1
            else:
                # No address row at all — INSERT one
                sql.append(
                    "INSERT INTO `address_details` "
                    "(`MemberProfileId`, `AddressType`, `Address`, `City`, `State`, `Country`, `Pincode`, `CreatedAt`, `UpdatedAt`) "
                    f"VALUES ({pid}, 'Residence', {esc(addr)}, {esc(city)}, {esc(state)}, {esc(country)}, {esc(pincode)}, @now, @now);"
                )
                # mark so subsequent rows in same script don't insert again
                members_with_address.add(pid)
                stats["address_inserts"] += 1

        sql.append("")

    sql.append("COMMIT;")
    sql.append("")
    sql.append("-- ============== Summary ==============")
    for k, v in stats.items():
        sql.append(f"-- {k}: {v}")

    with open(OUT_SQL, "w") as f:
        f.write("\n".join(sql) + "\n")

    print()
    print("─" * 70)
    print("Summary:")
    for k, v in stats.items():
        print(f"  {k:35s} {v:>8}")
    print(f"  SQL file written: {OUT_SQL}")
    print("─" * 70)

    if args.apply:
        print()
        print(">>> --apply was passed: executing SQL against the DB now ...")
        # mysql aborts on the first error by default when reading from stdin
        # (we're not passing --force). Combined with the explicit
        # START TRANSACTION / COMMIT in the SQL file, a partial failure
        # leaves zero rows changed instead of a half-applied state.
        proc = subprocess.run(
            [
                "mysql",
                "-h", DB_HOST,
                "-u", DB_USER,
                f"-p{DB_PASS}",
                DB_NAME,
            ],
            stdin=open(OUT_SQL, "rb"),
        )
        if proc.returncode == 0:
            print(">>> Apply: OK")
        else:
            print(f">>> Apply FAILED with exit code {proc.returncode}")
            print(">>> Transaction was rolled back — DB is unchanged.")
            sys.exit(proc.returncode)
    else:
        print()
        print("Dry run complete. Review the SQL file, then re-run with --apply to execute.")


if __name__ == "__main__":
    main()
