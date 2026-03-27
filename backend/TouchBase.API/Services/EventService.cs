using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;
using TouchBase.API.Models.DTOs.Event;
using TouchBase.API.Models.Entities;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notificationService;
    private const int PageSize = 25;
    public EventService(AppDbContext db, INotificationService notificationService) { _db = db; _notificationService = notificationService; }

    public async Task<EventListResponse> GetEventList(EventListRequest request)
    {
        var grpId = int.TryParse(request.grpId, out var gid) ? gid : 0;
        var page = int.TryParse(request.pageIndex, out var p) ? p : 1;
        var profileId = int.TryParse(request.groupProfileID, out var pid) ? pid : 0;

        var query = _db.Events.Include(e => e.Responses).Where(e => e.GroupId == grpId);
        if (!string.IsNullOrEmpty(request.searchText))
            query = query.Where(e => e.EventTitle!.Contains(request.searchText));
        if (request.Type == "1") query = query.Where(e => e.FilterType != "3"); // upcoming
        else if (request.Type == "3") query = query.Where(e => e.FilterType == "3"); // past

        var total = await query.CountAsync();
        var events = await query.OrderByDescending(e => e.CreatedAt).Skip((page - 1) * PageSize).Take(PageSize)
            .Select(e => new EventListItemDto
            {
                eventID = e.Id.ToString(), eventImg = e.EventImageId, eventTitle = e.EventTitle,
                eventDateTime = e.EventDate, venue = e.EventVenue, venueLat = e.VenueLat, venueLon = e.VenueLon,
                goingCount = e.Responses.Count(r => r.JoiningStatus == "Going").ToString(),
                maybeCount = e.Responses.Count(r => r.JoiningStatus == "Maybe").ToString(),
                notgoingCount = e.Responses.Count(r => r.JoiningStatus == "NotGoing").ToString(),
                myResponse = e.Responses.Where(r => r.MemberProfileId == profileId).Select(r => r.JoiningStatus).FirstOrDefault(),
                filterType = e.FilterType, grpID = e.GroupId.ToString(), grpAdminId = e.CreatedBy.ToString()
            }).ToListAsync();

        return new EventListResponse
        {
            status = "0", message = "success", resultCount = total.ToString(),
            TotalPages = ((int)Math.Ceiling(total / (double)PageSize)).ToString(),
            currentPage = page.ToString(), EventsListResult = events
        };
    }

    public async Task<EventDetailResponse> GetEventDetails(EventDetailRequest request)
    {
        var eventId = int.TryParse(request.eventID, out var eid) ? eid : 0;
        var profileId = int.TryParse(request.groupProfileID, out var pid) ? pid : 0;

        var ev = await _db.Events.Include(e => e.Responses).FirstOrDefaultAsync(e => e.Id == eventId);
        if (ev == null) return new EventDetailResponse { status = "1", message = "Event not found" };

        var detail = new EventsDetailDto
        {
            eventID = ev.Id.ToString(), eventImg = ev.EventImageId, eventTitle = ev.EventTitle,
            eventDesc = ev.EventDesc, Projectname = ev.ProjectName, eventDateTime = ev.EventDate,
            venue = ev.EventVenue, venueLat = ev.VenueLat, venueLon = ev.VenueLon,
            goingCount = ev.Responses.Count(r => r.JoiningStatus == "Going").ToString(),
            maybeCount = ev.Responses.Count(r => r.JoiningStatus == "Maybe").ToString(),
            notgoingCount = ev.Responses.Count(r => r.JoiningStatus == "NotGoing").ToString(),
            totalCount = ev.Responses.Count.ToString(),
            myResponse = ev.Responses.Where(r => r.MemberProfileId == profileId).Select(r => r.JoiningStatus).FirstOrDefault(),
            filterType = ev.FilterType, grpID = ev.GroupId.ToString(), grpAdminId = ev.CreatedBy.ToString(),
            isQuesEnable = ev.QuestionEnable, rsvpEnable = ev.RsvpEnable,
            eventType = ev.EventType, pubDate = ev.PublishDate, expiryDate = ev.ExpiryDate,
            eventDate = ev.EventDate, repeatDateTime = ev.RepeatDateTime,
            questionType = ev.QuestionType, questionText = ev.QuestionText,
            option1 = ev.Option1, option2 = ev.Option2,
            sendSMSNonSmartPh = ev.SendSMSNonSmartPh, sendSMSAll = ev.SendSMSAll,
            displayonbanner = ev.DisplayOnBanner, link = ev.Link
        };

        return new EventDetailResponse
        {
            status = "0", message = "success",
            EventsDetailResult = new List<EventsDetailWrapperDto> { new() { EventsDetail = detail } }
        };
    }

    public async Task<object> AddEvent(AddEventRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var userId = int.TryParse(request.userID, out var uid) ? uid : 0;
        var eventId = int.TryParse(request.eventID, out var eid) ? eid : 0;

        Event ev;
        if (eventId > 0)
        {
            ev = await _db.Events.FindAsync(eventId) ?? new Event();
        }
        else
        {
            ev = new Event { GroupId = grpId, CreatedBy = userId };
            _db.Events.Add(ev);
        }

        ev.EventTitle = request.evntTitle; ev.EventDesc = request.evntDesc; ev.EventType = request.eventType;
        ev.EventImageId = request.eventImageID; ev.EventVenue = request.eventVenue;
        ev.VenueLat = request.venueLat; ev.VenueLon = request.venueLong;
        ev.EventDate = request.evntDate; ev.PublishDate = request.publishDate;
        ev.ExpiryDate = request.expiryDate; ev.NotifyDate = request.notifyDate;
        ev.RepeatDateTime = request.RepeatDateTime; ev.RsvpEnable = request.rsvpEnable;
        ev.QuestionEnable = request.questionEnable; ev.QuestionType = request.questionType;
        ev.QuestionText = request.questionText; ev.Option1 = request.option1; ev.Option2 = request.option2;
        ev.DisplayOnBanner = request.displayonbanner; ev.RegLink = request.reglink;
        ev.SendSMSNonSmartPh = request.sendSMSNonSmartPh; ev.SendSMSAll = request.sendSMSAll;
        ev.MembersIds = request.membersIDs; ev.IsSubGrpAdmin = request.isSubGrpAdmin;
        ev.ProjectName = request.Projectname; ev.Link = request.reglink;
        ev.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // Send push notification to group members for new events
        if (eventId == 0 && grpId > 0)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await _notificationService.SendGroupNotification(
                        grpId, "Event",
                        request.evntTitle ?? "New Event",
                        request.evntDesc ?? "A new event has been created",
                        new Dictionary<string, string>
                        {
                            ["eventId"] = ev.Id.ToString(),
                            ["eventTitle"] = request.evntTitle ?? ""
                        });
                }
                catch { /* don't fail event creation if notification fails */ }
            });
        }

        return new { status = "0", message = "success" };
    }

    public async Task<AnswerEventResponse> AnswerEvent(AnswerEventRequest request)
    {
        var eventId = int.TryParse(request.eventId, out var eid) ? eid : 0;
        var profileId = int.TryParse(request.profileID, out var pid) ? pid : 0;

        var existing = await _db.EventResponses.FirstOrDefaultAsync(er => er.EventId == eventId && er.MemberProfileId == profileId);
        if (existing != null)
        {
            existing.JoiningStatus = request.joiningStatus;
            existing.AnswerByMe = request.answerByme;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _db.EventResponses.Add(new EventResponse
            {
                EventId = eventId, MemberProfileId = profileId,
                JoiningStatus = request.joiningStatus, AnswerByMe = request.answerByme,
                QuestionId = request.questionId
            });
        }
        await _db.SaveChangesAsync();

        var going = await _db.EventResponses.CountAsync(r => r.EventId == eventId && r.JoiningStatus == "Going");
        var maybe = await _db.EventResponses.CountAsync(r => r.EventId == eventId && r.JoiningStatus == "Maybe");
        var notGoing = await _db.EventResponses.CountAsync(r => r.EventId == eventId && r.JoiningStatus == "NotGoing");

        return new AnswerEventResponse
        {
            status = "0", message = "success",
            goingCount = going.ToString(), maybeCount = maybe.ToString(), notgoingCount = notGoing.ToString()
        };
    }

    public async Task<SmsCountResponse> GetSmsCountDetails(SmsCountRequest request)
    {
        return await Task.FromResult(new SmsCountResponse { status = "0", message = "success", smscount = "100" });
    }

    public async Task<object> DeleteEvent(string eventId)
    {
        var id = int.TryParse(eventId, out var eid) ? eid : 0;
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return new { status = "1", message = "Event not found" };
        var responses = await _db.EventResponses.Where(r => r.EventId == id).ToListAsync();
        _db.EventResponses.RemoveRange(responses);
        _db.Events.Remove(ev);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
}
