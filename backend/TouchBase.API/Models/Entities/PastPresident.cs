namespace TouchBase.API.Models.Entities;

public class PastPresident
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int? MemberProfileId { get; set; }
    public string? MemberName { get; set; }
    public string? PhotoPath { get; set; }
    public string? TenureYear { get; set; }
    public string? Designation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public MemberProfile? MemberProfile { get; set; }
}
