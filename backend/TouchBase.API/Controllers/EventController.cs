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

    [HttpPost("DeleteEvent")]
    public async Task<IActionResult> DeleteEvent([FromBody] DeleteEventRequest request)
    {
        try { return Ok(await _eventService.DeleteEvent(request.eventId ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetEventExtras")]
    public async Task<IActionResult> GetEventExtras([FromBody] EventExtrasRequest request)
    {
        try { return Ok(await _eventService.GetEventExtras(request.eventID ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("SaveEventExtras")]
    public async Task<IActionResult> SaveEventExtras([FromBody] SaveEventExtrasRequest request)
    {
        try { return Ok(await _eventService.SaveEventExtras(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UploadEventDoc")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadEventDoc([FromForm] TouchBase.API.Models.DTOs.Upload.UploadEventDocFormRequest request)
    {
        try { return Ok(await _eventService.UploadEventDoc(request.file!, request.eventID ?? "", request.docType ?? "agenda")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}

public class DeleteEventRequest { public string? eventId { get; set; } }
public class EventExtrasRequest { public string? eventID { get; set; } }
public class SaveEventExtrasRequest
{
    public string? eventID { get; set; }
    public List<EventPhotoInput>? photos { get; set; }
    public List<string>? agendaFileNames { get; set; }
    public List<string>? minutesFileNames { get; set; }
}
public class EventPhotoInput { public string? photoPath { get; set; } public string? description { get; set; } }
