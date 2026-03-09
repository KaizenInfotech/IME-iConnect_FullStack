using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.District;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DistrictController : ControllerBase
{
    private readonly IDistrictService _districtService;
    public DistrictController(IDistrictService districtService) => _districtService = districtService;

    [HttpPost("GetDistrictMemberListSync")]
    public async Task<IActionResult> GetDistrictMemberListSync([FromBody] DistrictMemberListRequest request)
    {
        try { return Ok(await _districtService.GetDistrictMemberList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClubs")]
    public async Task<IActionResult> GetClubs([FromBody] DistrictClubsRequest request)
    {
        try { return Ok(await _districtService.GetClubs(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetDistrictCommittee")]
    public async Task<IActionResult> GetDistrictCommittee([FromBody] DistrictCommitteeRequest request)
    {
        try { return Ok(await _districtService.GetDistrictCommittee(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClassificationList_New")]
    public async Task<IActionResult> GetClassificationListNew([FromBody] ClassificationListRequest request)
    {
        try { return Ok(await _districtService.GetClassificationList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMemberByClassification")]
    public async Task<IActionResult> GetMemberByClassification([FromBody] MemberByClassificationRequest request)
    {
        try { return Ok(await _districtService.GetMemberByClassification(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMemberWithDynamicFields")]
    public async Task<IActionResult> GetMemberWithDynamicFields([FromBody] DistrictMemberDetailRequest request)
    {
        try { return Ok(await _districtService.GetMemberWithDynamicFields(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
