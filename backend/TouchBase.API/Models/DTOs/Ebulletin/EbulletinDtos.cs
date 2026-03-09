namespace TouchBase.API.Models.DTOs.Ebulletin;

public class EbulletinListRequest
{
    public string? memberProfileId { get; set; }
    public string? groupId { get; set; }
    public string? YearFilter { get; set; }
}

public class AddEbulletinRequest
{
    public string? ebulletinID { get; set; }
    public string? ebulletinType { get; set; }
    public string? ebulletinTitle { get; set; }
    public string? ebulletinlink { get; set; }
    public string? ebulletinfileid { get; set; }
    public string? memID { get; set; }
    public string? grpID { get; set; }
    public string? inputIDs { get; set; }
    public string? publishDate { get; set; }
    public string? expiryDate { get; set; }
    public string? sendSMSAll { get; set; }
    public string? isSubGrpAdmin { get; set; }
}

public class EbulletinListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? smscount { get; set; }
    public EbulletinResultWrapper? Result { get; set; }
}

public class EbulletinResultWrapper
{
    public List<EbulletinItemDto>? EbulletinListResult { get; set; }
}

public class EbulletinItemDto
{
    public string? ebulletinID { get; set; }
    public string? ebulletinlink { get; set; }
    public string? ebulletinType { get; set; }
    public string? ebulletinDate { get; set; }
    public string? isAdmin { get; set; }
    public string? ebulletinTitle { get; set; }
    public string? filterType { get; set; }
    public string? createDateTime { get; set; }
    public string? publishDateTime { get; set; }
    public string? isRead { get; set; }
}
