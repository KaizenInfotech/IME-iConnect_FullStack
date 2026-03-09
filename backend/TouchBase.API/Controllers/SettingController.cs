using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Settings;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

// Note: Flutter uses both "setting" (lowercase) and "Setting" (uppercase) in routes
// ASP.NET Core routing is case-insensitive by default, so this handles both
[Route("api/[controller]")]
[ApiController]
public class SettingController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    public SettingController(ISettingsService settingsService) => _settingsService = settingsService;

    [HttpPost("GetTouchbaseSetting")]
    public async Task<IActionResult> GetTouchbaseSetting([FromBody] TouchbaseSettingRequest request)
    {
        try { return Ok(await _settingsService.GetTouchbaseSetting(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("TouchbaseSetting")]
    public async Task<IActionResult> UpdateTouchbaseSetting([FromBody] UpdateTouchbaseSettingRequest request)
    {
        try { return Ok(await _settingsService.UpdateTouchbaseSetting(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GroupSetting")]
    public async Task<IActionResult> UpdateGroupSetting([FromBody] UpdateGroupSettingRequest request)
    {
        try { return Ok(await _settingsService.UpdateGroupSetting(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetGroupSetting")]
    public async Task<IActionResult> GetGroupSetting([FromBody] GroupSettingRequest request)
    {
        try { return Ok(await _settingsService.GetGroupSetting(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
