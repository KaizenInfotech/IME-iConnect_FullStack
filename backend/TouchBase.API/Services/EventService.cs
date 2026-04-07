using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;
using TouchBase.API.Models.DTOs.Event;
using TouchBase.API.Models.Entities;
using TouchBase.API.Controllers;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notificationService;
    private readonly IHttpClientFactory _httpClientFactory;
    private const int PageSize = 25;
    private const string OldApiBase = "https://api.imeiconnect.com/V2/api";
    public EventService(AppDbContext db, INotificationService notificationService, IHttpClientFactory httpClientFactory) { _db = db; _notificationService = notificationService; _httpClientFactory = httpClientFactory; }

    private const string ProdConnStr = "server=101.53.148.126;database=imei_new;user=admin_mysql_db;password=o27AvGxQQGTBEfrlpD7G1;AllowZeroDateTime=True;ConvertZeroDateTime=True;Allow User Variables=true";

    public async Task<EventListResponse> GetEventList(EventListRequest request)
    {
        var grpId = int.TryParse(request.grpId, out var gid) ? gid : 0;

        // Read from event_master table (same as production web app)
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            var sql = @"SELECT e.pk_event_master_id as eventID, e.event_img as eventImg, e.event_title as eventTitle,
                DATE_FORMAT(e.event_date, '%d-%b-%Y %H:%i:%s') as eventDateTime,
                e.event_venue as venue, e.venue_lat as venueLat, e.venue_long as venueLon,
                e.fk_group_master_id as grpID,
                COALESCE(a.MembersCount, 0) as Attendance,
                CASE WHEN COALESCE(g.memberCount, 0) > 0 THEN ROUND(COALESCE(a.MembersCount, 0) * 100.0 / g.memberCount, 2) ELSE 0 END as AttendancePercent
                FROM event_master e
                LEFT JOIN attentance_master a ON a.FK_eventID = e.pk_event_master_id AND (a.isdeleted=0 OR a.isdeleted IS NULL)
                LEFT JOIN group_master g ON e.fk_group_master_id = g.pk_group_master_id
                WHERE e.fk_group_master_id = @grpId AND (e.isdeleted=0 OR e.isdeleted IS NULL)";
            if (!string.IsNullOrEmpty(request.searchText))
                sql += " AND e.event_title LIKE @search";
            sql += " ORDER BY e.event_date DESC";
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@grpId", grpId);
            if (!string.IsNullOrEmpty(request.searchText))
                cmd.Parameters.AddWithValue("@search", $"%{request.searchText}%");
            using var reader = await cmd.ExecuteReaderAsync();
            var events = new List<EventListItemDto>();
            while (await reader.ReadAsync())
            {
                events.Add(new EventListItemDto
                {
                    eventID = reader["eventID"]?.ToString(),
                    eventImg = reader["eventImg"]?.ToString(),
                    eventTitle = reader["eventTitle"]?.ToString(),
                    eventDateTime = reader["eventDateTime"]?.ToString(),
                    venue = reader["venue"]?.ToString(),
                    venueLat = reader["venueLat"]?.ToString(),
                    venueLon = reader["venueLon"]?.ToString(),
                    grpID = reader["grpID"]?.ToString(),
                    Attendance = reader["Attendance"]?.ToString(),
                    AttendancePercent = reader["AttendancePercent"]?.ToString(),
                });
            }
            return new EventListResponse
            {
                status = "0", message = "success", resultCount = events.Count.ToString(),
                TotalPages = "1", currentPage = "1", EventsListResult = events
            };
        }
        catch (Exception ex) { System.Console.WriteLine($"[GetEventList] event_master error: {ex.Message}"); }

        // Fallback to local DB (imei_new)
        var page = int.TryParse(request.pageIndex, out var p) ? p : 1;
        var profileId = int.TryParse(request.groupProfileID, out var pid) ? pid : 0;

        var query = _db.Events.Include(e => e.Responses).Where(e => e.GroupId == grpId);
        if (!string.IsNullOrEmpty(request.searchText))
            query = query.Where(e => e.EventTitle!.Contains(request.searchText));
        if (request.Type == "1") query = query.Where(e => e.FilterType != "3");
        else if (request.Type == "3") query = query.Where(e => e.FilterType == "3");

        var total = await query.CountAsync();
        var events2 = await query.OrderByDescending(e => e.CreatedAt).Skip((page - 1) * PageSize).Take(PageSize)
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
            currentPage = page.ToString(), EventsListResult = events2
        };
    }

    public async Task<EventDetailResponse> GetEventDetails(EventDetailRequest request)
    {
        var eventId = int.TryParse(request.eventID, out var eid) ? eid : 0;

        // Try production DB first
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT e.pk_event_master_id as eventID, e.event_img as eventImg, e.event_title as eventTitle,
                e.event_desc as eventDesc, e.event_date as eventDate, e.event_venue as venue,
                e.venue_lat as venueLat, e.venue_long as venueLon, e.publish_Date as pubDate,
                e.expiry_Date as expiryDate, e.rsvp_enable as rsvpEnable, e.questionEnable as isQuesEnable,
                e.event_type as eventType, e.fk_group_master_id as grpID, e.registration_link as link,
                COALESCE(a.MembersCount, 0) as Attendance,
                CASE WHEN COALESCE(g.memberCount, 0) > 0 THEN ROUND(COALESCE(a.MembersCount, 0) * 100.0 / g.memberCount, 2) ELSE 0 END as AttendancePercent
                FROM event_master e
                LEFT JOIN attentance_master a ON a.FK_eventID = e.pk_event_master_id AND (a.isdeleted=0 OR a.isdeleted IS NULL)
                LEFT JOIN group_master g ON e.fk_group_master_id = g.pk_group_master_id
                WHERE e.pk_event_master_id = @id AND (e.isdeleted=0 OR e.isdeleted IS NULL)";
            cmd.Parameters.AddWithValue("@id", eventId);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var prodDetail = new EventsDetailDto
                {
                    eventID = reader["eventID"]?.ToString(),
                    eventImg = reader["eventImg"]?.ToString(),
                    eventTitle = reader["eventTitle"]?.ToString(),
                    eventDesc = reader["eventDesc"]?.ToString(),
                    eventDateTime = reader["eventDate"]?.ToString(),
                    eventDate = reader["eventDate"]?.ToString(),
                    venue = reader["venue"]?.ToString(),
                    venueLat = reader["venueLat"]?.ToString(),
                    venueLon = reader["venueLon"]?.ToString(),
                    pubDate = reader["pubDate"]?.ToString(),
                    expiryDate = reader["expiryDate"]?.ToString(),
                    rsvpEnable = reader["rsvpEnable"]?.ToString(),
                    isQuesEnable = reader["isQuesEnable"]?.ToString(),
                    eventType = reader["eventType"]?.ToString(),
                    grpID = reader["grpID"]?.ToString(),
                    link = reader["link"]?.ToString(),
                    Attendance = reader["Attendance"]?.ToString(),
                    AttendancePercent = reader["AttendancePercent"]?.ToString(),
                };
                await reader.CloseAsync();

                // Fetch question data
                using var qCmd = conn.CreateCommand();
                qCmd.CommandText = "SELECT question_type, question_text, option1, option2 FROM event_questionnaire_master WHERE fk_event_master_ID=@eid AND (Isdeleted=0 OR Isdeleted IS NULL) ORDER BY pk_event_questionID DESC LIMIT 1";
                qCmd.Parameters.AddWithValue("@eid", eventId);
                using var qReader = await qCmd.ExecuteReaderAsync();
                if (await qReader.ReadAsync())
                {
                    prodDetail.questionType = qReader["question_type"]?.ToString();
                    prodDetail.questionText = qReader["question_text"]?.ToString();
                    prodDetail.option1 = qReader["option1"]?.ToString();
                    prodDetail.option2 = qReader["option2"]?.ToString();
                }
                await qReader.CloseAsync();

                // Fetch repeat dates
                using var rCmd = conn.CreateCommand();
                rCmd.CommandText = "SELECT DATE_FORMAT(Repeat_date, '%Y-%m-%dT%H:%i') as repeatDate FROM event_repeatdate WHERE fk_event_master_id=@eid AND (isdeleted=0 OR isdeleted IS NULL) ORDER BY Repeat_date";
                rCmd.Parameters.AddWithValue("@eid", eventId);
                using var rReader = await rCmd.ExecuteReaderAsync();
                var rDates = new List<string>();
                while (await rReader.ReadAsync()) rDates.Add(rReader["repeatDate"]?.ToString() ?? "");
                if (rDates.Count > 0) prodDetail.repeatDateTime = string.Join(",", rDates);
                return new EventDetailResponse
                {
                    status = "0", message = "success",
                    EventsDetailResult = new List<EventsDetailWrapperDto> { new() { EventsDetail = prodDetail } }
                };
            }
        }
        catch { }

        // Fallback to local DB
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

        // Save to production DB (imei_production) - same as production web app
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            if (eventId > 0)
            {
                cmd.CommandText = @"UPDATE event_master SET event_title=@title, event_desc=@desc, event_date=@evDate,
                    event_venue=@venue, publish_Date=@pubDate, expiry_Date=@expDate,
                    rsvp_enable=@rsvp, questionEnable=@ques, registration_link=@reglink,
                    event_img=@img, modification_date=NOW(), modification_by=@userId
                    WHERE pk_event_master_id=@id";
                cmd.Parameters.AddWithValue("@id", eventId);
            }
            else
            {
                cmd.CommandText = @"INSERT INTO event_master (fk_group_master_id, event_title, event_desc, event_date,
                    event_venue, publish_Date, expiry_Date, rsvp_enable, questionEnable, registration_link,
                    event_img, creation_date, created_by, event_enable, isdeleted)
                    VALUES (@grpId, @title, @desc, @evDate, @venue, @pubDate, @expDate, @rsvp, @ques, @reglink,
                    @img, NOW(), @userId, '1', 0)";
                cmd.Parameters.AddWithValue("@grpId", grpId);
            }
            cmd.Parameters.AddWithValue("@title", request.evntTitle ?? "");
            cmd.Parameters.AddWithValue("@desc", request.evntDesc ?? "");
            cmd.Parameters.AddWithValue("@evDate", string.IsNullOrEmpty(request.evntDate) ? DBNull.Value : (object)DateTime.Parse(request.evntDate));
            cmd.Parameters.AddWithValue("@venue", request.eventVenue ?? "");
            cmd.Parameters.AddWithValue("@pubDate", string.IsNullOrEmpty(request.publishDate) ? DBNull.Value : (object)DateTime.Parse(request.publishDate));
            cmd.Parameters.AddWithValue("@expDate", string.IsNullOrEmpty(request.expiryDate) ? DBNull.Value : (object)DateTime.Parse(request.expiryDate));
            cmd.Parameters.AddWithValue("@rsvp", request.rsvpEnable == "1" ? 1 : 0);
            cmd.Parameters.AddWithValue("@ques", request.questionEnable == "1" ? 1 : 0);
            cmd.Parameters.AddWithValue("@reglink", request.reglink ?? "");
            cmd.Parameters.AddWithValue("@img", request.eventImageID ?? "");
            cmd.Parameters.AddWithValue("@userId", userId);
            await cmd.ExecuteNonQueryAsync();

            // Get the new event ID
            var newEventId = eventId;
            if (eventId == 0)
            {
                using var idCmd = conn.CreateCommand();
                idCmd.CommandText = "SELECT LAST_INSERT_ID()";
                newEventId = Convert.ToInt32(await idCmd.ExecuteScalarAsync());
            }

            // Create/update attentance_master record for attendance data
            var att = int.TryParse(request.attendance, out var a) ? a : 0;
            var attPct = request.attendancePercent ?? "0";
            using var attCmd = conn.CreateCommand();
            // Check if attendance record exists for this event
            attCmd.CommandText = "SELECT attendance_id FROM attentance_master WHERE FK_eventID=@eid AND (isdeleted=0 OR isdeleted IS NULL) LIMIT 1";
            attCmd.Parameters.AddWithValue("@eid", newEventId);
            var existingAttId = await attCmd.ExecuteScalarAsync();
            if (existingAttId != null && existingAttId != DBNull.Value)
            {
                using var updCmd = conn.CreateCommand();
                updCmd.CommandText = "UPDATE attentance_master SET MembersCount=@mc, modification_date=NOW() WHERE attendance_id=@aid";
                updCmd.Parameters.AddWithValue("@mc", att);
                updCmd.Parameters.AddWithValue("@aid", existingAttId);
                await updCmd.ExecuteNonQueryAsync();
            }
            else
            {
                using var insCmd = conn.CreateCommand();
                insCmd.CommandText = "INSERT INTO attentance_master (fk_group_id, name, AttendanceDesc, AttendanceDate, MembersCount, FK_eventID, creation_date, isdeleted) VALUES (@grp, @name, @desc, @date, @mc, @eid, NOW(), 0)";
                insCmd.Parameters.AddWithValue("@grp", grpId);
                insCmd.Parameters.AddWithValue("@name", request.evntTitle ?? "");
                insCmd.Parameters.AddWithValue("@desc", request.evntDesc ?? "");
                insCmd.Parameters.AddWithValue("@date", string.IsNullOrEmpty(request.evntDate) ? DBNull.Value : (object)DateTime.Parse(request.evntDate));
                insCmd.Parameters.AddWithValue("@mc", att);
                insCmd.Parameters.AddWithValue("@eid", newEventId);
                await insCmd.ExecuteNonQueryAsync();
            }

            // Save question data
            if (!string.IsNullOrEmpty(request.questionText) && (request.questionEnable == "1" || request.questionEnable == "2"))
            {
                // Delete existing questions for this event
                using var delQ = conn.CreateCommand();
                delQ.CommandText = "UPDATE event_questionnaire_master SET Isdeleted=1 WHERE fk_event_master_ID=@eid";
                delQ.Parameters.AddWithValue("@eid", newEventId);
                await delQ.ExecuteNonQueryAsync();
                // Insert new
                using var insQ = conn.CreateCommand();
                insQ.CommandText = "INSERT INTO event_questionnaire_master (fk_event_master_ID, question_type, question_text, option1, option2, CreationDate, Isdeleted) VALUES (@eid, @qtype, @qtext, @opt1, @opt2, NOW(), 0)";
                insQ.Parameters.AddWithValue("@eid", newEventId);
                insQ.Parameters.AddWithValue("@qtype", int.TryParse(request.questionType, out var qt) ? qt : 2);
                insQ.Parameters.AddWithValue("@qtext", request.questionText ?? "");
                insQ.Parameters.AddWithValue("@opt1", request.option1 ?? "");
                insQ.Parameters.AddWithValue("@opt2", request.option2 ?? "");
                await insQ.ExecuteNonQueryAsync();
            }

            // Save repeat dates
            if (!string.IsNullOrEmpty(request.RepeatDateTime))
            {
                using var delR = conn.CreateCommand();
                delR.CommandText = "UPDATE event_repeatdate SET isdeleted=1 WHERE fk_event_master_id=@eid";
                delR.Parameters.AddWithValue("@eid", newEventId);
                await delR.ExecuteNonQueryAsync();
                foreach (var dateStr in request.RepeatDateTime.Split(','))
                {
                    if (DateTime.TryParse(dateStr.Trim(), out var dt))
                    {
                        using var insR = conn.CreateCommand();
                        insR.CommandText = "INSERT INTO event_repeatdate (fk_event_master_id, Repeat_date, creation_date, isdeleted) VALUES (@eid, @rd, NOW(), 0)";
                        insR.Parameters.AddWithValue("@eid", newEventId);
                        insR.Parameters.AddWithValue("@rd", dt);
                        await insR.ExecuteNonQueryAsync();
                    }
                }
            }

            return new { status = "0", message = "success", eventID = newEventId.ToString() };
        }
        catch (Exception ex)
        {
            // Fall through to local DB save
            System.Console.WriteLine($"[AddEvent] Production DB error: {ex.Message}");
        }

        // Fallback: save to local DB (imei_new)
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

        // Send push notification for new events
        // National (not a chapter in Clubs table) → all app users; Chapter → chapter members only
        // Flutter expects type="Event" with eventTitle, eventDate, eventDesc, eventImg, reglink, venue
        if (eventId == 0 && grpId > 0)
        {
            var isChapter = await _db.Clubs.AnyAsync(c => c.GroupId == grpId);
            _ = Task.Run(async () =>
            {
                try
                {
                    var extra = new Dictionary<string, string>
                    {
                        ["eventId"] = ev.Id.ToString(),
                        ["eventTitle"] = request.evntTitle ?? "",
                        ["eventDate"] = request.evntDate ?? "",
                        ["eventDesc"] = request.evntDesc ?? "",
                        ["eventImg"] = request.eventImageID ?? "",
                        ["reglink"] = request.reglink ?? "",
                        ["venue"] = request.eventVenue ?? "",
                        ["grpID"] = grpId.ToString()
                    };
                    if (!isChapter)
                        await _notificationService.SendAllUsersNotification(
                            "Event", request.evntTitle ?? "New Event",
                            request.evntDesc ?? "A new event has been created", extra);
                    else
                        await _notificationService.SendGroupNotification(
                            grpId, "Event", request.evntTitle ?? "New Event",
                            request.evntDesc ?? "A new event has been created", extra);
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

        // Delete from production DB (soft delete like production web app)
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE event_master SET isdeleted=1, deletion_date=NOW() WHERE pk_event_master_id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows > 0) return new { status = "0", message = "success" };
        }
        catch { }

        // Fallback: delete from local DB
        var ev = await _db.Events.FindAsync(id);
        if (ev == null) return new { status = "1", message = "Event not found" };
        var responses = await _db.EventResponses.Where(r => r.EventId == id).ToListAsync();
        _db.EventResponses.RemoveRange(responses);
        _db.Events.Remove(ev);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<object> GetEventExtras(string eventId)
    {
        var eid = int.TryParse(eventId, out var id) ? id : 0;
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();

            var agendas = new List<object>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT id, file_name FROM event_agendas WHERE event_id=@eid";
                cmd.Parameters.AddWithValue("@eid", eid);
                using var r = await cmd.ExecuteReaderAsync();
                while (await r.ReadAsync()) agendas.Add(new { id = r["id"], fileName = r["file_name"]?.ToString() });
            }

            var minutes = new List<object>();
            using (var cmd2 = conn.CreateCommand())
            {
                cmd2.CommandText = "SELECT id, file_name FROM event_minutes WHERE event_id=@eid";
                cmd2.Parameters.AddWithValue("@eid", eid);
                using var r2 = await cmd2.ExecuteReaderAsync();
                while (await r2.ReadAsync()) minutes.Add(new { id = r2["id"], fileName = r2["file_name"]?.ToString() });
            }

            var photos = new List<object>();
            using (var cmd3 = conn.CreateCommand())
            {
                cmd3.CommandText = "SELECT id, photo_path, description FROM event_photos WHERE event_id=@eid";
                cmd3.Parameters.AddWithValue("@eid", eid);
                using var r3 = await cmd3.ExecuteReaderAsync();
                while (await r3.ReadAsync()) photos.Add(new { id = r3["id"], photoPath = r3["photo_path"]?.ToString(), description = r3["description"]?.ToString() });
            }

            return new { status = "0", agendas, minutes, photos };
        }
        catch (Exception ex) { return new { status = "1", message = ex.Message }; }
    }

    public async Task<object> SaveEventExtras(SaveEventExtrasRequest request)
    {
        var eid = int.TryParse(request.eventID, out var id) ? id : 0;
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdConnStr);
            await conn.OpenAsync();

            // Save photos
            if (request.photos != null)
            {
                using var del = conn.CreateCommand();
                del.CommandText = "DELETE FROM event_photos WHERE event_id=@eid";
                del.Parameters.AddWithValue("@eid", eid);
                await del.ExecuteNonQueryAsync();
                foreach (var p in request.photos)
                {
                    if (!string.IsNullOrEmpty(p.description) || !string.IsNullOrEmpty(p.photoPath))
                    {
                        using var ins = conn.CreateCommand();
                        ins.CommandText = "INSERT INTO event_photos (event_id, photo_path, description) VALUES (@eid, @path, @desc)";
                        ins.Parameters.AddWithValue("@eid", eid);
                        ins.Parameters.AddWithValue("@path", p.photoPath ?? "");
                        ins.Parameters.AddWithValue("@desc", p.description ?? "");
                        await ins.ExecuteNonQueryAsync();
                    }
                }
            }

            // Save agendas
            if (request.agendaFileNames != null)
            {
                using var del2 = conn.CreateCommand();
                del2.CommandText = "DELETE FROM event_agendas WHERE event_id=@eid";
                del2.Parameters.AddWithValue("@eid", eid);
                await del2.ExecuteNonQueryAsync();
                foreach (var name in request.agendaFileNames)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        using var ins2 = conn.CreateCommand();
                        ins2.CommandText = "INSERT INTO event_agendas (event_id, file_name) VALUES (@eid, @name)";
                        ins2.Parameters.AddWithValue("@eid", eid);
                        ins2.Parameters.AddWithValue("@name", name);
                        await ins2.ExecuteNonQueryAsync();
                    }
                }
            }

            // Save minutes
            if (request.minutesFileNames != null)
            {
                using var del3 = conn.CreateCommand();
                del3.CommandText = "DELETE FROM event_minutes WHERE event_id=@eid";
                del3.Parameters.AddWithValue("@eid", eid);
                await del3.ExecuteNonQueryAsync();
                foreach (var name in request.minutesFileNames)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        using var ins3 = conn.CreateCommand();
                        ins3.CommandText = "INSERT INTO event_minutes (event_id, file_name) VALUES (@eid, @name)";
                        ins3.Parameters.AddWithValue("@eid", eid);
                        ins3.Parameters.AddWithValue("@name", name);
                        await ins3.ExecuteNonQueryAsync();
                    }
                }
            }

            return new { status = "0", message = "success" };
        }
        catch (Exception ex) { return new { status = "1", message = ex.Message }; }
    }
}
