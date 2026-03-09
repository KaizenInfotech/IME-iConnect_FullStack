namespace TouchBase.API.Models.Entities;

public class Zone
{
    public int Id { get; set; }
    public string? ZoneName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; } = new List<LeaderboardEntry>();
}

public class Chapter
{
    public int Id { get; set; }
    public int ZoneId { get; set; }
    public string? ChapterName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Zone Zone { get; set; } = null!;
}
