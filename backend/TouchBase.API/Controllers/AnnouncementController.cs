using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Announcement;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;
    public AnnouncementController(IAnnouncementService announcementService) => _announcementService = announcementService;

    [HttpPost("GetAnnouncementList")]
    public async Task<IActionResult> GetAnnouncementList([FromBody] AnnouncementListRequest request)
    {
        try { return Ok(await _announcementService.GetAnnouncementList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAnnouncementDetails")]
    public async Task<IActionResult> GetAnnouncementDetails([FromBody] AnnouncementDetailRequest request)
    {
        try { return Ok(await _announcementService.GetAnnouncementDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddAnnouncement")]
    public async Task<IActionResult> AddAnnouncement([FromBody] AddAnnouncementRequest request)
    {
        try { return Ok(await _announcementService.AddAnnouncement(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeleteAnnouncement")]
    public async Task<IActionResult> DeleteAnnouncement([FromBody] DeleteAnnouncementRequest request)
    {
        try { return Ok(await _announcementService.DeleteAnnouncement(request.announID ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}

public class DeleteAnnouncementRequest { public string? announID { get; set; } }
