namespace TouchBase.API.Models.Entities;

public class AttendanceRecord
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? AttendanceName { get; set; }
    public string? AttendanceDate { get; set; }
    public string? AttendanceTime { get; set; }
    public string? AttendanceDesc { get; set; }
    public int? MemberCount { get; set; }
    public int? VisitorCount { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public ICollection<AttendanceMember> AttendanceMembers { get; set; } = new List<AttendanceMember>();
    public ICollection<AttendanceVisitor> AttendanceVisitors { get; set; } = new List<AttendanceVisitor>();
}

public class AttendanceMember
{
    public int Id { get; set; }
    public int AttendanceRecordId { get; set; }
    public int MemberProfileId { get; set; }
    public string? Type { get; set; } // 1=Member, 2=Visitor, 3=Anns, 4=Annets, 5=Rotarian, 6=DistrictDelegate
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public AttendanceRecord AttendanceRecord { get; set; } = null!;
    public MemberProfile MemberProfile { get; set; } = null!;
}

public class AttendanceVisitor
{
    public int Id { get; set; }
    public int AttendanceRecordId { get; set; }
    public string? VisitorName { get; set; }
    public string? Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public AttendanceRecord AttendanceRecord { get; set; } = null!;
}
