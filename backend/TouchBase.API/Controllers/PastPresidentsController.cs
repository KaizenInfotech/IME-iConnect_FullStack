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

    [HttpPost("AddPastPresident")]
    public async Task<IActionResult> AddPastPresident([FromBody] AddPastPresidentRequest request)
    {
        try { return Ok(await _pastPresidentService.AddPastPresident(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdatePastPresident")]
    public async Task<IActionResult> UpdatePastPresident([FromBody] UpdatePastPresidentRequest request)
    {
        try { return Ok(await _pastPresidentService.UpdatePastPresident(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeletePastPresident")]
    public async Task<IActionResult> DeletePastPresident([FromBody] DeletePastPresidentRequest request)
    {
        try { return Ok(await _pastPresidentService.DeletePastPresident(request.PastPresidentId ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}

public class AddPastPresidentRequest
{
    public string? GroupId { get; set; }
    public string? MemberName { get; set; }
    public string? PhotoPath { get; set; }
    public string? TenureYear { get; set; }
    public string? Designation { get; set; }
}

public class UpdatePastPresidentRequest
{
    public string? PastPresidentId { get; set; }
    public string? MemberName { get; set; }
    public string? PhotoPath { get; set; }
    public string? TenureYear { get; set; }
    public string? Designation { get; set; }
}

public class DeletePastPresidentRequest
{
    public string? PastPresidentId { get; set; }
}