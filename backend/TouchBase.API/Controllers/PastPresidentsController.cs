using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.PastPresident;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PastPresidentsController : ControllerBase
{
    private readonly IPastPresidentService _pastPresidentService;
    public PastPresidentsController(IPastPresidentService pastPresidentService) => _pastPresidentService = pastPresidentService;

    [HttpPost("getPastPresidentsList")]
    public async Task<IActionResult> GetPastPresidentsList([FromBody] PastPresidentRequest request)
    {
        try { return Ok(await _pastPresidentService.GetPastPresidentsList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}