namespace TouchBase.API.Models.Entities;

public class Group
{
    public int Id { get; set; }
    public string? GrpName { get; set; }
    public string? GrpImg { get; set; }
    public string? GrpImageId { get; set; }
    public string? GrpType { get; set; }
    public string? GrpCategory { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Pincode { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Website { get; set; }
    public string? Other { get; set; }
    public int TotalMembers { get; set; }
    public int? DistrictId { get; set; }
    public string? ClubHistory { get; set; }
    public string? ContactNo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public District? District { get; set; }
    public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    public ICollection<GroupModule> Modules { get; set; } = new List<GroupModule>();
    public ICollection<SubGroup> SubGroups { get; set; } = new List<SubGroup>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    public ICollection<Ebulletin> Ebulletins { get; set; } = new List<Ebulletin>();
    public ICollection<Album> Albums { get; set; } = new List<Album>();
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    public ICollection<ServiceDirectoryEntry> ServiceDirectoryEntries { get; set; } = new List<ServiceDirectoryEntry>();
    public ICollection<WebLink> WebLinks { get; set; } = new List<WebLink>();
    public ICollection<PastPresident> PastPresidents { get; set; } = new List<PastPresident>();
    public ICollection<Banner> Banners { get; set; } = new List<Banner>();
    public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; } = new List<LeaderboardEntry>();
    public ICollection<MerItem> MerItems { get; set; } = new List<MerItem>();
    public ICollection<Popup> Popups { get; set; } = new List<Popup>();
    public ICollection<GroupSetting> GroupSettings { get; set; } = new List<GroupSetting>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
