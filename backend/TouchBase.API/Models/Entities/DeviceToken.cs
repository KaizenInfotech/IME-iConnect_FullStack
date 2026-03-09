namespace TouchBase.API.Models.Entities;

public class DeviceToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Token { get; set; }
    public string? Platform { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}
