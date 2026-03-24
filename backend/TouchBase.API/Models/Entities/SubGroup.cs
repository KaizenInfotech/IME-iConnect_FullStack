namespace TouchBase.API.Models.Entities;

public class SubGroup
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? SubgrpTitle { get; set; }
    public int? ParentId { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public ICollection<SubGroupMember> Members { get; set; } = new List<SubGroupMember>();
}

public class SubGroupMember
{
    public int Id { get; set; }
    public int SubGroupId { get; set; }
    public int MemberProfileId { get; set; }
    public string? MemberName { get; set; }
    public string? Designation { get; set; }
    public string? MobileNumber { get; set; }
    public string? EmailId { get; set; }
    public string? Branch { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public SubGroup SubGroup { get; set; } = null!;
    public MemberProfile MemberProfile { get; set; } = null!;
}
