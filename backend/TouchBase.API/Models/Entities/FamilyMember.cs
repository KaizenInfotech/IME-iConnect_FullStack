namespace TouchBase.API.Models.Entities;

public class FamilyMember
{
    public int Id { get; set; }
    public int MemberProfileId { get; set; }
    public string? MemberName { get; set; }
    public string? Relationship { get; set; }
    public string? Dob { get; set; }
    public string? Anniversary { get; set; }
    public string? ContactNo { get; set; }
    public string? Particulars { get; set; }
    public string? EmailId { get; set; }
    public string? BloodGroup { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public MemberProfile MemberProfile { get; set; } = null!;
}
