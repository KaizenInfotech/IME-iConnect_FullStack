#!/usr/bin/env python3
"""
Normalize address_details.Country values to canonical forms that match the
admin panel's countryOptions dropdown.

The earlier passes left ~60 distinct foreign-country variants like
'AUSTRALIA', 'UNITED KINGDOM', 'HONGKONG', 'U.S.A.', 'Mumbai, India' etc.
The admin panel's <select value={form.Country}> only renders the option
that matches EXACTLY (case-sensitive), so most overseas members currently
display blank.

This script:
  * applies a hand-built mapping (raw value → canonical name)
  * generates one UPDATE per distinct raw value
  * runs everything in one transaction
"""

import argparse
import os
import subprocess
import sys
from datetime import datetime

DB_HOST = "101.53.148.126"
DB_USER = "admin_mysql_db"
DB_PASS = "o27AvGxQQGTBEfrlpD7G1"
DB_NAME = "imei_new"

OUT_SQL = os.path.join(
    os.path.dirname(os.path.abspath(__file__)), "normalize_country_values.sql"
)

# Raw value → canonical country name (case-sensitive, must exactly match
# imei-connect-admin/src/pages/MemberDetailPage.jsx countryOptions list).
# Add new mappings here whenever a new variant appears.
MAPPING = {
    # India variants (rows where address mentions India + Indian state)
    "Mumbai, India":       "India",
    "Tamil Nadu, India":   "India",
    "Goa, India":          "India",
    "Karnataka, India":    "India",
    "India, Mharashtra":   "India",
    "Odisha, INDIA":       "India",
    "TAMIL NADU INDIA":    "India",
    "Tamilnadu, India":    "India",
    "Ratnagiri, India":    "India",
    "Kerala, India":       "India",
    # UK
    "UK":              "United Kingdom",
    "UNITED KINGDOM":  "United Kingdom",
    # USA
    "USA":             "United States",
    "U.S.A.":          "United States",
    "U.S.A":           "United States",
    "UNITED STATES":   "United States",
    "LA USA":          "United States",
    # Canada
    "CANADA":          "Canada",
    "CANADA, B.C. -":  "Canada",
    # Australia
    "AUSTRALIA":       "Australia",
    "VIC, AUSTRALIA":  "Australia",
    "NSW":             "Australia",
    # New Zealand
    "NEW ZEALAND":     "New Zealand",
    # Hong Kong
    "HONG KONG":          "Hong Kong",
    "HONGKONG":           "Hong Kong",
    "HONG KONG- S.A.R":   "Hong Kong",
    "HONGKING":           "Hong Kong",
    "NT, HONG KONG SAR":  "Hong Kong",
    # Singapore
    "SINGPORE":        "Singapore",
    # Sri Lanka
    "SRI LANKA":       "Sri Lanka",
    "SRILANKA":        "Sri Lanka",
    "SRI LANKA -":     "Sri Lanka",
    "SRI LANKA-":      "Sri Lanka",
    # UAE
    "UAE":                       "UAE",
    "U.A.E":                     "UAE",
    "U.A.E.":                    "UAE",
    "UNITED ARAB EMIRATES":      "UAE",
    "Dubai":                     "UAE",
    "DUBAI UAE":                 "UAE",
    "AL SOOR ST. SHARJAH- UAE":  "UAE",
    "BUR DUBAI - ZIP CODE":      "UAE",
    # Other countries
    "CYPRUS":             "Cyprus",
    "GERMANY":            "Germany",
    "THAILAND":           "Thailand",
    "P.R. CHINA":         "China",
    "JAKARTA, INDONESIA": "Indonesia",
    "MALAYSIA":           "Malaysia",
    "NASSAU, BAHAMAS":    "Bahamas",
    "TOKYO":              "Japan",
    "JAPAN":              "Japan",
    "KUWAIT":             "Kuwait",
    "ETHOPIA":            "Ethiopia",
    "France":             "France",
    "OMAN":               "Oman",
    "NETHERLANDS":        "Netherlands",
    "MANILA (NCR)":       "Philippines",
    "SWEDEN":             "Sweden",
    "UKRAINE":            "Ukraine",
    "SWITZERLAND":        "Switzerland",
    "NORWAY":             "Norway",
}


def esc(val):
    if val is None:
        return "NULL"
    s = str(val).replace("\\", "\\\\").replace("'", "\\'")
    return f"'{s}'"


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--apply", action="store_true")
    args = ap.parse_args()

    print("─" * 70)
    print("Normalize address_details.Country values to dropdown-canonical form")
    print(f"Mode: {'APPLY' if args.apply else 'DRY-RUN'}")
    print("─" * 70)
    print(f"Mapping has {len(MAPPING)} entries")

    # Build SQL
    sql = []
    sql.append("-- =====================================================================")
    sql.append("-- Normalize address_details.Country to canonical dropdown values")
    sql.append(f"-- Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
    sql.append("-- =====================================================================")
    sql.append(f"USE `{DB_NAME}`;")
    sql.append("SET sql_mode = '';")
    sql.append("SET autocommit = 0;")
    sql.append("START TRANSACTION;")
    sql.append("SET @now = NOW();")
    sql.append("")

    for raw, canonical in MAPPING.items():
        if raw == canonical:
            continue   # nothing to do
        sql.append(
            f"UPDATE `address_details` SET `Country` = {esc(canonical)}, "
            f"`UpdatedAt` = @now WHERE `Country` = {esc(raw)};"
        )

    sql.append("")
    sql.append("COMMIT;")

    with open(OUT_SQL, "w") as f:
        f.write("\n".join(sql) + "\n")

    print(f"SQL file written: {OUT_SQL}")
    print(f"  {sum(1 for r,c in MAPPING.items() if r != c)} UPDATE statements")

    if args.apply:
        print(">>> Executing against DB ...")
        proc = subprocess.run(
            ["mysql", "-h", DB_HOST, "-u", DB_USER, f"-p{DB_PASS}", DB_NAME],
            stdin=open(OUT_SQL, "rb"),
        )
        if proc.returncode == 0:
            print(">>> Apply: OK")
        else:
            print(f">>> Apply FAILED: exit {proc.returncode}")
            sys.exit(proc.returncode)


if __name__ == "__main__":
    main()
