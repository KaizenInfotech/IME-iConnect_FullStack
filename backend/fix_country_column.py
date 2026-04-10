#!/usr/bin/env python3
"""
Fix `address_details.Country` column.

The earlier backfill (backfill_from_old_api.py) was too eager: it routed any
non-numeric value of the old API's `state_id` field into the Country column,
on the assumption that "non-numeric = country name". In reality, the old DB
let admins type Indian STATE NAMES, CITY NAMES, DISTRICT NAMES, and worse
into `state_id`. The result: ~12,000 address rows now have Indian state /
city values like "MAHARASHTRA", "MUMBAI", "KERALA" sitting in the Country
column.

This script fixes that. For every distinct Country value:
  * If it (case-insensitively) contains any known country name → keep it.
  * Otherwise → it's misclassified. Emit an UPDATE that:
       - moves the value into `State` (only if `State` is currently empty)
       - sets `Country = NULL`
       - bumps `UpdatedAt`

The fix is generated as a single .sql file inside one transaction. Run with
`--apply` to execute it. Dry-run otherwise.
"""

import argparse
import os
import re
import subprocess
import sys
from datetime import datetime

# ───────────────────────────────────────────────────────────────────────
DB_HOST = "101.53.148.126"
DB_USER = "admin_mysql_db"
DB_PASS = "o27AvGxQQGTBEfrlpD7G1"
DB_NAME = "imei_new"

OUT_SQL = os.path.join(
    os.path.dirname(os.path.abspath(__file__)), "fix_country_column.sql"
)

# Country recognition uses WORD-LEVEL matching (not substring matching) so
# that short tokens like "uk" don't false-match inside Indian place names like
# "idUKki" or "thoothUKudi". The value is normalized (lowercased, dots
# stripped), then tokenized on non-letter characters; we then check whether
# any single token equals a known country, AND we also do a substring check
# for multi-word countries like "united kingdom" / "hong kong".

SINGLE_WORD_COUNTRIES = {
    # India + Anglosphere (single words only — multi-word handled below)
    "india", "australia", "nsw", "vic",
    "canada", "usa", "america", "uk", "england", "britain", "scotland",
    "wales", "ireland",
    # East / SE Asia
    "singapore", "singpore",          # typo seen in data
    "japan", "tokyo", "china", "taiwan", "korea",
    "malaysia", "thailand", "indonesia", "jakarta",
    "philippines", "manila", "vietnam", "cambodia", "laos",
    "hongkong", "hongking",           # typos / one-word variants
    # Europe
    "germany", "france", "netherlands", "norway", "sweden", "denmark",
    "finland", "switzerland", "belgium", "austria", "italy", "spain",
    "portugal", "greece", "russia", "ukraine", "poland", "romania",
    # Middle East
    "uae", "dubai", "sharjah", "oman", "qatar", "kuwait", "bahrain",
    "iran", "iraq", "israel", "turkey", "jordan", "lebanon", "egypt",
    "cyprus",
    # South Asia
    "srilanka", "bangladesh", "pakistan", "nepal", "bhutan",
    "maldives", "myanmar", "burma", "mauritius",
    # Africa
    "kenya", "nigeria", "tanzania", "uganda",
    "ethiopia", "ethopia",            # typo
    # Americas
    "brazil", "mexico", "argentina", "venezuela", "colombia",
    "chile", "peru", "bahamas", "nassau",
}

MULTI_WORD_COUNTRIES = [
    "united states", "united kingdom", "great britain", "new zealand",
    "hong kong", "sri lanka", "south africa", "south korea", "north korea",
    "saudi arabia", "united arab emirates", "p r china",
]


def is_real_country(value):
    if value is None:
        return False
    v = value.strip().lower()
    if not v:
        return False
    # Normalize: drop dots so "U.S.A.", "U.K.", "U.A.E." collapse to "usa", "uk", "uae"
    v_norm = v.replace(".", "").replace(",", " ")
    # Multi-word substring check (whitespace-collapsed)
    v_collapsed = re.sub(r"\s+", " ", v_norm).strip()
    for m in MULTI_WORD_COUNTRIES:
        if m in v_collapsed:
            return True
    # Word-level check — tokenize on non-letters
    words = re.findall(r"[a-z]+", v_norm)
    return any(w in SINGLE_WORD_COUNTRIES for w in words)


def esc(val):
    if val is None:
        return "NULL"
    s = str(val)
    s = s.replace("\\", "\\\\").replace("'", "\\'").replace("\n", "\\n").replace("\r", "\\r")
    return f"'{s}'"


