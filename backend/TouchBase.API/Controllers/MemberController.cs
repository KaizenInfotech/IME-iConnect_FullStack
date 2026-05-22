using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Member;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;
    public MemberController(IMemberService memberService) => _memberService = memberService;

    [HttpPost("UpdateProfile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        try { return Ok(await _memberService.UpdateProfile(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetDirectoryList")]
    public async Task<IActionResult> GetDirectoryList([FromBody] DirectoryListRequest request)
    {
        try { return Ok(await _memberService.GetDirectoryList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMember")]
    public async Task<IActionResult> GetMember([FromBody] MemberDetailRequest request)
    {
        try { return Ok(await _memberService.GetMember(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMemberListSync")]
    public async Task<IActionResult> GetMemberListSync([FromBody] MemberSyncRequest request)
    {
        try { return Ok(await _memberService.GetMemberListSync(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAllMembersPaged")]
    public async Task<IActionResult> GetAllMembersPaged([FromBody] AllMembersPagedRequest request)
    {
        try { return Ok(await _memberService.GetAllMembersPaged(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: lowercase 'member' in route to match Flutter ApiConstants
    [HttpPost("/api/member/UpdateProfileDetails")]
    public async Task<IActionResult> UpdateProfileDetails([FromBody] UpdatePersonalDetailsRequest request)
    {
        try { return Ok(await _memberService.UpdateProfileDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateAddressDetails")]
    public async Task<IActionResult> UpdateAddressDetails([FromBody] UpdateAddressRequest request)
    {
        try { return Ok(await _memberService.UpdateAddressDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateFamilyDetails")]
    public async Task<IActionResult> UpdateFamilyDetails([FromBody] UpdateFamilyRequest request)
    {
        try { return Ok(await _memberService.UpdateFamilyDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: iOS typo "Updatedmember" preserved in route
    [HttpPost("GetUpdatedmemberProfileDetails")]
    public async Task<IActionResult> GetUpdatedProfileDetails([FromBody] GetUpdatedProfileRequest request)
    {
        try { return Ok(await _memberService.GetUpdatedProfileDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UploadProfilePhoto")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfilePhoto([FromForm] TouchBase.API.Models.DTOs.Upload.ProfilePhotoFormRequest form)
    {
        try {
            var uploadFile = form.profile_image ?? form.file;
            if (uploadFile == null) return Ok(new { status = "1", message = "No file provided" });
            return Ok(await _memberService.UploadProfilePhoto(uploadFile, new ProfilePhotoRequest { ProfileID = form.ProfileID, GrpID = form.GrpID }));
        }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetBODList")]
    public async Task<IActionResult> GetBodList([FromBody] BodListRequest request)
    {
        try { return Ok(await _memberService.GetBodList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: iOS typo "GetGoverningCouncl" preserved
    [HttpPost("GetGoverningCouncl")]
    public async Task<IActionResult> GetGoverningCouncil([FromBody] GoverningCouncilRequest request)
    {
        try { return Ok(await _memberService.GetGoverningCouncil(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("ReorderBOD")]
    public async Task<IActionResult> ReorderBOD([FromBody] List<ReorderItem> items)
    {
        try { return Ok(await _memberService.ReorderBOD(items)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetBODDetails")]
    public async Task<IActionResult> GetBODDetails([FromBody] BODDetailRequest request)
    {
        try { return Ok(await _memberService.GetBODDetails(request.BOD_PkID ?? "", request.YearFilter ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeleteBOD")]
    public async Task<IActionResult> DeleteBOD([FromBody] DeleteBODRequest request)
    {
        try { return Ok(await _memberService.DeleteBOD(request.BOD_PkID ?? "", request.YearFilter ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateBOD")]
    public async Task<IActionResult> UpdateBOD([FromBody] UpdateBODRequest request)
    {
        try { return Ok(await _memberService.UpdateBOD(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: iOS typo "UpdateMemebr" preserved
    [HttpPost("UpdateMemebr")]
    public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberRequest request)
    {
        try { return Ok(await _memberService.UpdateMember(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateProfilePersonalDetails")]
    public async Task<IActionResult> UpdateProfilePersonalDetails([FromBody] UpdatePersonalDetailsRequest request)
    {
        try { return Ok(await _memberService.UpdateProfilePersonalDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpGet("GetMemberDetails")]
    public async Task<IActionResult> GetMemberDetails([FromQuery] string? MemProfileId, [FromQuery] string? GrpID)
    {
        try { return Ok(await _memberService.GetMemberDetails(MemProfileId, GrpID)); }
        catch (Exception ex) { return Ok(new { TBGetSponsorReferredResult = new { status = "1", message = ex.Message } }); }
    }

    [HttpPost("Saveprofile")]
    public async Task<IActionResult> SaveProfile([FromBody] SaveProfileRequest request)
    {
        try { return Ok(await _memberService.SaveProfile(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeleteMember")]
    public async Task<IActionResult> DeleteMember([FromBody] DeleteMemberRequest request)
    {
        try { return Ok(await _memberService.DeleteMember(request.memberProfileId ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddMember")]
    public async Task<IActionResult> AddMember([FromBody] WebAddMemberRequest request)
    {
        try { return Ok(await _memberService.AddMember(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}

public class DeleteMemberRequest
{
    public string? memberProfileId { get; set; }
}

public class WebAddMemberRequest
{
    public string? grpID { get; set; }
    public string? firstName { get; set; }
    public string? middleName { get; set; }
    public string? lastName { get; set; }
    public string? mobileNo { get; set; }
    public string? emailID { get; set; }
    public string? countryCode { get; set; }
    public string? dob { get; set; }
    public string? doa { get; set; }
    public string? bloodGrp { get; set; }
    public string? secondaryMobileNo { get; set; }
    public string? membershipId { get; set; }
    public string? membershipGrade { get; set; }
    public string? category { get; set; }
    public string? companyName { get; set; }
    public string? chapterBranchName { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? pincode { get; set; }
    public string? country { get; set; }
}

public class ReorderItem { public int MemberId { get; set; } public int DisplayOrder { get; set; } }
public class BODDetailRequest { public string? BOD_PkID { get; set; } public string? YearFilter { get; set; } }
public class DeleteBODRequest { public string? BOD_PkID { get; set; } public string? YearFilter { get; set; } }
public class UpdateBODRequest
{
    public string? BOD_PkID { get; set; }
    public string? memberProfileID { get; set; }
    public string? groupId { get; set; }
    public string? designation { get; set; }
    public string? otherDesignation { get; set; }
    public string? name { get; set; }
    public string? emailID { get; set; }
    public string? phoneNo { get; set; }
    public string? yearFilter { get; set; }
    public string? chapterId { get; set; }
}
