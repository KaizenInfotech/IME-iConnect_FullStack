namespace TouchBase.API.Models.Entities;

public class AlbumPhoto
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Album Album { get; set; } = null!;
}
