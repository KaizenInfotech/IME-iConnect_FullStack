namespace TouchBase.API.Models.Entities;

public class Ebulletin
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? EbulletinTitle { get; set; }
    public string? EbulletinLink { get; set; }
    public string? EbulletinType { get; set; }
    public string? EbulletinFileId { get; set; }
    public string? PublishDate { get; set; }
    public string? ExpiryDate { get; set; }
    public string? SendSMSAll { get; set; }
    public string? InputIds { get; set; }
    public string? FilterType { get; set; }
    public string? IsSubGrpAdmin { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
