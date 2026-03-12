#!/usr/bin/env python3
"""
TouchBase Data Migration Script
Reads API response JSON files and inserts data into local MySQL database.
"""

import json
import os
import subprocess
import re
from datetime import datetime

DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "api_data")

def load_json(filename):
    fpath = os.path.join(DIR, filename)
    if not os.path.exists(fpath):
        return None
    with open(fpath) as f:
        return json.load(f)

def esc(val):
    """Escape a value for MySQL insertion."""
    if val is None:
        return "NULL"
    s = str(val).strip()
    if s == "" or s.lower() == "null" or s.lower() == "none":
        return "NULL"
    s = s.replace("\\", "\\\\").replace("'", "\\'").replace("\n", "\\n").replace("\r", "\\r")
    return f"'{s}'"

def parse_mobile(mobile_str):
    """Extract raw digits from formatted mobile like '+91 9597056799'."""
    if not mobile_str:
        return None, None
    m = re.match(r'\+?(\d+)\s+(\d+)', str(mobile_str))
    if m:
        return m.group(2), m.group(1)
    digits = re.sub(r'\D', '', str(mobile_str))
    return digits if digits else None, None

def parse_date_dmy(date_str):
    """Parse dates like '04/03/2026' or '20260304' to 'YYYY-MM-DD' or return as-is."""
    if not date_str or date_str.strip() == "":
        return None
    # Try DD/MM/YYYY
    try:
        dt = datetime.strptime(date_str.strip(), "%d/%m/%Y")
        return dt.strftime("%Y-%m-%d")
    except:
        pass
    # Try YYYYMMDD
    try:
        dt = datetime.strptime(date_str.strip(), "%Y%m%d")
        return dt.strftime("%Y-%m-%d")
    except:
        pass
    return date_str.strip()

# ============================================================
# GENERATE SQL
# ============================================================
sql_lines = []

def emit(line):
    sql_lines.append(line)

