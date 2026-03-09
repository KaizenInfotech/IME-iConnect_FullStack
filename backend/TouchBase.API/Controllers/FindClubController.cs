using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.FindClub;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FindClubController : ControllerBase
{
    private readonly IFindClubService _findClubService;
    public FindClubController(IFindClubService findClubService) => _findClubService = findClubService;

    [HttpPost("GetClubList")]
    public async Task<IActionResult> GetClubList([FromBody] ClubSearchRequest request)
    {
        try { return Ok(await _findClubService.GetClubList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClubDetails")]
    public async Task<IActionResult> GetClubDetails([FromBody] ClubDetailRequest request)
    {
        try { return Ok(await _findClubService.GetClubDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClubsNearMe")]
    public async Task<IActionResult> GetClubsNearMe([FromBody] ClubNearMeRequest request)
    {
        try { return Ok(await _findClubService.GetClubsNearMe(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetPublicAlbumsList")]
    public async Task<IActionResult> GetPublicAlbumsList([FromBody] ClubDetailRequest request)
    {
        try { return Ok(await _findClubService.GetPublicAlbumsList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetPublicEventsList")]
    public async Task<IActionResult> GetPublicEventsList([FromBody] ClubDetailRequest request)
    {
        try { return Ok(await _findClubService.GetPublicEventsList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetPublicNewsletterList")]
    public async Task<IActionResult> GetPublicNewsletterList([FromBody] ClubDetailRequest request)
    {
        try { return Ok(await _findClubService.GetPublicNewsletterList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClubMembers")]
    public async Task<IActionResult> GetClubMembers([FromBody] ClubDetailRequest request)
    {
        try { return Ok(await _findClubService.GetClubMembers(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetCommitteelist")]
    public async Task<IActionResult> GetCommitteeList([FromBody] ClubDetailRequest request)
    {
        try { return Ok(await _findClubService.GetCommitteeList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
