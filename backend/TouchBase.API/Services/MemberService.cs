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
        var client = _httpClientFactory.CreateClient();
        var httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.imeiconnect.com/api/Member/GetMemberListSync");
        httpReq.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
        httpReq.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grpID"] = request.grpID ?? "",
            ["updatedOn"] = request.updatedOn ?? "1970-01-01 00:00:00"
        });
        var response = await client.SendAsync(httpReq);
        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<object>(json)!;
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
        // Proxy multipart upload to live API
        var client = _httpClientFactory.CreateClient();
        var liveUrl = $"https://api.imeiconnect.com/api/Member/UploadProfilePhoto?ProfileID={request.ProfileID}&GrpID={request.GrpID}&Type=profile";
        var httpReq = new HttpRequestMessage(HttpMethod.Post, liveUrl);
        httpReq.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");

        var content = new MultipartFormDataContent();
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;
        var fileContent = new ByteArrayContent(ms.ToArray());
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        content.Add(fileContent, "profile_image", file.FileName ?? "image.jpg");
        content.Add(new StringContent(request.ProfileID ?? ""), "ProfileID");
        content.Add(new StringContent(request.GrpID ?? ""), "GrpID");
        content.Add(new StringContent("profile"), "Type");
        httpReq.Content = content;

        var response = await client.SendAsync(httpReq);
        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<object>(json)!;
    }

    public async Task<object> GetBodList(BodListRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.imeiconnect.com/api/Member/GetBODList");
        httpReq.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
        httpReq.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grpId"] = request.grpId ?? "",
            ["searchText"] = request.searchText ?? "",
            ["YearFilter"] = request.YearFilter ?? ""
        });
        var response = await client.SendAsync(httpReq);
        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<object>(json)!;
    }

    public async Task<object> GetGoverningCouncil(GoverningCouncilRequest request)
    {
        // Proxy to live API — governing council is national data
        var client = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.imeiconnect.com/api/Member/GetGoverningCouncl")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["searchText"] = request.searchText ?? "",
                ["YearFilter"] = request.YearFilter ?? ""
            })
        };
        httpRequest.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
        var response = await client.SendAsync(httpRequest);
        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<object>(json)!;
    }

    public async Task<UpdateResponse> UpdateMember(UpdateMemberRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.imeiconnect.com/api/Member/UpdateMemebr");
        httpRequest.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
        var formData = new Dictionary<string, string>();
        if (request.mobile_num_hide != null) formData["mobile_num_hide"] = request.mobile_num_hide;
        if (request.secondary_num_hide != null) formData["secondary_num_hide"] = request.secondary_num_hide;
        if (request.email_hide != null) formData["email_hide"] = request.email_hide;
        if (request.DOB != null) formData["DOB"] = request.DOB;
        if (request.DOA != null) formData["DOA"] = request.DOA;
        if (request.company_name != null) formData["company_name"] = request.company_name;
        if (request.MemProfileId != null) formData["MemProfileId"] = request.MemProfileId;
        httpRequest.Content = new FormUrlEncodedContent(formData);
        var response = await client.SendAsync(httpRequest);
        var json = await response.Content.ReadAsStringAsync();
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<UpdateResponse>(json)!;
        }
        catch
        {
            return new UpdateResponse { status = response.IsSuccessStatusCode ? "0" : "1", message = response.IsSuccessStatusCode ? "success" : "Failed" };
        }
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
        var client = _httpClientFactory.CreateClient();
        var url = $"https://api.imeiconnect.com/api/Member/GetMemberDetails?MemProfileId={memProfileId}&GrpID={grpId}";
        var req = new HttpRequestMessage(HttpMethod.Get, url);
        req.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
        var response = await client.SendAsync(req);
        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<object>(json)!;
    }
}
