using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Attendance;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    public AttendanceController(IAttendanceService attendanceService) => _attendanceService = attendanceService;

    [HttpPost("GetAttendanceListNew")]
    public async Task<IActionResult> GetAttendanceListNew([FromBody] AttendanceListRequest request)
    {
        try { return Ok(await _attendanceService.GetAttendanceListNew(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("getAttendanceDetails")]
    public async Task<IActionResult> GetAttendanceDetails([FromBody] AttendanceDetailRequest request)
    {
        try { return Ok(await _attendanceService.GetAttendanceDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AttendanceDelete")]
    public async Task<IActionResult> AttendanceDelete([FromBody] AttendanceDeleteRequest request)
    {
        try { return Ok(await _attendanceService.AttendanceDelete(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("getAttendanceMemberDetails")]
    public async Task<IActionResult> GetAttendanceMemberDetails([FromBody] AttendanceMemberDetailRequest request)
    {
        try { return Ok(await _attendanceService.GetAttendanceMemberDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("getAttendanceVisitorsDetails")]
    public async Task<IActionResult> GetAttendanceVisitorsDetails([FromBody] AttendanceMemberDetailRequest request)
    {
        try { return Ok(await _attendanceService.GetAttendanceVisitorsDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}