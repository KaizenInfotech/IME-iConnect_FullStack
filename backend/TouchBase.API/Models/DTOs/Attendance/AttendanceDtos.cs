namespace TouchBase.API.Models.DTOs.Attendance;

// ─── Requests ───

public class AttendanceListRequest
{
    public string? GroupId { get; set; }
}

public class AttendanceDetailRequest
{
    public string? AttendanceID { get; set; }
}

public class AttendanceDeleteRequest
{
    public string? AttendanceID { get; set; }
    public string? createdBy { get; set; }
}

public class AttendanceMemberDetailRequest
{
    public string? AttendanceID { get; set; }
    public string? type { get; set; }
}

// ─── Responses ───

public class AttendanceListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public AttendanceListResultWrapper? Result { get; set; }
}

public class AttendanceListResultWrapper
{
    public List<AttendanceItemDto>? Table { get; set; }
}

public class AttendanceItemDto
{
    public string? AttendanceID { get; set; }
    public string? AttendanceName { get; set; }
    public string? AttendanceDate { get; set; }
    public string? Attendancetime { get; set; }
    public string? member_count { get; set; }
    public string? visitor_count { get; set; }
    public string? Description { get; set; }
}

public class AttendanceDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<AttendanceDetailDto>? AttendanceDetailsResult { get; set; }
}

public class AttendanceDetailDto
{
    public string? AttendanceID { get; set; }
    public string? AttendanceName { get; set; }
    public string? AttendanceDate { get; set; }
    public string? Attendancetime { get; set; }
    public string? AttendanceDesc { get; set; }
    public int? MemberCount { get; set; }
    public int? AnnsCount { get; set; }
    public int? AnnetsCount { get; set; }
    public int? VisitorsCount { get; set; }
    public int? RotarianCount { get; set; }
    public int? DistrictDelegatesCount { get; set; }
}

public class AttendanceMemberResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<AttendanceMemberDto>? Result { get; set; }
}

public class AttendanceMemberDto
{
    public string? FK_MemberID { get; set; }
    public string? MemberName { get; set; }
    public string? Designation { get; set; }
    public string? image { get; set; }
}

public class AttendanceVisitorResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<AttendanceVisitorDto>? Result { get; set; }
}

public class AttendanceVisitorDto
{
    public string? PK_AttendanceVisitorID { get; set; }
    public string? FK_AttendanceID { get; set; }
    public string? VisitorsName { get; set; }
}
