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
        try { return Ok(await _memberService.UploadProfilePhoto(form.file!, new ProfilePhotoRequest { ProfileID = form.ProfileID })); }
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

    [HttpPost("GetMemberDetails")]
    public async Task<IActionResult> GetMemberDetails([FromBody] MemberDetailRequest request)
    {
        try { return Ok(await _memberService.GetMember(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("Saveprofile")]
    public async Task<IActionResult> SaveProfile([FromBody] SaveProfileRequest request)
    {
        try { return Ok(await _memberService.SaveProfile(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
