namespace TouchBase.API.Models.Entities;

public class Document
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? DocTitle { get; set; }
    public string? DocType { get; set; }
    public string? DocURL { get; set; }
    public string? DocAccessType { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public ICollection<DocumentReadStatus> ReadStatuses { get; set; } = new List<DocumentReadStatus>();
}

public class DocumentReadStatus
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public int MemberProfileId { get; set; }
    public string? IsRead { get; set; }
    public DateTime ReadAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Document Document { get; set; } = null!;
    public MemberProfile MemberProfile { get; set; } = null!;
}
