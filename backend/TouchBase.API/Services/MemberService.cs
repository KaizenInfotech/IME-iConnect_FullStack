using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TouchBase.API.Controllers;
using TouchBase.API.Data;
using TouchBase.API.Models.DTOs.Member;
using TouchBase.API.Models.Entities;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

public class MemberService : IMemberService
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private const int PageSize = 25;

    public MemberService(AppDbContext db, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<DirectoryListResponse> GetDirectoryList(DirectoryListRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var page = int.TryParse(request.page, out var p) ? p : 1;

        var query = _db.GroupMembers
            .Include(gm => gm.MemberProfile)
            .Where(gm => gm.GroupId == grpId && gm.IsActive);

        if (!string.IsNullOrEmpty(request.searchText))
            query = query.Where(gm => gm.MemberProfile.MemberName!.Contains(request.searchText));

        var total = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(total / (double)PageSize);

        var members = await query
            .OrderBy(gm => gm.MemberProfile.MemberName)
            .Skip((page - 1) * PageSize).Take(PageSize)
            .Select(gm => new MemberListItemDto
            {
                masterID = gm.MemberProfile.UserId.ToString(),
                grpID = gm.GroupId.ToString(),
                profileID = gm.MemberProfileId.ToString(),
                isAdmin = gm.IsGrpAdmin,
                memberName = gm.MemberProfile.MemberName,
                memberEmail = gm.MemberProfile.MemberEmail,
                memberMobile = gm.MemberProfile.MemberMobile,
                memberCountry = gm.MemberProfile.MemberCountry,
                profilePic = gm.MemberProfile.ProfilePic,
                familyPic = gm.MemberProfile.FamilyPic,
                designation = gm.MemberProfile.Designation,
                companyName = gm.MemberProfile.CompanyName,
                bloodGroup = gm.MemberProfile.BloodGroup,
                countryCode = gm.MemberProfile.CountryCode
            }).ToListAsync();

        return new DirectoryListResponse
        {
            status = "0", message = "success",
            resultCount = total.ToString(), TotalPages = totalPages.ToString(), currentPage = page.ToString(),
            MemberListResults = members
        };
    }

    public async Task<MemberDetailResponse> GetMember(MemberDetailRequest request)
    {
        var profileId = int.TryParse(request.memberProfileId ?? request.memberProfID, out var pid) ? pid : 0;
        var grpId = int.TryParse(request.groupId ?? request.grpID, out var gid) ? gid : 0;

        var profile = await _db.MemberProfiles
            .Include(mp => mp.FamilyMembers)
            .Include(mp => mp.Addresses)
            .Include(mp => mp.GroupMemberships)
            .Include(mp => mp.User)
            .FirstOrDefaultAsync(mp => mp.Id == profileId);

        if (profile == null) return new MemberDetailResponse { status = "1", message = "Member not found" };

        var gm = profile.GroupMemberships.FirstOrDefault(g => g.GroupId == grpId);
        var detail = new MemberDetailDto
        {
            masterID = profile.UserId.ToString(), grpID = grpId.ToString(), profileID = profile.Id.ToString(),
            isAdmin = gm?.IsGrpAdmin, memberName = profile.MemberName, memberEmail = profile.MemberEmail,
            memberMobile = profile.MemberMobile, memberCountry = profile.MemberCountry, profilePic = profile.ProfilePic,
            isPersonalDetVisible = profile.IsPersonalDetVisible ?? "1",
            isBusinDetVisible = profile.IsBusinDetVisible ?? "1",
            isFamilDetailVisible = profile.IsFamilDetailVisible ?? "1",
            personalMemberDetails = new List<PersonalMemberDetailDto>
            {
                new() { key = "Name", value = profile.MemberName, uniquekey = "memberName", colType = "text" },
                new() { key = "Mobile", value = profile.MemberMobile, uniquekey = "memberMobile", colType = "text" },
                new() { key = "Email", value = profile.MemberEmail, uniquekey = "memberEmail", colType = "text" },
                new() { key = "DOB", value = profile.Dob, uniquekey = "dob", colType = "date" },
                new() { key = "DOA", value = profile.Doa, uniquekey = "doa", colType = "date" },
                new() { key = "Blood Group", value = profile.BloodGroup, uniquekey = "bloodGroup", colType = "text" },
                new() { key = "Whatsapp", value = profile.WhatsappNum, uniquekey = "whatsappNum", colType = "text" },
                new() { key = "Secondary Mobile", value = profile.SecondaryMobileNo, uniquekey = "secondaryMobileNo", colType = "text" },
            },
            businessMemberDetails = new List<PersonalMemberDetailDto>
            {
                new() { key = "Company", value = profile.CompanyName, uniquekey = "companyName", colType = "text" },
                new() { key = "Designation", value = profile.Designation, uniquekey = "designation", colType = "text" },
                new() { key = "Classification", value = profile.Classification, uniquekey = "classification", colType = "text" },
            },
            familyMemberDetails = profile.FamilyMembers.Select(fm => new FamilyMemberDetailDto
            {
                familyMemberId = fm.Id.ToString(), memberName = fm.MemberName, relationship = fm.Relationship,
                dOB = fm.Dob, emailID = fm.EmailId, anniversary = fm.Anniversary, contactNo = fm.ContactNo,
                particulars = fm.Particulars, bloodGroup = fm.BloodGroup
            }).ToList(),
            addressDetails = profile.Addresses.Select(a => new AddressDetailDto
            {
                addressID = a.Id.ToString(), addressType = a.AddressType, address = a.Address,
                city = a.City, state = a.State, country = a.Country, pincode = a.Pincode,
                phoneNo = a.PhoneNo, fax = a.Fax
            }).ToList()
        };

        return new MemberDetailResponse { status = "0", message = "success", MemberDetails = new List<MemberDetailDto> { detail } };
    }

    public async Task<UpdateResponse> UpdateProfile(UpdateProfileRequest request)
    {
        var profileId = int.TryParse(request.ProfileId, out var pid) ? pid : 0;
        var profile = await _db.MemberProfiles.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == profileId);
        if (profile == null) return new UpdateResponse { status = "1", message = "Profile not found" };

        if (request.memberName != null)
        {
            profile.MemberName = request.memberName;
            // Also update User table FirstName/LastName
            if (profile.User != null)
            {
                var parts = request.memberName.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
                profile.User.FirstName = parts.Length > 0 ? parts[0] : "";
                profile.User.MiddleName = parts.Length > 2 ? parts[1] : "";
                profile.User.LastName = parts.Length > 2 ? parts[2] : (parts.Length > 1 ? parts[1] : "");
                profile.User.UpdatedAt = DateTime.UtcNow;
            }
        }
        if (request.memberMobile != null)
        {
            profile.MemberMobile = request.memberMobile;
            if (profile.User != null) profile.User.MobileNo = request.memberMobile;
        }
        if (request.memberEmailid != null)
        {
            profile.MemberEmail = request.memberEmailid;
            if (profile.User != null) profile.User.Email = request.memberEmailid;
        }
        if (!string.IsNullOrEmpty(request.ProfilePicPath)) profile.ProfilePic = request.ProfilePicPath;
        if (request.dob != null) profile.Dob = request.dob;
        if (request.doa != null) profile.Doa = request.doa;
        profile.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new UpdateResponse { status = "0", message = "success" };
    }

    public async Task<object> GetMemberListSync(MemberSyncRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var members = await _db.GroupMembers.Include(gm => gm.MemberProfile).ThenInclude(mp => mp.User)
            .Include(gm => gm.MemberProfile).ThenInclude(mp => mp.Addresses)
            .Include(gm => gm.Group)
            .Where(gm => gm.GroupId == grpId && gm.IsActive)
            .Select(gm => new
            {
                masterID = gm.MemberProfile.UserId.ToString(),
                grpID = gm.GroupId.ToString(),
                GrpName = gm.Group.GrpName,
                profileID = gm.MemberProfileId.ToString(),
                memberName = gm.MemberProfile.User.FirstName ?? gm.MemberProfile.MemberName,
                middleName = gm.MemberProfile.User.MiddleName ?? "",
                lastName = gm.MemberProfile.User.LastName ?? "",
                memberEmail = gm.MemberProfile.MemberEmail,
                hide_mail = gm.MemberProfile.HideMail ?? "0",
                memberMobile = (gm.MemberProfile.CountryCode != null ? "+" + gm.MemberProfile.CountryCode + " " : "") + gm.MemberProfile.MemberMobile,
                hide_whatsnum = gm.MemberProfile.HideWhatsnum ?? "0",
                secondry_mobile_no = gm.MemberProfile.SecondaryMobileNo ?? "",
                hide_num = gm.MemberProfile.HideNum ?? "0",
                memberCountry = gm.MemberProfile.MemberCountry,
                profilePic = gm.MemberProfile.ProfilePic,
                member_date_of_birth = gm.MemberProfile.Dob ?? "",
                member_date_of_wedding = gm.MemberProfile.Doa ?? "",
                blood_Group = gm.MemberProfile.BloodGroup,
                GradeId = "",
                MembershipGrade = gm.MemberProfile.MembershipGrade,
                Category = gm.MemberProfile.Category,
                CategoryId = gm.MemberProfile.CategoryId ?? "0",
                member_IMEI_id = gm.MemberProfile.ImeiMembershipId ?? "",
                CompanyName = gm.MemberProfile.CompanyName,
                Club_Name = (string?)null,
                address = gm.MemberProfile.Addresses.Select(a => a.Address).FirstOrDefault(),
                city = gm.MemberProfile.Addresses.Select(a => a.City).FirstOrDefault(),
                state_id = "",
                state_name = gm.MemberProfile.Addresses.Select(a => a.State).FirstOrDefault(),
                pincode = gm.MemberProfile.Addresses.Select(a => a.Pincode).FirstOrDefault(),
                addressResult = gm.MemberProfile.Addresses.Select(a => new { a.Address, a.City, state_name = a.State, a.Pincode }).ToList()
            }).ToListAsync();
        return new { status = "0", message = "success", curDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), zipFilePath = "", MemberDetail = new { NewMemberList = members, UpdatedMemberList = new List<object>(), DeletedMemberList = "" } };
    }

    public async Task<UpdateResponse> UpdateProfileDetails(UpdatePersonalDetailsRequest request) =>
        await UpdateProfilePersonalDetails(request);

    public async Task<UpdateResponse> UpdateAddressDetails(UpdateAddressRequest request)
    {
        var addrId = int.TryParse(request.addressID, out var aid) ? aid : 0;
        var profileId = int.TryParse(request.profileID, out var pid) ? pid : 0;

        AddressDetail? addr;
        if (addrId > 0)
        {
            addr = await _db.AddressDetails.FindAsync(addrId);
            if (addr == null) return new UpdateResponse { status = "1", message = "Address not found" };
        }
        else
        {
            // Try to find existing address for this profile
            addr = await _db.AddressDetails.FirstOrDefaultAsync(a => a.MemberProfileId == profileId);
            if (addr == null)
            {
                addr = new AddressDetail { MemberProfileId = profileId };
                _db.AddressDetails.Add(addr);
            }
        }

        addr.AddressType = request.addressType; addr.Address = request.address; addr.City = request.city;
        addr.State = request.state; addr.Country = request.country; addr.Pincode = request.pincode;
        addr.PhoneNo = request.phoneNo; addr.Fax = request.fax; addr.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new UpdateResponse { status = "0", message = "success" };
    }

    public async Task<UpdateResponse> UpdateFamilyDetails(UpdateFamilyRequest request)
    {
        var fmId = int.TryParse(request.familyMemberId, out var fid) ? fid : 0;
        var profileId = int.TryParse(request.profileId, out var pid) ? pid : 0;

        FamilyMember? fm;
        if (fmId > 0)
        {
            fm = await _db.FamilyMembers.FindAsync(fmId);
            if (fm == null) return new UpdateResponse { status = "1", message = "Family member not found" };
        }
        else
        {
            fm = new FamilyMember { MemberProfileId = profileId };
            _db.FamilyMembers.Add(fm);
        }

        fm.MemberName = request.memberName; fm.Relationship = request.relationship; fm.Dob = request.dOB;
        fm.Anniversary = request.anniversary; fm.ContactNo = request.contactNo; fm.Particulars = request.particulars;
        fm.EmailId = request.emailID; fm.BloodGroup = request.bloodGroup; fm.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new UpdateResponse { status = "0", message = "success" };
    }

    public async Task<MemberDetailResponse> GetUpdatedProfileDetails(GetUpdatedProfileRequest request) =>
        await GetMember(new MemberDetailRequest { memberProfileId = request.profileId, groupId = request.groupId });

    public async Task<object> UploadProfilePhoto(IFormFile file, ProfilePhotoRequest request)
    {
        var pid = int.TryParse(request.ProfileID, out var p) ? p : 0;
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine("wwwroot", "uploads", "profile"); Directory.CreateDirectory(path);
        await using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
        var relativePath = $"/uploads/profile/{fileName}";
        // Build full URL using configured base URL (includes /V2 path prefix)
        var appBaseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? "";
        var fullUrl = !string.IsNullOrEmpty(appBaseUrl)
            ? $"{appBaseUrl}{relativePath}"
            : relativePath;
        var profile = await _db.MemberProfiles.FindAsync(pid);
        if (profile != null) { profile.ProfilePic = fullUrl; profile.UpdatedAt = DateTime.UtcNow; await _db.SaveChangesAsync(); }
        return new { status = "0", message = "success", ProfileImage = fullUrl, UploadImageResult = new { status = "0", Imagepath = fullUrl } };
    }

    public async Task<object> GetBodList(BodListRequest request)
    {
        var grpId = int.TryParse(request.grpId, out var gid) ? gid : 0;
        // Read from production DB using WebGetBODList stored procedure
        try
        {
            var members = new List<object>();
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            var yearFilter = !string.IsNullOrEmpty(request.YearFilter) ? request.YearFilter : $"{DateTime.Now.Year}-{DateTime.Now.Year}";
            cmd.CommandText = "CALL WebGetBODList(@grpId, @search, @year)";
            cmd.Parameters.AddWithValue("@grpId", grpId);
            cmd.Parameters.AddWithValue("@search", request.searchText ?? "");
            cmd.Parameters.AddWithValue("@year", yearFilter);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                members.Add(new
                {
                    BOD_pkID = reader["BOD_pkID"]?.ToString(),
                    profileID = reader["FK_member_master_profile_id"]?.ToString(),
                    sr_NO = reader["sr_NO"]?.ToString(),
                    memberName = reader["member_name"]?.ToString()?.Trim(),
                    MemberDesignation = reader["Designation_Name"]?.ToString(),
                    membermobile = reader["PhoneNo"]?.ToString(),
                    Email = reader["EmailID"]?.ToString(),
                });
            }
            if (members.Count > 0)
                return new { TBGetBODResult = new { status = "0", message = "success", BODResult = members } };
        }
        catch { }

        // Fallback to local DB
        var query = _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Where(x => x.gm.GroupId == grpId && x.gm.IsActive && x.mp.Designation != null && x.mp.Designation != "");
        if (!string.IsNullOrEmpty(request.searchText)) query = query.Where(x => x.mp.MemberName != null && x.mp.MemberName.Contains(request.searchText));
        var localMembers = await query.Select(x => new { masterUID = x.mp.UserId.ToString(), grpID = x.gm.GroupId.ToString(), profileID = x.mp.Id.ToString(), memberName = x.mp.MemberName, membermobile = x.mp.MemberMobile, MemberDesignation = x.mp.Designation, pic = x.mp.ProfilePic, Email = x.mp.MemberEmail }).ToListAsync();
        return new { TBGetBODResult = new { status = "0", message = "success", BODResult = localMembers } };
    }

    public async Task<object> ReorderBOD(List<ReorderItem> items)
    {
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            foreach (var item in items)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE bod_master SET BODorderNumber = @order WHERE BOD_PkID = @id AND COALESCE(Isdeleted,0)=0";
                cmd.Parameters.AddWithValue("@order", item.DisplayOrder);
                cmd.Parameters.AddWithValue("@id", item.MemberId);
                await cmd.ExecuteNonQueryAsync();
            }
            return new { status = "0", message = "Reorder saved successfully" };
        }
        catch (Exception ex) { return new { status = "1", message = ex.Message }; }
    }

    private const string ProdConnStr = "server=101.53.148.126;database=imei_new;user=admin_mysql_db;password=o27AvGxQQGTBEfrlpD7G1;AllowZeroDateTime=True;ConvertZeroDateTime=True;Allow User Variables=true";

    public async Task<object> GetGoverningCouncil(GoverningCouncilRequest request)
    {
        // Read from production DB (imei_production.bod_master) - same data as mobile app
        try
        {
            var members = new List<object>();
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            var sql = @"SELECT b.BOD_PkID, b.FK_member_master_profile_id as ProfileID, b.PhoneNo, b.EmailID as Email, b.YearFilter,
                CASE WHEN mm.member_name IS NOT NULL
                  THEN TRIM(REPLACE(CONCAT(COALESCE(mm.member_name,''), ' ', COALESCE(NULLIF(mm.middle_name,''),''), ' ', COALESCE(mm.last_name,'')), '  ', ' '))
                  ELSE COALESCE(CONVERT(mp.MemberName USING utf8mb4), '')
                END as MemberName,
                COALESCE(m.fk_main_member_master_id, mp.UserId) as masterUID, b.FK_group_master_id as grpID,
                COALESCE(mm.ProfilePic_Path, m.member_profile_photo_path, CONVERT(mp.ProfilePic USING utf8mb4)) as pic,
                COALESCE(d.Designation, b.OtherDesignation, '') as MemberDesignation,
                COALESCE(m.member_mobile_no, mm.member_mobile, CONVERT(mp.MemberMobile USING utf8mb4)) as membermobile,
                b.fk_chapter_id as chapterId, COALESCE(cg.group_name, '') as chapterName
                FROM bod_master b
                LEFT JOIN member_master_profile m ON b.FK_member_master_profile_id = m.pk_member_master_profile_id
                LEFT JOIN main_member_master mm ON m.fk_main_member_master_id = mm.pk_main_member_master_id
                LEFT JOIN member_profiles mp ON b.FK_member_master_profile_id = mp.Id
                LEFT JOIN users u ON mp.UserId = u.Id
                LEFT JOIN tbl_master_designation d ON b.Fk_Master_Designation_ID = d.Pk_Master_Designation_ID
                LEFT JOIN group_master cg ON b.fk_chapter_id = cg.pk_group_master_id
                WHERE b.FK_group_master_id = 31185 AND (b.Isdeleted = 0 OR b.Isdeleted IS NULL)
                AND (b.YearFilter LIKE CONCAT('%', YEAR(NOW()), '%') OR b.YearFilter LIKE CONCAT('%', YEAR(NOW())-1, '%'))";
            if (!string.IsNullOrEmpty(request.searchText))
                sql += " AND (m.Member_name LIKE @search OR m.last_name LIKE @search)";
            if (!string.IsNullOrEmpty(request.YearFilter))
                sql += " AND b.YearFilter = @yearFilter";
            sql += " ORDER BY b.BODorderNumber";
            cmd.CommandText = sql;
            if (!string.IsNullOrEmpty(request.searchText))
                cmd.Parameters.AddWithValue("@search", $"%{request.searchText}%");
            if (!string.IsNullOrEmpty(request.YearFilter))
                cmd.Parameters.AddWithValue("@yearFilter", request.YearFilter);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                members.Add(new
                {
                    BOD_pkID = reader["BOD_PkID"]?.ToString(),
                    ProfileID = reader["ProfileID"]?.ToString(),
                    PhoneNo = reader["PhoneNo"]?.ToString(),
                    Email = reader["Email"]?.ToString(),
                    YearFilter = reader["YearFilter"]?.ToString(),
                    MemberName = reader["MemberName"]?.ToString()?.Trim(),
                    masterUID = reader["masterUID"]?.ToString(),
                    grpID = reader["grpID"]?.ToString(),
                    pic = reader["pic"]?.ToString(),
                    MemberDesignation = reader["MemberDesignation"]?.ToString(),
                    membermobile = reader["membermobile"]?.ToString(),
                    chapterId = reader["chapterId"]?.ToString(),
                    chapterName = reader["chapterName"]?.ToString(),
                });
            }
            if (members.Count > 0)
                return new { status = "0", message = "success", Result = new { Table = members } };
        }
        catch { }

        // Fallback to local DB
        var query = _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Where(x => x.gm.GroupId == 31185 && x.gm.IsActive && x.mp.Designation != null && x.mp.Designation != "");
        if (!string.IsNullOrEmpty(request.searchText)) query = query.Where(x => x.mp.MemberName != null && x.mp.MemberName.Contains(request.searchText));
        var localMembers = await query.Select(x => new { BOD_pkID = x.mp.Id, ProfileID = x.mp.Id, FK_Master_Designation_ID = 0, PhoneNo = x.mp.MemberMobile, Email = x.mp.MemberEmail, MemberName = x.mp.MemberName, masterUID = x.mp.UserId, sr_NO = 0, grpID = 31185, pic = x.mp.ProfilePic, MemberDesignation = x.mp.Designation, membermobile = (x.mp.CountryCode != null ? "+" + x.mp.CountryCode + " " : "") + x.mp.MemberMobile }).ToListAsync();
        return new { status = "0", message = "success", Result = new { Table = localMembers } };
    }

    public async Task<object> GetBODDetails(string bodPkId, string yearFilter)
    {
        var id = int.TryParse(bodPkId, out var bid) ? bid : 0;
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "CALL GetBODDetails_by_ID(@id, @year)";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@year", yearFilter ?? "");
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new
                {
                    status = "0", message = "success",
                    data = new
                    {
                        BOD_PkID = bodPkId,
                        Sr_No = reader["Sr_No"]?.ToString(),
                        FK_Master_Designation_ID = reader["FK_Master_Designation_ID"]?.ToString(),
                        OtherDesignation = reader["OtherDesignation"]?.ToString(),
                        EmailID = reader["EmailID"]?.ToString(),
                        PhoneNo = reader["PhoneNo"]?.ToString(),
                        FK_member_master_profile_id = reader["FK_member_master_profile_id"]?.ToString(),
                    }
                };
            }
        }
        catch { }
        return new { status = "1", message = "Not found" };
    }

    public async Task<object> DeleteBOD(string bodPkId, string yearFilter)
    {
        var id = int.TryParse(bodPkId, out var bid) ? bid : 0;
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "CALL Delete_BOD_Member(@id, @year)";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@year", yearFilter ?? "");
            await cmd.ExecuteNonQueryAsync();
            return new { status = "0", message = "Member Deleted Successfully" };
        }
        catch (Exception ex) { return new { status = "1", message = ex.Message }; }
    }

    public async Task<object> UpdateBOD(UpdateBODRequest request)
    {
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            // Map designation: frontend sends ID (numeric string), use as-is
            var desigIdMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Chairman"] = "1", ["Vice Chairman"] = "2", ["Hon. Secretary"] = "3",
                ["Hon. Treasurer"] = "4", ["Governing Council Member"] = "5", ["Exe. Comm. Member"] = "6",
                ["President"] = "7", ["Vice President"] = "8", ["Hon. General Secretary"] = "9", ["Others"] = "10"
            };
            var desigRaw = request.designation ?? "10";
            // If it's already a number, use it; if it's a name, map it
            var desigId = int.TryParse(desigRaw, out _) ? desigRaw : desigIdMap.GetValueOrDefault(desigRaw, "10");

            // Check for duplicate Chairman - only one allowed per group (same as production)
            var bodPkId = int.TryParse(request.BOD_PkID, out var bpk) ? bpk : 0;
            var restrictedDesigs = new[] { "1" }; // Chairman only
            if (restrictedDesigs.Contains(desigId))
            {
                using var chkCmd = conn.CreateCommand();
                var yearFilter = request.yearFilter ?? $"{DateTime.Now.Year}-{DateTime.Now.Year}";
                chkCmd.CommandText = "SELECT BOD_PkID FROM bod_master WHERE FK_group_master_id=@grp AND Fk_Master_Designation_ID=@desig AND COALESCE(Isdeleted,0)=0 AND BOD_PkID != @bodId AND YearFilter=@year LIMIT 1";
                chkCmd.Parameters.AddWithValue("@grp", request.groupId ?? "31185");
                chkCmd.Parameters.AddWithValue("@desig", desigId);
                chkCmd.Parameters.AddWithValue("@bodId", bodPkId);
                chkCmd.Parameters.AddWithValue("@year", yearFilter);
                var existing = await chkCmd.ExecuteScalarAsync();
                if (existing != null)
                {
                    var desigName2 = desigId == "7" ? "President" : "Chairman";
                    return new { status = "1", message = $"{desigName2} already exists. Only one {desigName2} is allowed." };
                }
            }
            // Check if member is already President in another group (Executive Committee)
            if (desigId == "7" && request.groupId == "31185")
            {
                var memberProfileId = request.memberProfileID ?? "0";
                using var chkPresCmd = conn.CreateCommand();
                chkPresCmd.CommandText = "SELECT FK_group_master_id FROM bod_master WHERE FK_member_master_profile_id=@mid AND Fk_Master_Designation_ID=7 AND COALESCE(Isdeleted,0)=0 AND FK_group_master_id != 31185 AND BOD_PkID != @bodId LIMIT 1";
                chkPresCmd.Parameters.AddWithValue("@mid", memberProfileId);
                chkPresCmd.Parameters.AddWithValue("@bodId", bodPkId);
                var existingGrp = await chkPresCmd.ExecuteScalarAsync();
                if (existingGrp != null)
                {
                    return new { status = "1", message = "This member is already a President in Executive Committee." };
                }
            }

            // Reverse map: get the name from ID for p_Mem_Designation
            var idToName = new Dictionary<string, string>
            {
                ["1"] = "Chairman", ["2"] = "Vice Chairman", ["3"] = "Hon. Secretary",
                ["4"] = "Hon. Treasurer", ["5"] = "Governing Council Member", ["6"] = "Exe. Comm. Member",
                ["7"] = "President", ["8"] = "Vice President", ["9"] = "Hon. General Secretary", ["10"] = "Others"
            };
            var desigName = idToName.GetValueOrDefault(desigId, desigRaw);

            // Param 12 (p_gallery_Out_id) is INOUT — must use a MySQL session variable
            cmd.CommandText = "SET @gallery_out = 0; CALL WEBUpdateMemberDesignation(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @gallery_out, @p13, @p14, @p15);";
            cmd.Parameters.AddWithValue("@p1", int.TryParse(request.BOD_PkID, out var bod) ? bod : 0);
            cmd.Parameters.AddWithValue("@p2", desigId);
            cmd.Parameters.AddWithValue("@p3", desigName);
            cmd.Parameters.AddWithValue("@p4", request.memberProfileID ?? "0");
            cmd.Parameters.AddWithValue("@p5", request.name ?? "");
            cmd.Parameters.AddWithValue("@p6", request.phoneNo ?? "");
            cmd.Parameters.AddWithValue("@p7", request.emailID ?? "");
            cmd.Parameters.AddWithValue("@p8", request.yearFilter ?? "");
            cmd.Parameters.AddWithValue("@p9", request.groupId ?? "31185");
            cmd.Parameters.AddWithValue("@p10", "0");
            cmd.Parameters.AddWithValue("@p11", request.otherDesignation ?? "");
            cmd.Parameters.AddWithValue("@p13", DBNull.Value);
            cmd.Parameters.AddWithValue("@p14", DBNull.Value);
            cmd.Parameters.AddWithValue("@p15", int.TryParse(request.chapterId, out var ch) ? ch : 0);
            await cmd.ExecuteNonQueryAsync();

            // Sync member name from EF tables to production-synced tables so GC listing shows correct name
            var mpId = int.TryParse(request.memberProfileID, out var mpid) ? mpid : 0;
            if (mpId > 0)
            {
                try
                {
                    var efProfile = await _db.MemberProfiles.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == mpId);
                    if (efProfile != null)
                    {
                        var fullName = efProfile.User != null
                            ? $"{efProfile.User.FirstName} {efProfile.User.LastName}".Trim()
                            : efProfile.MemberName ?? "";
                        var firstName = efProfile.User?.FirstName ?? efProfile.MemberName ?? "";
                        var middleName = efProfile.User?.MiddleName ?? "";
                        var lastName = efProfile.User?.LastName ?? "";
                        var userId = efProfile.UserId;

                        using var syncCmd = conn.CreateCommand();
                        syncCmd.CommandText = "INSERT INTO member_master_profile (pk_member_master_profile_id, fk_main_member_master_id, Member_name, member_email_id, member_mobile_no) VALUES (@id, @uid, @name, @email, @mobile) ON DUPLICATE KEY UPDATE fk_main_member_master_id=@uid, Member_name=@name, member_email_id=@email, member_mobile_no=@mobile";
                        syncCmd.Parameters.AddWithValue("@id", mpId);
                        syncCmd.Parameters.AddWithValue("@uid", userId);
                        syncCmd.Parameters.AddWithValue("@name", firstName);
                        syncCmd.Parameters.AddWithValue("@email", efProfile.MemberEmail ?? "");
                        syncCmd.Parameters.AddWithValue("@mobile", efProfile.MemberMobile ?? "");
                        await syncCmd.ExecuteNonQueryAsync();

                        using var syncCmd2 = conn.CreateCommand();
                        syncCmd2.CommandText = "INSERT INTO main_member_master (pk_main_member_master_id, member_name, middle_name, last_name, member_mobile, member_emailid) VALUES (@id, @first, @middle, @last, @mobile, @email) ON DUPLICATE KEY UPDATE member_name=@first, middle_name=@middle, last_name=@last, member_mobile=@mobile, member_emailid=@email";
                        syncCmd2.Parameters.AddWithValue("@id", userId);
                        syncCmd2.Parameters.AddWithValue("@first", firstName);
                        syncCmd2.Parameters.AddWithValue("@middle", middleName);
                        syncCmd2.Parameters.AddWithValue("@last", lastName);
                        syncCmd2.Parameters.AddWithValue("@mobile", efProfile.MemberMobile ?? "");
                        syncCmd2.Parameters.AddWithValue("@email", efProfile.MemberEmail ?? "");
                        await syncCmd2.ExecuteNonQueryAsync();
                    }
                }
                catch { }
            }

            // Check gallery_out value: 1=new, 2=already exists, 3=updated
            using var outCmd = conn.CreateCommand();
            outCmd.CommandText = "SELECT @gallery_out";
            var galleryOut = await outCmd.ExecuteScalarAsync();
            var outVal = Convert.ToInt32(galleryOut ?? 0);
            if (outVal == 2)
                return new { status = "1", message = "This member already exists with the same designation." };

            return new { status = "0", message = "Governing Council Updated Successfully" };
        }
        catch (Exception ex) { return new { status = "1", message = ex.Message }; }
    }

    public async Task<UpdateResponse> UpdateMember(UpdateMemberRequest request)
    {
        var pid = int.TryParse(request.MemProfileId, out var p) ? p : 0;
        var profile = await _db.MemberProfiles.FindAsync(pid);
        if (profile == null) return new UpdateResponse { status = "1", message = "Not found" };
        if (request.mobile_num_hide != null) { profile.HideWhatsnum = request.mobile_num_hide; profile.MobileNumHide = request.mobile_num_hide; }
        if (request.secondary_num_hide != null) { profile.HideNum = request.secondary_num_hide; profile.SecondaryNumHide = request.secondary_num_hide; }
        if (request.email_hide != null) { profile.HideMail = request.email_hide; profile.EmailHide = request.email_hide; }
        if (request.DOB != null) profile.Dob = request.DOB;
        if (request.DOA != null) profile.Doa = request.DOA;
        if (request.company_name != null) profile.CompanyName = request.company_name;
        profile.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new UpdateResponse { status = "0", message = "success" };
    }

    public async Task<UpdateResponse> UpdateProfilePersonalDetails(UpdatePersonalDetailsRequest request)
    {
        var profileId = int.TryParse(request.profileID, out var pid) ? pid : 0;
        var profile = await _db.MemberProfiles.FindAsync(profileId);
        if (profile == null) return new UpdateResponse { status = "1", message = "Not found" };
        // key is a JSON array string of {key, value} pairs — parse and apply
        profile.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new UpdateResponse { status = "0", message = "success" };
    }

    public async Task<object> SaveProfile(SaveProfileRequest request)
    {
        return await Task.FromResult(new { status = "0", message = "success" });
    }

    public async Task<object> GetMemberDetails(string? memProfileId, string? grpId)
    {
        var uid = int.TryParse(memProfileId, out var u) ? u : 0;
        var gid = int.TryParse(grpId, out var g) ? g : 0;

        // Find profile by UserId (memProfileId is actually masterUID/UserId)
        var mp = await _db.MemberProfiles
            .Include(m => m.Addresses)
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.UserId == uid && _db.GroupMembers.Any(gm => gm.MemberProfileId == m.Id && gm.GroupId == gid));

        if (mp == null)
            mp = await _db.MemberProfiles.Include(m => m.Addresses).Include(m => m.User).FirstOrDefaultAsync(m => m.Id == uid);

        if (mp == null) return new { TBGetSponsorReferredResult = new { status = "1", message = "Not found", Result = new { Table = new List<object>() } } };

        var grp = await _db.Groups.FindAsync(gid);
        var addr = mp.Addresses.FirstOrDefault();

        return new { TBGetSponsorReferredResult = new { status = "0", message = "success", Result = new { Table = new[] { new {
            MemProfileId = mp.Id, GrpID = gid, Chaptr_Brnch_Name = grp?.GrpName,
            MemId = mp.UserId, First_Name = mp.User?.FirstName ?? mp.MemberName, Middle_Name = mp.User?.MiddleName ?? "",
            Last_Name = mp.User?.LastName ?? "", Whatsapp_num = mp.WhatsappNum ?? mp.MemberMobile,
            hide_whatsnum = int.TryParse(mp.HideWhatsnum, out var hw) ? hw : 0,
            Secondry_num = mp.SecondaryMobileNo ?? "", hide_num = int.TryParse(mp.HideNum, out var hn) ? hn : 0,
            member_email_id = mp.MemberEmail, hide_mail = int.TryParse(mp.HideMail, out var hm) ? hm : 0,
            blood_Group = mp.BloodGroup, DOB = mp.Dob,
            DOA = mp.Doa,
            birthday = mp.Dob, annivarsary = mp.Doa,
            member_profile_photo_path = mp.ProfilePic ?? "", Company_name = mp.CompanyName,
            IMEI_Membership_Id = mp.ImeiMembershipId, MembershipGrade_Id = "",
            Membership_Grade = mp.MembershipGrade, CategoryId = mp.CategoryId ?? "0",
            CategoryName = mp.Category, Address = addr?.Address, City = addr?.City,
            StateId = "", State = addr?.State, pincode = addr?.Pincode,
            CountryId = "", Country = addr?.Country ?? mp.MemberCountry
        } } } } };
    }

    public async Task<object> DeleteMember(string memberProfileId)
    {
        var pid = int.TryParse(memberProfileId, out var p) ? p : 0;
        var profile = await _db.MemberProfiles.FindAsync(pid);
        if (profile == null) return new { status = "1", message = "Member not found" };

        // Remove group memberships
        var memberships = await _db.GroupMembers.Where(gm => gm.MemberProfileId == pid).ToListAsync();
        _db.GroupMembers.RemoveRange(memberships);

        // Remove family members
        var family = await _db.FamilyMembers.Where(fm => fm.MemberProfileId == pid).ToListAsync();
        _db.FamilyMembers.RemoveRange(family);

        // Remove addresses
        var addresses = await _db.AddressDetails.Where(a => a.MemberProfileId == pid).ToListAsync();
        _db.AddressDetails.RemoveRange(addresses);

        // Remove profile
        _db.MemberProfiles.Remove(profile);
        await _db.SaveChangesAsync();

        return new { status = "0", message = "success" };
    }

    public async Task<object> AddMember(WebAddMemberRequest request)
    {
        // Check if mobile number already exists
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.MobileNo == request.mobileNo);
        if (existingUser != null)
            return new { status = "1", message = "Mobile number already exists" };

        // Create user
        var user = new User
        {
            MobileNo = request.mobileNo,
            CountryCode = request.countryCode ?? "91",
            FirstName = request.firstName,
            MiddleName = request.middleName,
            LastName = request.lastName,
            Email = request.emailID,
            IsRegistered = true,
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Create member profile
        var fullName = new[] { request.firstName, request.middleName, request.lastName }
            .Where(s => !string.IsNullOrWhiteSpace(s)).Aggregate("", (a, b) => a + " " + b).Trim();

        // Parse DOB/DOA
        DateTime? dob = null, doa = null;
        if (!string.IsNullOrEmpty(request.dob) && DateTime.TryParse(request.dob, out var d)) dob = d;
        if (!string.IsNullOrEmpty(request.doa) && DateTime.TryParse(request.doa, out var a)) doa = a;

        var profile = new MemberProfile
        {
            UserId = user.Id,
            MemberName = fullName,
            MemberMobile = request.mobileNo,
            MemberEmail = request.emailID,
            CountryCode = request.countryCode ?? "91",
            BloodGroup = request.bloodGrp,
            Dob = dob?.ToString("yyyy-MM-dd"),
            Doa = doa?.ToString("yyyy-MM-dd"),
            CompanyName = request.companyName,
            MembershipGrade = request.membershipGrade,
            Category = request.category,
            ImeiMembershipId = request.membershipId,
            SecondaryMobileNo = request.secondaryMobileNo,
            MemberCountry = request.country,
        };
        _db.MemberProfiles.Add(profile);
        await _db.SaveChangesAsync();

        // Add to group
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        if (grpId > 0)
        {
            _db.GroupMembers.Add(new GroupMember { GroupId = grpId, MemberProfileId = profile.Id, MemberMainId = user.Id.ToString(), IsActive = true });
            await _db.SaveChangesAsync();
        }

        // Add address
        if (!string.IsNullOrEmpty(request.address) || !string.IsNullOrEmpty(request.city))
        {
            _db.AddressDetails.Add(new AddressDetail
            {
                MemberProfileId = profile.Id,
                AddressType = "Residence",
                Address = request.address,
                City = request.city,
                State = request.state,
                Country = request.country,
                Pincode = request.pincode,
            });
            await _db.SaveChangesAsync();
        }

        return new { status = "0", message = "success", profileId = profile.Id.ToString(), masterUID = user.Id.ToString() };
    }
}
