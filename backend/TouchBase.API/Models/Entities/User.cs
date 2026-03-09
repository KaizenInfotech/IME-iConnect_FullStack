namespace TouchBase.API.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string? MobileNo { get; set; }
    public string? CountryCode { get; set; }
    public string? DeviceToken { get; set; }
    public string? DeviceName { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfileImage { get; set; }
    public string? ImeiMemId { get; set; }
    public string? ImeiNo { get; set; }
    public string? VersionNo { get; set; }
    public string? LoginType { get; set; }
    public string? Otp { get; set; }
    public DateTime? OtpExpiry { get; set; }
    public bool IsRegistered { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<MemberProfile> MemberProfiles { get; set; } = new List<MemberProfile>();
    public ICollection<DeviceToken> DeviceTokens { get; set; } = new List<DeviceToken>();
    public ICollection<TouchbaseSetting> TouchbaseSettings { get; set; } = new List<TouchbaseSetting>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