emit("-- ============================================================")
emit("-- TouchBase Data Migration - Generated from Live API Responses")
emit(f"-- Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
emit("-- ============================================================")
emit("")
emit("USE `touchbase_db`;")
emit("SET FOREIGN_KEY_CHECKS = 0;")
emit("SET sql_mode = '';")
emit("SET @now = NOW(6);")
emit("")

# ────────────────────────────────────────────────────────────
# 1. COUNTRIES
# ────────────────────────────────────────────────────────────
emit("-- ===================== COUNTRIES =====================")
emit("TRUNCATE TABLE `countries`;")
data = load_json("countries_categories.json")
if data:
    result = data.get("TBCountryResult", {})
    countries = result.get("CountryLists", [])
    for c in countries:
        cl = c.get("GrpCountryList", c)
        cid = cl.get("countryId", "")
        code = cl.get("countryCode", "")
        name = cl.get("countryName", "")
        emit(f"INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ({esc(cid)}, {esc(code)}, {esc(name)});")
    emit(f"-- {len(countries)} countries inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 2. CATEGORIES
# ────────────────────────────────────────────────────────────
emit("-- ===================== CATEGORIES =====================")
emit("TRUNCATE TABLE `categories`;")
data = load_json("categories.json")
if data:
    cats = data.get("str", {}).get("Table", [])
    for cat in cats:
        emit(f"INSERT INTO `categories` (`Id`, `CatName`) VALUES ({cat['id']}, {esc(cat['name'])});")
    emit(f"-- {len(cats)} categories inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 3. DISTRICTS (from the app context - using group 31185 as National Admin)
# ────────────────────────────────────────────────────────────
emit("-- ===================== DISTRICTS =====================")
emit("TRUNCATE TABLE `districts`;")
emit("INSERT INTO `districts` (`Id`, `DistrictName`, `DistrictNumber`) VALUES (1, 'National Admin', '31185');")
emit("")

# ────────────────────────────────────────────────────────────
# 4. ZONES
# ────────────────────────────────────────────────────────────
emit("-- ===================== ZONES =====================")
emit("TRUNCATE TABLE `zones`;")
# No zones returned from API for this group
emit("-- No zones data available from API")
emit("")

# ────────────────────────────────────────────────────────────
# 5. USERS
# ────────────────────────────────────────────────────────────
emit("-- ===================== USERS =====================")
emit("TRUNCATE TABLE `users`;")

# Collect unique users from both groups
all_users = {}  # masterID -> user data

# From PostOTP response
otp_data = load_json("post_otp.json")
if otp_data:
    table = otp_data.get("LoginResult", {}).get("ds", {}).get("Table", [])
    for t in table:
        uid = str(t.get("masterUID", ""))
        all_users[uid] = {
            "masterUID": uid,
            "firstName": t.get("FirstName", ""),
            "middleName": t.get("MiddleName", ""),
            "lastName": t.get("LastName", ""),
            "imeiMemId": t.get("IMEI_Mem_Id", ""),
            "profileImage": t.get("Profile_Image", ""),
        }

# From member_list_sync (group 33359)
for sync_file in ["member_list_sync.json", "member_list_sync_31185.json"]:
    data = load_json(sync_file)
    if data:
        members = data.get("MemberDetail", {}).get("NewMemberList", [])
        for m in members:
            uid = str(m.get("masterID", ""))
            mobile, cc = parse_mobile(m.get("memberMobile", ""))
            if uid not in all_users:
                all_users[uid] = {}
            all_users[uid].update({
                "masterUID": uid,
                "mobileNo": mobile,
                "countryCode": cc,
                "firstName": m.get("memberName", ""),
                "middleName": m.get("middleName", ""),
                "lastName": m.get("lastName", ""),
                "email": m.get("memberEmail", ""),
                "profileImage": m.get("profilePic", ""),
                "imeiMemId": m.get("member_IMEI_id", ""),
            })

for uid, u in sorted(all_users.items(), key=lambda x: int(x[0]) if x[0].isdigit() else 0):
    emit(f"INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ({esc(uid)}, {esc(u.get('mobileNo'))}, {esc(u.get('countryCode'))}, {esc(u.get('firstName'))}, {esc(u.get('middleName'))}, {esc(u.get('lastName'))}, {esc(u.get('email'))}, {esc(u.get('profileImage'))}, {esc(u.get('imeiMemId'))}, 1, 1);")

emit(f"-- {len(all_users)} users inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 6. GROUPS
# ────────────────────────────────────────────────────────────
emit("-- ===================== GROUPS =====================")
emit("TRUNCATE TABLE `groups`;")

groups_sync = load_json("all_groups_sync.json")
group_list = []
if groups_sync:
    group_list = groups_sync.get("Result", {}).get("GroupList", {}).get("NewGroupList", [])

# Group detail for 33359
gd = load_json("group_detail.json")
gd_info = None
if gd:
    gdr = gd.get("TBGetGroupResult", {}).get("getGroupDetailResult", [])
    if gdr:
        gd_info = gdr[0].get("GetGroupInfo", {})

# Insert group 33359
emit(f"INSERT INTO `groups` (`Id`, `GrpName`, `GrpImg`, `GrpType`, `GrpCategory`, `City`, `Country`, `Email`, `Mobile`, `TotalMembers`, `DistrictId`, `IsActive`) VALUES (33359, {esc(gd_info.get('grpNAme', 'Testing') if gd_info else 'Testing')}, {esc(gd_info.get('grpImg') if gd_info else None)}, {esc(gd_info.get('grpType') if gd_info else None)}, {esc(gd_info.get('grpCat') if gd_info else None)}, {esc(gd_info.get('cityCountry') if gd_info else None)}, NULL, {esc(gd_info.get('grpEmail') if gd_info else None)}, {esc(gd_info.get('grpMobile') if gd_info else None)}, 7, NULL, 1);")

# Insert group 31185 (National Admin)
emit(f"INSERT INTO `groups` (`Id`, `GrpName`, `GrpType`, `TotalMembers`, `DistrictId`, `IsActive`) VALUES (31185, 'National Admin', 'Close', 34, 1, 1);")

# Insert district clubs as groups
dist_clubs = load_json("district_clubs.json")
if dist_clubs:
    clubs = dist_clubs.get("ClubListResult", {}).get("Clubs", [])
    for club in clubs:
        gid = club.get("grpID", "")
        if str(gid) not in ["33359", "31185"]:
            emit(f"INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ({esc(gid)}, {esc(club.get('clubName'))}, 1);")

emit("")

# ────────────────────────────────────────────────────────────
# 7. GROUP MODULES (from sync data)
# ────────────────────────────────────────────────────────────
emit("-- ===================== GROUP MODULES =====================")
emit("TRUNCATE TABLE `group_modules`;")

if groups_sync:
    modules = groups_sync.get("Result", {}).get("ModuleList", {}).get("NewModuleList", [])
    for mod in modules:
        emit(f"INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ({esc(mod.get('groupModuleId'))}, {esc(mod.get('groupId'))}, {esc(mod.get('moduleId'))}, {esc(mod.get('moduleName'))}, {esc(mod.get('moduleStaticRef'))}, {esc(mod.get('image'))}, {esc(mod.get('masterProfileID'))}, {esc(mod.get('isCustomized'))}, {esc(mod.get('moduleOrderNo'))}, 1);")
    emit(f"-- {len(modules)} modules inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 8. MEMBER PROFILES
# ────────────────────────────────────────────────────────────
emit("-- ===================== MEMBER PROFILES =====================")
emit("TRUNCATE TABLE `member_profiles`;")

all_profiles = {}  # profileID -> data

for sync_file, grp_id in [("member_list_sync.json", "33359"), ("member_list_sync_31185.json", "31185")]:
    data = load_json(sync_file)
    if not data:
        continue
    members = data.get("MemberDetail", {}).get("NewMemberList", [])
    for m in members:
        pid = str(m.get("profileID", ""))
        uid = str(m.get("masterID", ""))
        mobile, cc = parse_mobile(m.get("memberMobile", ""))
        sec_mobile, _ = parse_mobile(m.get("secondry_mobile_no", ""))

        if pid not in all_profiles:
            all_profiles[pid] = {
                "profileID": pid,
                "userId": uid,
                "memberName": f"{m.get('memberName', '')} {m.get('middleName', '')} {m.get('lastName', '')}".strip(),
                "memberEmail": m.get("memberEmail", ""),
                "memberMobile": mobile,
                "memberCountry": m.get("memberCountry", ""),
                "profilePic": m.get("profilePic", ""),
                "bloodGroup": m.get("blood_Group", ""),
                "countryCode": cc,
                "dob": parse_date_dmy(m.get("member_date_of_birth", "")),
                "doa": parse_date_dmy(m.get("member_date_of_wedding", "")),
                "secondaryMobileNo": sec_mobile,
                "membershipGrade": m.get("MembershipGrade", ""),
                "imeiMembershipId": m.get("member_IMEI_id", ""),
                "category": m.get("Category", ""),
                "categoryId": m.get("CategoryId", ""),
                "hideNum": m.get("hide_num", ""),
                "hideMail": m.get("hide_mail", ""),
                "hideWhatsnum": m.get("hide_whatsnum", ""),
                "companyName": m.get("CompanyName", ""),
                "grpID": grp_id,
            }

for pid, p in sorted(all_profiles.items(), key=lambda x: int(x[0]) if x[0].isdigit() else 0):
    emit(f"INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ({esc(pid)}, {esc(p['userId'])}, {esc(p['memberName'])}, {esc(p['memberEmail'])}, {esc(p['memberMobile'])}, {esc(p['memberCountry'])}, {esc(p['profilePic'])}, {esc(p['bloodGroup'])}, {esc(p['countryCode'])}, {esc(p['dob'])}, {esc(p['doa'])}, {esc(p['secondaryMobileNo'])}, {esc(p['membershipGrade'])}, {esc(p['imeiMembershipId'])}, {esc(p['category'])}, {esc(p['categoryId'])}, {esc(p['hideNum'])}, {esc(p['hideMail'])}, {esc(p['hideWhatsnum'])}, {esc(p['companyName'])});")

emit(f"-- {len(all_profiles)} member profiles inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 9. GROUP MEMBERS (link profiles to groups)
# ────────────────────────────────────────────────────────────
emit("-- ===================== GROUP MEMBERS =====================")
emit("TRUNCATE TABLE `group_members`;")

gm_id = 1
for sync_file, grp_id in [("member_list_sync.json", "33359"), ("member_list_sync_31185.json", "31185")]:
    data = load_json(sync_file)
    if not data:
        continue
    members = data.get("MemberDetail", {}).get("NewMemberList", [])

    # Get admin info from sync
    grp_sync = groups_sync.get("Result", {}).get("GroupList", {}).get("NewGroupList", []) if groups_sync else []
    admin_map = {}
    for gs in grp_sync:
        admin_map[gs.get("grpId")] = gs.get("isGrpAdmin", "No")

    for m in members:
        pid = str(m.get("profileID", ""))
        uid = str(m.get("masterID", ""))
        is_admin = "Yes" if pid == "13148" else "No"  # Khushboo is admin from group_detail
        emit(f"INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES ({gm_id}, {grp_id}, {esc(pid)}, '1', {esc(is_admin)}, {esc(uid)}, 1);")
        gm_id += 1

emit(f"-- {gm_id - 1} group memberships inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 10. ADDRESS DETAILS (from member detail API)
# ────────────────────────────────────────────────────────────
emit("-- ===================== ADDRESS DETAILS =====================")
emit("TRUNCATE TABLE `address_details`;")

member_detail = load_json("member_detail.json")
if member_detail:
    md_result = member_detail.get("MemberListDetailResult", {}).get("MemberDetails", [])
    for md in md_result:
        mld = md.get("MemberListDetail", md.get("MemberDetail", {}))
        addresses = mld.get("addressDetails", [])
        for addr_wrap in addresses:
            a = addr_wrap.get("Address", addr_wrap)
            emit(f"INSERT INTO `address_details` (`Id`, `MemberProfileId`, `AddressType`, `Address`, `City`, `State`, `Country`, `Pincode`, `PhoneNo`, `Fax`) VALUES ({esc(a.get('addressID'))}, {esc(a.get('profileID'))}, {esc(a.get('addressType'))}, {esc(a.get('address'))}, {esc(a.get('city'))}, {esc(a.get('StateName', a.get('state')))}, {esc(a.get('country'))}, {esc(a.get('pincode'))}, {esc(a.get('phoneNo'))}, {esc(a.get('fax'))});")

# Also parse addresses from full member sync data
for sync_file in ["member_list_sync.json", "member_list_sync_31185.json"]:
    data = load_json(sync_file)
    if not data:
        continue
    members = data.get("MemberDetail", {}).get("NewMemberList", [])
    for m in members:
        pid = str(m.get("profileID", ""))
        addr = m.get("address", "")
        city = m.get("city", "")
        state = m.get("state_name", m.get("state_id", ""))
        pincode = m.get("pincode", "")
        if addr or city:
            # Only insert if not already inserted from member_detail
            if pid != "13059":  # Skip Mani's - already inserted above
                emit(f"INSERT IGNORE INTO `address_details` (`MemberProfileId`, `AddressType`, `Address`, `City`, `State`, `Pincode`) VALUES ({esc(pid)}, 'Residence', {esc(addr)}, {esc(city)}, {esc(state)}, {esc(pincode)});")

emit("")

# ────────────────────────────────────────────────────────────
# 11. CLUBS (from district_clubs and FindClub data)
# ────────────────────────────────────────────────────────────
emit("-- ===================== CLUBS =====================")
emit("TRUNCATE TABLE `clubs`;")

# From FindClub/GetClubList (the detailed club data)
clubs_data = load_json("clubs.json")
if clubs_data:
    club_list = clubs_data.get("TBGetClubResult", {}).get("ClubResult", {}).get("Table", [])
    for i, club in enumerate(club_list, 1):
        gid = club.get("GroupId", "")
        emit(f"INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES ({i}, {esc(gid)}, {esc(club.get('group_name'))}, {esc(club.get('address'))}, {esc(club.get('city'))}, {esc(club.get('State'))}, {esc(club.get('country_name'))}, {esc(club.get('Meeting_Day'))}, {esc(club.get('meeting_from_time'))}, {esc(club.get('member_name', '').strip() or None)}, {esc(club.get('member_name0', '').strip() or None)});")
    emit(f"-- {len(club_list)} clubs inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 12. ATTENDANCE RECORDS
# ────────────────────────────────────────────────────────────
emit("-- ===================== ATTENDANCE RECORDS =====================")
emit("TRUNCATE TABLE `attendance_records`;")
emit("TRUNCATE TABLE `attendance_members`;")
emit("TRUNCATE TABLE `attendance_visitors`;")

attendance_data = load_json("attendance.json")
if attendance_data:
    records = attendance_data.get("TBAttendanceListResult", {}).get("Result", {}).get("Table", [])
    for rec in records:
        aid = rec.get("AttendanceID", "")
        emit(f"INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES ({aid}, 33359, {esc(rec.get('AttendanceName'))}, {esc(rec.get('AttendanceDate'))}, {esc(rec.get('Attendancetime'))}, {esc(rec.get('Description'))}, 13059);")

        # Attendance members
        am_data = load_json(f"attendance_members_{aid}.json")
        if am_data:
            am_list = am_data.get("TBAttendanceMemberDetailsResult", {}).get("AttendanceMemberResult", [])
            for am in am_list:
                emit(f"INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES ({aid}, {esc(am.get('FK_MemberID'))}, '1');")

        # Attendance visitors
        av_data = load_json(f"attendance_visitors_{aid}.json")
        if av_data:
            av_list = av_data.get("TBAttendanceVisitorsDetailsResult", {}).get("AttendanceVisitorsResult", [])
            for av in av_list:
                emit(f"INSERT INTO `attendance_visitors` (`Id`, `AttendanceRecordId`, `VisitorName`, `Type`) VALUES ({esc(av.get('PK_AttendanceVisitorID'))}, {esc(av.get('FK_AttendanceID'))}, {esc(av.get('VisitorsName'))}, '2');")

    emit(f"-- {len(records)} attendance records inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 13. PAST PRESIDENTS
# ────────────────────────────────────────────────────────────
emit("-- ===================== PAST PRESIDENTS =====================")
emit("TRUNCATE TABLE `past_presidents`;")

pp_data = load_json("past_presidents.json")
if pp_data:
    pp_result = pp_data.get("TBPastPresidentListResult", {})
    pp_list = pp_result.get("TBPastPresidentList", {}).get("newRecords", [])
    for pp in pp_list:
        emit(f"INSERT INTO `past_presidents` (`Id`, `GroupId`, `MemberName`, `PhotoPath`, `TenureYear`) VALUES ({esc(pp.get('PastPresidentId'))}, 33359, {esc(pp.get('MemberName'))}, {esc(pp.get('PhotoPath'))}, {esc(pp.get('TenureYear'))});")
    emit(f"-- {len(pp_list)} past presidents inserted")
emit("")

# ────────────────────────────────────────────────────────────
# 14. TOUCHBASE SETTINGS
# ────────────────────────────────────────────────────────────
emit("-- ===================== TOUCHBASE SETTINGS =====================")
emit("TRUNCATE TABLE `touchbase_settings`;")

ts_data = load_json("touchbase_settings.json")
if ts_data:
    ts_list = ts_data.get("TBSettingResult", {}).get("AllTBSettingResults", [])
    for i, ts in enumerate(ts_list, 1):
        sr = ts.get("SettingResult", ts)
        emit(f"INSERT INTO `touchbase_settings` (`Id`, `UserId`, `GrpId`, `GrpVal`, `GrpName`) VALUES ({i}, 13020, {esc(sr.get('grpId'))}, {esc(sr.get('grpVal'))}, {esc(sr.get('grpName'))});")
emit("")

# ────────────────────────────────────────────────────────────
# 15. BOD / GOVERNING COUNCIL (stored as group_members with designation)
# ────────────────────────────────────────────────────────────
emit("-- ===================== BOD DESIGNATIONS (update group_members) =====================")

bod_data = load_json("bod_list.json")
if bod_data:
    bod_list = bod_data.get("TBGetBODResult", {}).get("BODResult", [])
    for bod in bod_list:
        pid = bod.get("profileID", "")
        desg = bod.get("MemberDesignation", "")
        if desg:
            emit(f"UPDATE `member_profiles` SET `Designation` = {esc(desg)} WHERE `Id` = {esc(pid)};")
    emit(f"-- {len(bod_list)} BOD designations updated")
emit("")

# ────────────────────────────────────────────────────────────
# 16. BANNERS (from dashboard - none returned but add structure)
# ────────────────────────────────────────────────────────────
emit("-- ===================== BANNERS =====================")
emit("TRUNCATE TABLE `banners`;")
emit("-- No banner data returned from API (dashboard empty)")
emit("")

# ────────────────────────────────────────────────────────────
# 17. NOTIFICATIONS (none fetched - add empty truncate)
# ────────────────────────────────────────────────────────────
emit("-- ===================== NOTIFICATIONS =====================")
emit("TRUNCATE TABLE `notifications`;")
emit("-- Notifications are user-specific and transient")
emit("")

# ────────────────────────────────────────────────────────────
# RE-ENABLE FK CHECKS
# ────────────────────────────────────────────────────────────
emit("")
emit("SET FOREIGN_KEY_CHECKS = 1;")
emit("")
emit("-- ============================================================")
emit("-- Migration complete!")
emit("-- ============================================================")

# ────────────────────────────────────────────────────────────
# WRITE SQL FILE
# ────────────────────────────────────────────────────────────
output_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), "migrate_data.sql")
with open(output_path, "w") as f:
    f.write("\n".join(sql_lines) + "\n")

print(f"SQL file generated: {output_path}")
print(f"Total SQL statements: {len([l for l in sql_lines if l.startswith('INSERT') or l.startswith('UPDATE')])}")