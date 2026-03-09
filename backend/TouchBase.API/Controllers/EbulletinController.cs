using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Ebulletin;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EbulletinController : ControllerBase
{
    private readonly IEbulletinService _ebulletinService;
    public EbulletinController(IEbulletinService ebulletinService) => _ebulletinService = ebulletinService;

    [HttpPost("AddEbulletin")]
    public async Task<IActionResult> AddEbulletin([FromBody] AddEbulletinRequest request)
    {
        try { return Ok(await _ebulletinService.AddEbulletin(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetYearWiseEbulletinList")]
    public async Task<IActionResult> GetYearWiseEbulletinList([FromBody] EbulletinListRequest request)
    {
        try { return Ok(await _ebulletinService.GetYearWiseList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
