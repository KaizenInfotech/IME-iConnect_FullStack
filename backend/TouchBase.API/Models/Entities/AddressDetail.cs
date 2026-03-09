namespace TouchBase.API.Models.Entities;

public class AddressDetail
{
    public int Id { get; set; }
    public int MemberProfileId { get; set; }
    public string? AddressType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Pincode { get; set; }
    public string? PhoneNo { get; set; }
    public string? Fax { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public MemberProfile MemberProfile { get; set; } = null!;
}
