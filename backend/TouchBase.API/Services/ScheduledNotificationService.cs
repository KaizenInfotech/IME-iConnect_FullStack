using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;

namespace TouchBase.API.Services;

/// <summary>
/// Parses date strings in various formats used by the admin panel and API.
/// </summary>
public static class ScheduledNotificationHelper
{
    private static readonly string[] DateFormats = new[]
    {
        "yyyy-MM-dd'T'HH:mm",           // HTML datetime-local: 2026-04-10T09:00
        "yyyy-MM-dd'T'HH:mm:ss",        // With seconds
        "yyyy-MM-dd HH:mm:ss",          // Standard SQL format
        "yyyy-MM-dd HH:mm",             // Without seconds
        "dd/MM/yyyy HH:mm:ss",          // DD/MM/YYYY format
        "dd/MM/yyyy HH:mm",             // DD/MM/YYYY without seconds
        "dd-MM-yyyy HH:mm:ss",          // DD-MM-YYYY format
        "dd-MM-yyyy HH:mm",             // DD-MM-YYYY without seconds
        "MM/dd/yyyy hh:mm:ss tt",       // US format with AM/PM
        "dd/MM/yyyy hh:mm:ss tt",       // DD/MM with AM/PM
        "dd MMM yyyy hh:mm tt",         // 10 Apr 2026 09:00 AM
    };

    public static DateTime? ParseDate(string? dateStr)
    {
        if (string.IsNullOrWhiteSpace(dateStr)) return null;
        // Clean up non-breaking spaces
        dateStr = dateStr.Replace('\u202f', ' ').Replace('\u00a0', ' ').Trim();
        if (DateTime.TryParseExact(dateStr, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            return result;
        if (DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            return result;
        return null;
    }
}

/// <summary>
/// Background service that checks every minute for announcements and events
/// whose publishDate has arrived but notification hasn't been sent yet.
/// </summary>
public class ScheduledNotificationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledNotificationService> _logger;

    public ScheduledNotificationService(IServiceProvider serviceProvider, ILogger<ScheduledNotificationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ScheduledNotificationService started.");

        // On first run, mark all existing records as sent so we don't spam old notifications
        await MarkExistingRecordsAsSent();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingNotifications();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing scheduled notifications");
            }

            // Check every 60 seconds
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
        }
    }

    private async Task MarkExistingRecordsAsSent()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Mark all existing announcements and events that don't have NotificationSent set
            // This prevents sending notifications for records created before this feature was deployed
            var unmarkedAnnouncements = await db.Announcements.Where(a => !a.NotificationSent && a.CreatedAt < DateTime.UtcNow.AddMinutes(-5)).ToListAsync();
            foreach (var a in unmarkedAnnouncements) a.NotificationSent = true;

            var unmarkedEvents = await db.Events.Where(e => !e.NotificationSent && e.CreatedAt < DateTime.UtcNow.AddMinutes(-5)).ToListAsync();
            foreach (var e in unmarkedEvents) e.NotificationSent = true;

            if (unmarkedAnnouncements.Any() || unmarkedEvents.Any())
            {
                await db.SaveChangesAsync();
                _logger.LogInformation("Marked {AnnCount} announcements and {EvtCount} events as notification-sent (pre-existing records)",
                    unmarkedAnnouncements.Count, unmarkedEvents.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking existing records as sent");
        }
    }

    private async Task ProcessPendingNotifications()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        await ProcessPendingAnnouncements(db, notificationService);
        await ProcessPendingEvents(db, notificationService);
    }

    private async Task ProcessPendingAnnouncements(AppDbContext db, INotificationService notificationService)
    {
        var pendingAnnouncements = await db.Announcements
            .Where(a => !a.NotificationSent)
            .ToListAsync();

        foreach (var ann in pendingAnnouncements)
        {
            var publishTime = ScheduledNotificationHelper.ParseDate(ann.PublishDate);
            if (publishTime == null || publishTime > DateTime.Now) continue;

            // Publish time has arrived — send notification
            try
            {
                var isChapter = await db.Clubs.AnyAsync(c => c.GroupId == ann.GroupId);
                var extra = new Dictionary<string, string>
                {
                    ["announId"] = ann.Id.ToString(),
                    ["ann_title"] = ann.AnnounTitle ?? "",
                    ["Ann_date"] = ann.PublishDate ?? "",
                    ["ann_desc"] = ann.AnnounDesc ?? "",
                    ["ann_lnk"] = ann.RegLink ?? "",
                    ["ann_img"] = ann.AnnounImg ?? "",
                    ["grpID"] = ann.GroupId.ToString()
                };

                if (!isChapter)
                    await notificationService.SendAllUsersNotification(
                        "ann", ann.AnnounTitle ?? "New Announcement",
                        ann.AnnounDesc ?? "A new announcement has been posted", extra);
                else
                    await notificationService.SendGroupNotification(
                        ann.GroupId, "ann", ann.AnnounTitle ?? "New Announcement",
                        ann.AnnounDesc ?? "A new announcement has been posted", extra);

                ann.NotificationSent = true;
                await db.SaveChangesAsync();
                _logger.LogInformation("Sent scheduled notification for announcement {Id}: {Title}", ann.Id, ann.AnnounTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification for announcement {Id}", ann.Id);
            }
        }
    }

    private async Task ProcessPendingEvents(AppDbContext db, INotificationService notificationService)
    {
        var pendingEvents = await db.Events
            .Where(e => !e.NotificationSent)
            .ToListAsync();

        foreach (var ev in pendingEvents)
        {
            var publishTime = ScheduledNotificationHelper.ParseDate(ev.PublishDate);
            if (publishTime == null || publishTime > DateTime.Now) continue;

            // Publish time has arrived — send notification
            try
            {
                var isChapter = await db.Clubs.AnyAsync(c => c.GroupId == ev.GroupId);
                var extra = new Dictionary<string, string>
                {
                    ["eventId"] = ev.Id.ToString(),
                    ["eventTitle"] = ev.EventTitle ?? "",
                    ["eventDate"] = ev.EventDate ?? "",
                    ["eventDesc"] = ev.EventDesc ?? "",
                    ["eventImg"] = ev.EventImageId ?? "",
                    ["reglink"] = ev.RegLink ?? "",
                    ["venue"] = ev.EventVenue ?? "",
                    ["grpID"] = ev.GroupId.ToString()
                };

                if (!isChapter)
                    await notificationService.SendAllUsersNotification(
                        "Event", ev.EventTitle ?? "New Event",
                        ev.EventDesc ?? "A new event has been created", extra);
                else
                    await notificationService.SendGroupNotification(
                        ev.GroupId, "Event", ev.EventTitle ?? "New Event",
                        ev.EventDesc ?? "A new event has been created", extra);

                ev.NotificationSent = true;
                await db.SaveChangesAsync();
                _logger.LogInformation("Sent scheduled notification for event {Id}: {Title}", ev.Id, ev.EventTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification for event {Id}", ev.Id);
            }
        }
    }
}
