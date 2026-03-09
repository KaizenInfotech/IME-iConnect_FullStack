namespace TouchBase.API.Models.Entities;

public class WebLink
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? Title { get; set; }
    public string? FullDesc { get; set; }
    public string? LinkUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
