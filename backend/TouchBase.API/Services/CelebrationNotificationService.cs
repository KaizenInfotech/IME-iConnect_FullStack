using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;
using TouchBase.API.Models.Entities;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

// Sends a separate FCM push to every app user for each member whose birthday
// or anniversary falls on the current local-server date. Fires once a day at
// exactly 08:00 local time. No catch-up: if the API is restarted after 08:00,
// today's batch is skipped entirely and the next fire is tomorrow at 08:00.
public class CelebrationNotificationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CelebrationNotificationService> _logger;
    private const int RunHour = 8;

    public CelebrationNotificationService(
        IServiceScopeFactory scopeFactory,
        ILogger<CelebrationNotificationService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CelebrationNotificationService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.Now;
                var todayRun = now.Date.AddHours(RunHour);
                // If we're already past today's 08:00, wait until tomorrow's 08:00.
                // No catch-up — restarting the API after 08:00 will not re-fire today.
                var nextRun = now >= todayRun ? todayRun.AddDays(1) : todayRun;
                var delay = nextRun - DateTime.Now;
                if (delay < TimeSpan.Zero) delay = TimeSpan.FromMinutes(1);
                _logger.LogInformation("CelebrationNotificationService: next run scheduled for {NextRun}.", nextRun);

                try { await Task.Delay(delay, stoppingToken); }
                catch (TaskCanceledException) { return; }

                if (stoppingToken.IsCancellationRequested) return;
                await ProcessTodaysCelebrations(DateTime.Now.Date, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CelebrationNotificationService loop error");
                try { await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); }
                catch (TaskCanceledException) { return; }
            }
        }
    }

    private async Task ProcessTodaysCelebrations(DateTime today, CancellationToken ct)
    {
        var dateSuffix = today.ToString("-MM-dd");
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var notif = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // Dob/Doa are stored as strings — we match by suffix "-MM-dd" so the
        // year doesn't matter. Contains() handles "yyyy-MM-dd" and any trailing
        // time portion that older rows may still carry.
        var birthdayCelebrants = await db.MemberProfiles
            .Where(mp => mp.Dob != null && mp.Dob.Contains(dateSuffix))
            .Select(mp => new { mp.Id, mp.MemberName })
            .ToListAsync(ct);

        foreach (var c in birthdayCelebrants)
        {
            if (ct.IsCancellationRequested) return;
            await SendCelebrationOnce(db, notif, today, c.Id, c.MemberName ?? "", "Birthday", "1", ct);
        }

        var anniversaryCelebrants = await db.MemberProfiles
            .Where(mp => mp.Doa != null && mp.Doa.Contains(dateSuffix))
            .Select(mp => new { mp.Id, mp.MemberName })
            .ToListAsync(ct);

        foreach (var c in anniversaryCelebrants)
        {
            if (ct.IsCancellationRequested) return;
            await SendCelebrationOnce(db, notif, today, c.Id, c.MemberName ?? "", "Anniversary", "2", ct);
        }

        _logger.LogInformation(
            "Celebration push complete for {Date}: {Birthdays} birthday(s), {Anniversaries} anniversary(ies).",
            today.ToString("yyyy-MM-dd"), birthdayCelebrants.Count, anniversaryCelebrants.Count);
    }

    private async Task SendCelebrationOnce(
        AppDbContext db,
        INotificationService notif,
        DateTime today,
        int memberProfileId,
        string memberName,
        string type,
        string baType,
        CancellationToken ct)
    {
        // Skip if this celebrant was already pushed today (e.g. catch-up after restart).
        var alreadySent = await db.CelebrationNotificationLogs
            .AnyAsync(l => l.CelebrationDate == today && l.MemberProfileId == memberProfileId && l.Type == type, ct);
        if (alreadySent) return;

        try
        {
            var title = type == "Birthday" ? "Birthday Wishes" : "Anniversary Wishes";
            var greeting = type == "Birthday"
                ? $"Wish a Happy Birthday to {memberName}"
                : $"Wish a Happy Anniversary to {memberName}";

            await notif.SendAllUsersNotification(
                type,
                title,
                greeting,
                new Dictionary<string, string>
                {
                    ["memberProfileId"] = memberProfileId.ToString(),
                    ["memberName"] = memberName,
                    ["BAType"] = baType,
                });

            db.CelebrationNotificationLogs.Add(new CelebrationNotificationLog
            {
                CelebrationDate = today,
                MemberProfileId = memberProfileId,
                Type = type,
                SentAt = DateTime.UtcNow,
            });
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            // Unique-index collision — a parallel run already logged this one. Safe to ignore.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to send {Type} notification for memberProfileId={Id}",
                type, memberProfileId);
        }
    }
}
