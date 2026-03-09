namespace TouchBase.API.Models.Entities;

public class Banner
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? BannerImage { get; set; }
    public string? BannerTitle { get; set; }
    public string? BannerDescription { get; set; }
    public string? BannerUrl { get; set; }
    public string? BannerType { get; set; } // "banner" or "slider"
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
