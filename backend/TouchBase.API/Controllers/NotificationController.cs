using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Services;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IFcmService _fcmService;
    public NotificationController(IFcmService fcmService) => _fcmService = fcmService;

    [HttpPost("SendPushNotification")]
    public async Task<IActionResult> SendPushNotification([FromBody] SendPushRequest request)
    {
        try
        {
            var data = new Dictionary<string, string>
            {
                ["title"] = request.title ?? "",
                ["body"] = request.body ?? "",
                ["click_action"] = "OpenNotification"
            };

            int successCount = 0;

            if (request.sendToAndroid)
            {
                try
                {
                    await _fcmService.SendToDevice("/topics/mobile1", "android", data, request.title, request.body);
                    successCount++;
                }
                catch { }
            }

            if (request.sendToiOS)
            {
                try
                {
                    await _fcmService.SendToDevice("/topics/mobileIOS", "iOS", data, request.title, request.body);
                    successCount++;
                }
                catch { }
            }

            return Ok(new { status = "0", message = $"Notification sent to {successCount} platform(s)" });
        }
        catch (Exception ex)
        {
            return Ok(new { status = "1", message = ex.Message });
        }
    }
}

public class SendPushRequest
{
    public string? title { get; set; }
    public string? body { get; set; }
    public bool sendToAndroid { get; set; }
    public bool sendToiOS { get; set; }
}