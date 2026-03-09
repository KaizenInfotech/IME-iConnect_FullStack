namespace TouchBase.API.Models.Entities;

public class Announcement
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? AnnounTitle { get; set; }
    public string? AnnounDesc { get; set; }
    public string? AnnounType { get; set; }
    public string? AnnounImg { get; set; }
    public string? PublishDate { get; set; }
    public string? ExpiryDate { get; set; }
    public string? SendSMSNonSmartPh { get; set; }
    public string? SendSMSAll { get; set; }
    public string? ModuleId { get; set; }
    public string? RegLink { get; set; }
    public string? InputIds { get; set; }
    public string? RepeatDates { get; set; }
    public string? FilterType { get; set; }
    public string? IsSubGrpAdmin { get; set; }
    public string? Link { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
