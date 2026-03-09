namespace TouchBase.API.Models.Entities;

public class Event
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? EventTitle { get; set; }
    public string? EventDesc { get; set; }
    public string? EventType { get; set; }
    public string? EventImageId { get; set; }
    public string? EventVenue { get; set; }
    public string? VenueLat { get; set; }
    public string? VenueLon { get; set; }
    public string? EventDate { get; set; }
    public string? PublishDate { get; set; }
    public string? ExpiryDate { get; set; }
    public string? NotifyDate { get; set; }
    public string? RepeatDateTime { get; set; }
    public string? RsvpEnable { get; set; }
    public string? QuestionEnable { get; set; }
    public string? QuestionType { get; set; }
    public string? QuestionText { get; set; }
    public string? Option1 { get; set; }
    public string? Option2 { get; set; }
    public string? DisplayOnBanner { get; set; }
    public string? RegLink { get; set; }
    public string? SendSMSNonSmartPh { get; set; }
    public string? SendSMSAll { get; set; }
    public string? InputIds { get; set; }
    public string? MembersIds { get; set; }
    public string? Link { get; set; }
    public string? ProjectName { get; set; }
    public string? FilterType { get; set; }
    public string? IsSubGrpAdmin { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public ICollection<EventResponse> Responses { get; set; } = new List<EventResponse>();
}
