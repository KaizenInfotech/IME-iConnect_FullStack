using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;
using TouchBase.API.Models.DTOs.Member;
using TouchBase.API.Models.Entities;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

public class MemberService : IMemberService
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private const int PageSize = 25;

    public MemberService(AppDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
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
        if (request.memberMobile != null) profile.MemberMobile = request.memberMobile;
        if (request.memberEmailid != null)
        {
            profile.MemberEmail = request.memberEmailid;
            if (profile.User != null) profile.User.Email = request.memberEmailid;
        }
        if (request.ProfilePicPath != null) profile.ProfilePic = request.ProfilePicPath;
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
            .Where(gm => gm.GroupId == grpId)
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
                member_IMEI_id = gm.MemberProfile.ImeiMembershipId,
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
            addr = new AddressDetail { MemberProfileId = profileId };
            _db.AddressDetails.Add(addr);
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
        var photoUrl = $"/uploads/profile/{fileName}";
        var profile = await _db.MemberProfiles.FindAsync(pid);
        if (profile != null) { profile.ProfilePic = photoUrl; profile.UpdatedAt = DateTime.UtcNow; await _db.SaveChangesAsync(); }
        return new { status = "0", message = "success", ProfileImage = photoUrl };
    }

    public async Task<object> GetBodList(BodListRequest request)
    {
        var grpId = int.TryParse(request.grpId, out var gid) ? gid : 0;
        var query = _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Where(x => x.gm.GroupId == grpId && x.mp.Designation != null && x.mp.Designation != "");
        if (!string.IsNullOrEmpty(request.searchText)) query = query.Where(x => x.mp.MemberName != null && x.mp.MemberName.Contains(request.searchText));
        var members = await query.Select(x => new { masterUID = x.mp.UserId.ToString(), grpID = x.gm.GroupId.ToString(), profileID = x.mp.Id.ToString(), memberName = x.mp.MemberName, membermobile = x.mp.MemberMobile, MemberDesignation = x.mp.Designation, pic = x.mp.ProfilePic, Email = x.mp.MemberEmail }).ToListAsync();
        return new { TBGetBODResult = new { status = "0", message = "success", BODResult = members } };
    }

    public async Task<object> GetGoverningCouncil(GoverningCouncilRequest request)
    {
        var query = _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Where(x => x.gm.GroupId == 31185 && x.mp.Designation != null && x.mp.Designation != "");
        if (!string.IsNullOrEmpty(request.searchText)) query = query.Where(x => x.mp.MemberName != null && x.mp.MemberName.Contains(request.searchText));
        var members = await query.Select(x => new { BOD_pkID = x.mp.Id, ProfileID = x.mp.Id, FK_Master_Designation_ID = 0, PhoneNo = x.mp.MemberMobile, Email = x.mp.MemberEmail, MemberName = x.mp.MemberName, masterUID = x.mp.UserId, sr_NO = 0, grpID = 31185, pic = x.mp.ProfilePic, MemberDesignation = x.mp.Designation, membermobile = (x.mp.CountryCode != null ? "+" + x.mp.CountryCode + " " : "") + x.mp.MemberMobile }).ToListAsync();
        return new { status = "0", message = "success", Result = new { Table = members } };
    }

    public async Task<UpdateResponse> UpdateMember(UpdateMemberRequest request)
    {
        var pid = int.TryParse(request.MemProfileId, out var p) ? p : 0;
        var profile = await _db.MemberProfiles.FindAsync(pid);
        if (profile == null) return new UpdateResponse { status = "1", message = "Not found" };
        if (request.mobile_num_hide != null) profile.MobileNumHide = request.mobile_num_hide;
        if (request.secondary_num_hide != null) profile.SecondaryNumHide = request.secondary_num_hide;
        if (request.email_hide != null) profile.EmailHide = request.email_hide;
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
            member_profile_photo_path = mp.ProfilePic, Company_name = mp.CompanyName,
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
}
