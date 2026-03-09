namespace TouchBase.API.Models.Entities;

public class EventResponse
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int MemberProfileId { get; set; }
    public string? JoiningStatus { get; set; }
    public string? AnswerByMe { get; set; }
    public string? QuestionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Event Event { get; set; } = null!;
    public MemberProfile MemberProfile { get; set; } = null!;
}
