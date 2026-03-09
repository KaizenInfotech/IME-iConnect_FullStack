namespace TouchBase.API.Models.DTOs.Event;

// ─── Requests ───

public class EventListRequest
{
    public string? groupProfileID { get; set; }
    public string? grpId { get; set; }
    public string? Type { get; set; }
    public string? Admin { get; set; }
    public string? searchText { get; set; }
    public string? pageIndex { get; set; }
    public string? moduleId { get; set; }
}

public class EventDetailRequest
{
    public string? groupProfileID { get; set; }
    public string? eventID { get; set; }
    public string? grpId { get; set; }
}

public class AddEventRequest
{
    public string? eventID { get; set; }
    public string? questionEnable { get; set; }
    public string? eventType { get; set; }
    public string? membersIDs { get; set; }
    public string? eventImageID { get; set; }
    public string? evntTitle { get; set; }
    public string? evntDesc { get; set; }
    public string? eventVenue { get; set; }
    public string? venueLat { get; set; }
    public string? venueLong { get; set; }
    public string? evntDate { get; set; }
    public string? publishDate { get; set; }
    public string? expiryDate { get; set; }
    public string? notifyDate { get; set; }
    public string? userID { get; set; }
    public string? grpID { get; set; }
    public string? RepeatDateTime { get; set; }
    public string? questionType { get; set; }
    public string? questionText { get; set; }
    public string? option1 { get; set; }
    public string? option2 { get; set; }
    public string? sendSMSNonSmartPh { get; set; }
    public string? sendSMSAll { get; set; }
    public string? rsvpEnable { get; set; }
    public string? displayonbanner { get; set; }
    public string? reglink { get; set; }
    public string? isSubGrpAdmin { get; set; }
    public string? Projectname { get; set; }
}

public class AnswerEventRequest
{
    public string? grpId { get; set; }
    public string? profileID { get; set; }
    public string? eventId { get; set; }
    public string? joiningStatus { get; set; }
    public string? questionId { get; set; }
    public string? answerByme { get; set; }
}

public class SmsCountRequest
{
    public string? grpId { get; set; }
    public string? profileID { get; set; }
}

// ─── Responses ───

public class EventListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? SMSCount { get; set; }
    public string? resultCount { get; set; }
    public string? TotalPages { get; set; }
    public string? currentPage { get; set; }
    public List<EventListItemDto>? EventsListResult { get; set; }
    public string? link { get; set; }
}

public class EventListItemDto
{
    public string? eventID { get; set; }
    public string? eventImg { get; set; }
    public string? eventTitle { get; set; }
    public string? eventDateTime { get; set; }
    public string? goingCount { get; set; }
    public string? maybeCount { get; set; }
    public string? notgoingCount { get; set; }
    public string? venue { get; set; }
    public string? myResponse { get; set; }
    public string? filterType { get; set; }
    public string? grpID { get; set; }
    public string? isRead { get; set; }
    public string? venueLat { get; set; }
    public string? venueLon { get; set; }
    public string? grpAdminId { get; set; }
}

public class EventDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<EventsDetailWrapperDto>? EventsDetailResult { get; set; }
}

public class EventsDetailWrapperDto
{
    public EventsDetailDto? EventsDetail { get; set; }
}

public class EventsDetailDto
{
    public string? eventID { get; set; }
    public string? eventImg { get; set; }
    public string? eventTitle { get; set; }
    public string? eventDesc { get; set; }
    public string? Projectname { get; set; }
    public string? eventDateTime { get; set; }
    public string? goingCount { get; set; }
    public string? maybeCount { get; set; }
    public string? notgoingCount { get; set; }
    public string? venue { get; set; }
    public string? myResponse { get; set; }
    public string? filterType { get; set; }
    public string? grpID { get; set; }
    public string? grpAdminId { get; set; }
    public string? totalCount { get; set; }
    public string? venueLat { get; set; }
    public string? venueLon { get; set; }
    public string? isQuesEnable { get; set; }
    public List<RepeatEventDto>? repeatEventResult { get; set; }
    public List<object>? questionArray { get; set; }
    public string? eventType { get; set; }
    public string? inputIds { get; set; }
    public string? pubDate { get; set; }
    public string? expiryDate { get; set; }
    public string? eventDate { get; set; }
    public string? repeatDateTime { get; set; }
    public string? questionType { get; set; }
    public string? questionText { get; set; }
    public string? option1 { get; set; }
    public string? option2 { get; set; }
    public string? questionId { get; set; }
    public string? sendSMSNonSmartPh { get; set; }
    public string? sendSMSAll { get; set; }
    public string? rsvpEnable { get; set; }
    public string? displayonbanner { get; set; }
    public string? isAdmin { get; set; }
    public string? memberprofileid { get; set; }
    public string? link { get; set; }
}

public class RepeatEventDto
{
    public string? eventDate { get; set; }
    public string? eventTime { get; set; }
}

public class AnswerEventResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? goingCount { get; set; }
    public string? maybeCount { get; set; }
    public string? notgoingCount { get; set; }
}

public class SmsCountResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? smscount { get; set; }
}