def mysql_query(sql):
    proc = subprocess.run(
        ["mysql", "-h", DB_HOST, "-u", DB_USER, f"-p{DB_PASS}",
         "-N", "-B", DB_NAME, "-e", sql],
        capture_output=True, text=True,
    )
    if proc.returncode != 0:
        sys.stderr.write(proc.stderr)
        raise SystemExit(f"mysql query failed: {sql[:120]}")
    rows = []
    for line in proc.stdout.splitlines():
        if line:
            rows.append(tuple(line.split("\t")))
    return rows


# ───────────────────────────────────────────────────────────────────────
def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true",
                    help="Run the generated SQL against the DB.")
    args = ap.parse_args()

    print("─" * 70)
    print("Fix `address_details.Country` — move misclassified state/city names")
    print(f"Mode  : {'APPLY (will execute)' if args.apply else 'DRY-RUN (writes .sql only)'}")
    print(f"DB    : {DB_USER}@{DB_HOST}/{DB_NAME}")
    print("─" * 70)

    print("Loading distinct Country values from address_details ...")
    rows = mysql_query(
        "SELECT Country, COUNT(*) FROM address_details "
        "WHERE Country IS NOT NULL AND Country <> '' "
        "GROUP BY Country ORDER BY COUNT(*) DESC;"
    )

    keep, fix = [], []
    keep_total, fix_total = 0, 0
    for value, count in rows:
        n = int(count)
        if is_real_country(value):
            keep.append((value, n))
            keep_total += n
        else:
            fix.append((value, n))
            fix_total += n

    print(f"  Distinct values: {len(rows)}")
    print(f"  Real-country values  → keep:  {len(keep):>4}  ({keep_total:>6} rows)")
    print(f"  Misclassified values → fix:   {len(fix):>4}  ({fix_total:>6} rows)")
    print()
    print("Sample of values that will be FIXED (top 20 by row count):")
    for v, c in fix[:20]:
        print(f"   {c:>5}  {v}")
    print()
    print("Sample of values that will be KEPT (real countries):")
    for v, c in keep:
        print(f"   {c:>5}  {v}")
    print()

    # Build SQL
    sql = []
    sql.append("-- =====================================================================")
    sql.append("-- Fix address_details.Country column")
    sql.append(f"-- Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    sql.append("-- Strategy: for each misclassified Country value, move it into State")
    sql.append("--           (only if State is currently empty), then NULL out Country.")
    sql.append("-- =====================================================================")
    sql.append(f"USE `{DB_NAME}`;")
    sql.append("SET sql_mode = '';")
    sql.append("SET autocommit = 0;")
    sql.append("START TRANSACTION;")
    sql.append("SET @now = NOW();")
    sql.append("")

    # One UPDATE per distinct misclassified Country value (efficient — ~700 stmts)
    for v, c in fix:
        sql.append(
            f"-- {c} rows"
            "\n"
            "UPDATE `address_details` SET "
            f"`State` = COALESCE(NULLIF(`State`, ''), {esc(v)}), "
            "`Country` = NULL, "
            "`UpdatedAt` = @now "
            f"WHERE `Country` = {esc(v)};"
        )

    sql.append("")
    sql.append("COMMIT;")
    sql.append("")
    sql.append("-- ============== Summary ==============")
    sql.append(f"-- distinct values fixed:  {len(fix)}")
    sql.append(f"-- total rows fixed:       {fix_total}")
    sql.append(f"-- distinct values kept:   {len(keep)}")
    sql.append(f"-- total rows kept:        {keep_total}")

    with open(OUT_SQL, "w") as f:
        f.write("\n".join(sql) + "\n")

    print(f"SQL file written: {OUT_SQL}")
    print(f"  {len(fix)} UPDATE statements covering {fix_total} rows.")
    print("─" * 70)

    if args.apply:
        print()
        print(">>> --apply: executing against the DB now ...")
        proc = subprocess.run(
            ["mysql", "-h", DB_HOST, "-u", DB_USER, f"-p{DB_PASS}", DB_NAME],
            stdin=open(OUT_SQL, "rb"),
        )
        if proc.returncode == 0:
            print(">>> Apply: OK")
        else:
            print(f">>> Apply FAILED with exit code {proc.returncode}")
            print(">>> Transaction rolled back — DB unchanged.")
            sys.exit(proc.returncode)
    else:
        print("Dry-run complete. Review the SQL file, then re-run with --apply.")


if __name__ == "__main__":
    main()
