using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Auth;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IAuthService _authService;
    public LoginController(IAuthService authService) => _authService = authService;

    [AllowAnonymous]
    [HttpPost("UserLogin")]
    public async Task<IActionResult> UserLogin([FromForm] LoginRequest request)
    {
        try { return Ok(await _authService.RequestOtp(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [AllowAnonymous]
    [HttpPost("PostOTP")]
    public async Task<IActionResult> PostOTP([FromForm] OtpVerifyRequest request)
    {
        try { return Ok(await _authService.VerifyOtp(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [AllowAnonymous]
    [HttpPost("GetWelcomeScreen")]
    public async Task<IActionResult> GetWelcomeScreen([FromForm] WelcomeScreenRequest request)
    {
        try { return Ok(await _authService.GetWelcomeGroups(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [AllowAnonymous]
    [HttpPost("GetMemberDetails")]
    public async Task<IActionResult> GetMemberDetails([FromForm] MemberDetailsRequest request)
    {
        try { return Ok(await _authService.GetMemberDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [AllowAnonymous]
    [HttpPost("Registration")]
    public async Task<IActionResult> Registration([FromForm] RegistrationRequest request)
    {
        try { return Ok(await _authService.Register(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
