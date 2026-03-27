namespace TouchBase.API.Models.Entities;

public class WebAdmin
{
    public int Id { get; set; }
    public string MobileNo { get; set; } = "";
    public string Password { get; set; } = "";
    public string? UserRole { get; set; }
    public int? UserId { get; set; }
    public string? CountryCode { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}