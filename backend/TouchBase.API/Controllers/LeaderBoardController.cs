using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Leaderboard;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderBoardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    public LeaderBoardController(ILeaderboardService leaderboardService) => _leaderboardService = leaderboardService;

    [HttpPost("GetLeaderBoardDetails")]
    public async Task<IActionResult> GetLeaderBoardDetails([FromBody] LeaderboardRequest request)
    {
        try { return Ok(await _leaderboardService.GetLeaderBoardDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
