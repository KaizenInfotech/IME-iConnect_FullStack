using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;
using TouchBase.API.Models.Entities;

namespace TouchBase.API.Services;

public interface INotificationService
{
    /// Send push notification to all members of a group when content is created.
    Task SendGroupNotification(int groupId, string type, string title, string message, Dictionary<string, string>? extraData = null);

    /// Send push notification to a specific user.
    Task SendUserNotification(int userId, string type, string title, string message, Dictionary<string, string>? extraData = null);

    /// Store a notification record in the database.
    Task StoreNotification(int userId, string type, string title, string details);

    /// Get unread notification count for a user.
    Task<int> GetUnreadCount(int userId);
}

public class NotificationService : INotificationService
{
    private readonly AppDbContext _db;
    private readonly IFcmService _fcmService;

    public NotificationService(AppDbContext db, IFcmService fcmService)
    {
        _db = db;
        _fcmService = fcmService;
    }

    public async Task SendGroupNotification(int groupId, string type, string title, string message, Dictionary<string, string>? extraData = null)
    {
        // Get all active members of the group with their device tokens
        var memberUserIds = await _db.GroupMembers
            .Where(gm => gm.GroupId == groupId && gm.IsActive)
            .Select(gm => gm.MemberProfile.UserId)
            .Distinct()
            .ToListAsync();

        if (!memberUserIds.Any()) return;

        // Get device tokens for all members
        var devices = await _db.DeviceTokens
            .Where(dt => memberUserIds.Contains(dt.UserId) && dt.Token != null)
            .Select(dt => new { dt.Token, dt.Platform, dt.UserId })
            .ToListAsync();

        // Build FCM data payload matching old notification system format
        var data = new Dictionary<string, string>
        {
            ["type"] = type,
            ["entityName"] = title,
            ["Message"] = message,
            ["groupId"] = groupId.ToString()
        };

        if (extraData != null)
        {
            foreach (var kv in extraData)
                data[kv.Key] = kv.Value;
        }

        // Send FCM to all devices
        var deviceList = devices
            .Where(d => !string.IsNullOrEmpty(d.Token))
            .Select(d => (d.Token!, d.Platform ?? "android"));

        await _fcmService.SendToMultipleDevices(deviceList, data, title, message);

        // Store notification records for each user
        foreach (var userId in memberUserIds)
        {
            await StoreNotification(userId, type, title, message);
        }
    }

    public async Task SendUserNotification(int userId, string type, string title, string message, Dictionary<string, string>? extraData = null)
    {
        var devices = await _db.DeviceTokens
            .Where(dt => dt.UserId == userId && dt.Token != null)
            .Select(dt => new { dt.Token, dt.Platform })
            .ToListAsync();

        var data = new Dictionary<string, string>
        {
            ["type"] = type,
            ["entityName"] = title,
            ["Message"] = message
        };

        if (extraData != null)
        {
            foreach (var kv in extraData)
                data[kv.Key] = kv.Value;
        }

        foreach (var device in devices.Where(d => !string.IsNullOrEmpty(d.Token)))
        {
            await _fcmService.SendToDevice(device.Token!, device.Platform ?? "android", data, title, message);
        }

        await StoreNotification(userId, type, title, message);
    }

    public async Task StoreNotification(int userId, string type, string title, string details)
    {
        var now = DateTime.UtcNow;
        _db.Notifications.Add(new Notification
        {
            UserId = userId,
            Title = title,
            Details = details,
            Type = type,
            NotifyDate = now.ToString("dd MMM yyyy hh:mm tt"),
            ExpiryDate = now.AddDays(3).ToString("dd/MM/yyyy"),
            SortDate = now.ToString("o"),
            ReadStatus = "UnRead",
            CreatedAt = now
        });
        await _db.SaveChangesAsync();
    }

    public async Task<int> GetUnreadCount(int userId)
    {
        return await _db.Notifications.CountAsync(n => n.UserId == userId && n.ReadStatus != "Read");
    }
}