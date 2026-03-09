using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Event;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService) => _eventService = eventService;

    [HttpPost("GetEventDetails")]
    public async Task<IActionResult> GetEventDetails([FromBody] EventDetailRequest request)
    {
        try { return Ok(await _eventService.GetEventDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetEventList")]
    public async Task<IActionResult> GetEventList([FromBody] EventListRequest request)
    {
        try { return Ok(await _eventService.GetEventList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddEvent_New")]
    public async Task<IActionResult> AddEventNew([FromBody] AddEventRequest request)
    {
        try { return Ok(await _eventService.AddEvent(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AnsweringEvent")]
    public async Task<IActionResult> AnsweringEvent([FromBody] AnswerEventRequest request)
    {
        try { return Ok(await _eventService.AnswerEvent(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("Getsmscountdetails")]
    public async Task<IActionResult> GetSmsCountDetails([FromBody] SmsCountRequest request)
    {
        try { return Ok(await _eventService.GetSmsCountDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
