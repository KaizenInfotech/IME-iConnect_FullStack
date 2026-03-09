using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Celebrations;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CelebrationsController : ControllerBase
{
    private readonly ICelebrationsService _celebrationsService;
    public CelebrationsController(ICelebrationsService celebrationsService) => _celebrationsService = celebrationsService;

    [HttpPost("GetMonthEventList")]
    public async Task<IActionResult> GetMonthEventList([FromBody] MonthEventListRequest request)
    {
        try { return Ok(await _celebrationsService.GetMonthEventList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetEventMinDetails")]
    public async Task<IActionResult> GetEventMinDetails([FromBody] EventMinDetailRequest request)
    {
        try { return Ok(await _celebrationsService.GetEventMinDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetTodaysBirthday")]
    public async Task<IActionResult> GetTodaysBirthday([FromBody] TodaysBirthdayRequest request)
    {
        try { return Ok(await _celebrationsService.GetTodaysBirthday(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMonthEventListTypeWise_National")]
    public async Task<IActionResult> GetMonthEventListTypeWiseNational([FromBody] TypeWiseRequest request)
    {
        try { return Ok(await _celebrationsService.GetMonthEventListTypeWiseNational(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMonthEventListDetails_National")]
    public async Task<IActionResult> GetMonthEventListDetailsNational([FromBody] DateWiseRequest request)
    {
        try { return Ok(await _celebrationsService.GetMonthEventListDetailsNational(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}