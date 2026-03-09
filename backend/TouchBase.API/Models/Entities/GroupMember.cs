namespace TouchBase.API.Models.Entities;

public class GroupMember
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int MemberProfileId { get; set; }
    public string? MyCategory { get; set; }
    public string? IsGrpAdmin { get; set; }
    public string? GrpProfileId { get; set; }
    public string? ModuleId { get; set; }
    public string? MemberMainId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public MemberProfile MemberProfile { get; set; } = null!;
}
