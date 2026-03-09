using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;
using TouchBase.API.Models.DTOs.Member;
using TouchBase.API.Models.Entities;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

public class MemberService : IMemberService
{
    private readonly AppDbContext _db;
    private const int PageSize = 25;

    public MemberService(AppDbContext db) => _db = db;

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
        var profile = await _db.MemberProfiles.FindAsync(profileId);
        if (profile == null) return new UpdateResponse { status = "1", message = "Profile not found" };

        if (request.memberName != null) profile.MemberName = request.memberName;
        if (request.memberMobile != null) profile.MemberMobile = request.memberMobile;
        if (request.memberEmailid != null) profile.MemberEmail = request.memberEmailid;
        if (request.ProfilePicPath != null) profile.ProfilePic = request.ProfilePicPath;
        profile.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new UpdateResponse { status = "0", message = "success" };
    }

    public async Task<object> GetMemberListSync(MemberSyncRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var members = await _db.GroupMembers.Include(gm => gm.MemberProfile)
            .Where(gm => gm.GroupId == grpId && gm.IsActive)
            .Select(gm => new { gm.MemberProfileId, gm.MemberProfile.MemberName, gm.MemberProfile.MemberMobile, gm.MemberProfile.ProfilePic, gm.MemberProfile.Designation })
            .ToListAsync();
        return new { status = "0", message = "success", data = members };
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
        var profileId = int.TryParse(request.ProfileID, out var pid) ? pid : 0;
        var profile = await _db.MemberProfiles.FindAsync(profileId);
        if (profile == null) return new { status = "1", message = "Profile not found" };

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var uploadPath = Path.Combine("wwwroot", "uploads", "profile");
        Directory.CreateDirectory(uploadPath);
        var filePath = Path.Combine(uploadPath, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        profile.ProfilePic = $"/uploads/profile/{fileName}";
        profile.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success", profilePic = profile.ProfilePic };
    }

    public async Task<BodListResponse> GetBodList(BodListRequest request)
    {
        var grpId = int.TryParse(request.grpId, out var gid) ? gid : 0;
        var query = _db.GroupMembers.Include(gm => gm.MemberProfile).Where(gm => gm.GroupId == grpId && gm.IsGrpAdmin == "Yes");
        if (!string.IsNullOrEmpty(request.searchText))
            query = query.Where(gm => gm.MemberProfile.MemberName!.Contains(request.searchText));

        var members = await query.Select(gm => new BodMemberDto
        {
            ProfileID = gm.MemberProfileId.ToString(), MemberName = gm.MemberProfile.MemberName,
            MemberDesignation = gm.MemberProfile.Designation, pic = gm.MemberProfile.ProfilePic,
            masterUID = gm.MemberProfile.UserId.ToString(), grpID = gm.GroupId.ToString(),
            membermobile = gm.MemberProfile.MemberMobile, Email = gm.MemberProfile.MemberEmail
        }).ToListAsync();

        return new BodListResponse { status = "0", message = "success", BODResult = members };
    }

    public async Task<GoverningCouncilResponse> GetGoverningCouncil(GoverningCouncilRequest request)
    {
        var members = await _db.GroupMembers.Include(gm => gm.MemberProfile)
            .Where(gm => gm.IsGrpAdmin == "Yes")
            .Select(gm => new CouncilMemberDto
            {
                ProfileID = gm.MemberProfileId.ToString(), MemberName = gm.MemberProfile.MemberName,
                MemberDesignation = gm.MemberProfile.Designation, pic = gm.MemberProfile.ProfilePic,
                masterUID = gm.MemberProfile.UserId.ToString(), grpID = gm.GroupId.ToString(),
                membermobile = gm.MemberProfile.MemberMobile, Email = gm.MemberProfile.MemberEmail
            }).ToListAsync();
        return new GoverningCouncilResponse { status = "0", message = "success", Result = new GoverningCouncilResult { Table = members } };
    }

    public async Task<UpdateResponse> UpdateMember(UpdateMemberRequest request)
    {
        var profileId = int.TryParse(request.MemProfileId, out var pid) ? pid : 0;
        var profile = await _db.MemberProfiles.FindAsync(profileId);
        if (profile == null) return new UpdateResponse { status = "1", message = "Not found" };

        profile.MobileNumHide = request.mobile_num_hide; profile.SecondaryNumHide = request.secondary_num_hide;
        profile.EmailHide = request.email_hide; profile.CompanyNameHide = request.company_name;
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
}
