namespace TouchBase.API.Models.Entities;

public class Feedback
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int MemberProfileId { get; set; }
    public string? FeedbackText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
