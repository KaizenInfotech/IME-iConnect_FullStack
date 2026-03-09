using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.FindRotarian;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FindRotarianController : ControllerBase
{
    private readonly IFindRotarianService _findRotarianService;
    public FindRotarianController(IFindRotarianService findRotarianService) => _findRotarianService = findRotarianService;

    [HttpPost("GetZonechapterlist")]
    public async Task<IActionResult> GetZoneChapterList()
    {
        try { return Ok(await _findRotarianService.GetZoneChapterList()); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetRotarianList")]
    public async Task<IActionResult> GetRotarianList([FromBody] RotarianSearchRequest request)
    {
        try { return Ok(await _findRotarianService.GetRotarianList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetRotarianDetails")]
    public async Task<IActionResult> GetRotarianDetails([FromBody] RotarianDetailRequest request)
    {
        try { return Ok(await _findRotarianService.GetRotarianDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetCategoryList")]
    public async Task<IActionResult> GetCategoryList()
    {
        try { return Ok(await _findRotarianService.GetCategoryList()); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMemberGradeList")]
    public async Task<IActionResult> GetMemberGradeList()
    {
        try { return Ok(await _findRotarianService.GetMemberGradeList()); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClubList")]
    public async Task<IActionResult> GetClubList()
    {
        try { return Ok(await _findRotarianService.GetClubList()); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
