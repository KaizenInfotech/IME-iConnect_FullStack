namespace TouchBase.API.Models.DTOs.Notification;

public class NotificationSettingResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? isMobileSelf { get; set; }
    public string? isMobileOther { get; set; }
    public string? isEmailSelf { get; set; }
    public string? isEmailOther { get; set; }
    public NotificationSettingResultWrapper? TBGroupSettingResult { get; set; }
}

public class NotificationSettingResultWrapper
{
    public NotificationSettingResult? GRpSettingResult { get; set; }
}

public class NotificationSettingResult
{
    public List<NotificationSettingItemDto>? GRpSettingDetails { get; set; }
}

public class NotificationSettingItemDto
{
    public string? moduleId { get; set; }
    public string? modName { get; set; }
    public string? modVal { get; set; }
}
