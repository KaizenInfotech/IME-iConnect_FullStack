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

    [HttpPost("AttendanceAddEdit")]
    public async Task<IActionResult> AttendanceAddEdit([FromBody] AttendanceAddEditRequest request)
    {
        try { return Ok(await _attendanceService.AttendanceAddEdit(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}

public class AttendanceAddEditRequest
{
    public string? AttendanceID { get; set; }
    public string? GroupId { get; set; }
    public string? AttendanceName { get; set; }
    public string? AttendanceDesc { get; set; }
    public string? AttendanceDate { get; set; }
    public List<AttendanceMemberInput>? Members { get; set; }
    public List<AttendanceVisitorInput>? Visitors { get; set; }
}

public class AttendanceMemberInput
{
    public string? Id { get; set; }
    public string? MemberProfileId { get; set; }
    public string? Type { get; set; }
}

public class AttendanceVisitorInput
{
    public string? VisitorName { get; set; }
    public string? Type { get; set; }
}