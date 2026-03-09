namespace TouchBase.API.Models.DTOs.Announcement;

// ─── Requests ───

public class AnnouncementListRequest
{
    public string? groupId { get; set; }
    public string? memberProfileId { get; set; }
    public string? searchText { get; set; }
    public string? moduleId { get; set; }
}

public class AnnouncementDetailRequest
{
    public string? announID { get; set; }
    public string? grpID { get; set; }
    public string? memberProfileID { get; set; }
}

public class AddAnnouncementRequest
{
    public string? announID { get; set; }
    public string? annType { get; set; }
    public string? announTitle { get; set; }
    public string? announceDEsc { get; set; }
    public string? memID { get; set; }
    public string? grpID { get; set; }
    public string? inputIDs { get; set; }
    public string? announImg { get; set; }
    public string? publishDate { get; set; }
    public string? expiryDate { get; set; }
    public string? sendSMSNonSmartPh { get; set; }
    public string? sendSMSAll { get; set; }
    public string? moduleId { get; set; }
    public string? AnnouncementRepeatDates { get; set; }
    public string? reglink { get; set; }
    public string? isSubGrpAdmin { get; set; }
}

// ─── Responses ───

public class AnnouncementListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? smscount { get; set; }
    public List<AnnouncementItemDto>? AnnounceList { get; set; }
}

public class AnnouncementItemDto
{
    public string? announID { get; set; }
    public string? announTitle { get; set; }
    public string? announceDEsc { get; set; }
    public string? announType { get; set; }
    public string? announImg { get; set; }
    public string? publishDate { get; set; }
    public string? expiryDate { get; set; }
    public string? filterType { get; set; }
    public string? isRead { get; set; }
    public string? createDateTime { get; set; }
    public string? link { get; set; }
    public string? grpID { get; set; }
    public string? grpAdminId { get; set; }
    public string? isAdmin { get; set; }
}
