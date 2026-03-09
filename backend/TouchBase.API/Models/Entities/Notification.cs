namespace TouchBase.API.Models.Entities;

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public string? Type { get; set; }
    public string? ClubDistrictType { get; set; }
    public string? NotifyDate { get; set; }
    public string? ExpiryDate { get; set; }
    public string? SortDate { get; set; }
    public string? ReadStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}
