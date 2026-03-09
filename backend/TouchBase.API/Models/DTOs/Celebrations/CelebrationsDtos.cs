namespace TouchBase.API.Models.DTOs.Celebrations;

// ─── Requests ───

public class MonthEventListRequest
{
    public string? profileId { get; set; }
    public string? groupIds { get; set; }
    public string? selectedDate { get; set; }
    public string? updatedOns { get; set; }
    public string? groupCategory { get; set; }
}

public class TypeWiseRequest
{
    public string? GroupID { get; set; }
    public string? groupCategory { get; set; }
    public string? SelectedDate { get; set; }
    public string? Type { get; set; } // B=Birthday, A=Anniversary, E=Event
}

public class DateWiseRequest
{
    public string? GroupID { get; set; }
    public string? SelectedDate { get; set; }
    public string? GroupCategory { get; set; }
}

public class EventMinDetailRequest
{
    public string? eventID { get; set; }
}

public class TodaysBirthdayRequest
{
    public string? groupID { get; set; }
}

// ─── Responses ───

public class MonthEventListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public MonthEventResult? Result { get; set; }
}

public class MonthEventResult
{
    public List<CelebrationEventDto>? Events { get; set; }
}

public class CelebrationEventDto
{
    public string? eventID { get; set; }
    public string? eventDate { get; set; }
    public string? title { get; set; }
    public string? contactNumber { get; set; }
    public string? emailId { get; set; }
    public List<CelebrationEmailDto>? EmailId { get; set; }
    public List<CelebrationMobileDto>? MobileNo { get; set; }
    public string? hideWhatsnum { get; set; }
    public string? hideNum { get; set; }
    public string? hideMail { get; set; }
    public string? description { get; set; }
    public string? eventTitle { get; set; }
    public string? eventImg { get; set; }
    public string? eventDateTime { get; set; }
    public string? venue { get; set; }
    public string? goingCount { get; set; }
    public string? maybeCount { get; set; }
    public string? notgoingCount { get; set; }
    public string? myResponse { get; set; }
    public string? filterType { get; set; }
    public string? grpID { get; set; }
    public string? grpAdminId { get; set; }
    public string? isRead { get; set; }
    public string? venueLat { get; set; }
    public string? venueLon { get; set; }
    public string? type { get; set; }
    public string? memberID { get; set; }
    public string? groupIdNew { get; set; }
}

public class CelebrationEmailDto
{
    public string? MemberName { get; set; }
    public string? EmailId { get; set; }
}

public class CelebrationMobileDto
{
    public string? MemberName { get; set; }
    public string? MobileNo { get; set; }
}

public class TypeWiseResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public TypeWiseResult? Result { get; set; }
}

public class TypeWiseResult
{
    public List<CelebrationEventDto>? Events { get; set; }
}

public class TodaysBirthdayResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<BirthdayItemDto>? Result { get; set; }
}

public class BirthdayItemDto
{
    public string? memberName { get; set; }
    public string? msg { get; set; }
    public string? memberMobile { get; set; }
    public string? memberEmail { get; set; }
    public string? profileId { get; set; }
    public string? relation { get; set; }
    public string? groupID { get; set; }
    public string? contactNumber { get; set; }
    public List<CelebrationMobileDto>? MobileNo { get; set; }
    public List<CelebrationEmailDto>? EmailId { get; set; }
    public string? hideWhatsnum { get; set; }
    public string? hideNum { get; set; }
    public string? hideMail { get; set; }
}
