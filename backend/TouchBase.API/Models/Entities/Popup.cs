namespace TouchBase.API.Models.Entities;

public class Popup
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public ICollection<PopupStatus> Statuses { get; set; } = new List<PopupStatus>();
}

public class PopupStatus
{
    public int Id { get; set; }
    public int PopupId { get; set; }
    public int MemberProfileId { get; set; }
    public bool IsSeen { get; set; }
    public DateTime SeenAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Popup Popup { get; set; } = null!;
}
