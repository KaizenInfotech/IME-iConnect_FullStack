using System.Text;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;

namespace TouchBase.API.Services;

/// <summary>
/// Firebase Cloud Messaging service for sending push notifications.
/// Uses FCM v1 HTTP API with service account OAuth2 authentication.
/// </summary>
public interface IFcmService
{
    Task<bool> SendToDevice(string deviceToken, string platform, Dictionary<string, string> data, string? title = null, string? body = null);
    Task<int> SendToMultipleDevices(IEnumerable<(string token, string platform)> devices, Dictionary<string, string> data, string? title = null, string? body = null);
}

public class FcmService : IFcmService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<FcmService> _logger;
    private GoogleCredential? _credential;
    private string? _projectId;

    public FcmService(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<FcmService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            if (_credential == null)
            {
                var serviceAccountPath = _config["Firebase:ServiceAccountPath"];
                if (string.IsNullOrEmpty(serviceAccountPath))
                {
                    _logger.LogWarning("Firebase:ServiceAccountPath not configured");
                    return null;
                }

                // Resolve relative path from app base directory
                if (!Path.IsPathRooted(serviceAccountPath))
                    serviceAccountPath = Path.Combine(AppContext.BaseDirectory, serviceAccountPath);

                if (!File.Exists(serviceAccountPath))
                {
                    _logger.LogWarning("Firebase service account file not found: {Path}", serviceAccountPath);
                    return null;
                }

                using var stream = new FileStream(serviceAccountPath, FileMode.Open, FileAccess.Read);
#pragma warning disable CS0618 // GoogleCredential.FromStream is deprecated but replacement API is unstable
                _credential = GoogleCredential
                    .FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
#pragma warning restore CS0618

                // Extract project_id from the service account JSON
                var jsonContent = await File.ReadAllTextAsync(serviceAccountPath);
                using var doc = JsonDocument.Parse(jsonContent);
                if (doc.RootElement.TryGetProperty("project_id", out var pid))
                    _projectId = pid.GetString();
            }

            var token = await _credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Firebase access token");
            return null;
        }
    }

    public async Task<bool> SendToDevice(string deviceToken, string platform, Dictionary<string, string> data, string? title = null, string? body = null)
    {
        if (string.IsNullOrEmpty(deviceToken))
            return false;

        var accessToken = await GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(_projectId))
            return false;

        var fcmUrl = $"https://fcm.googleapis.com/v1/projects/{_projectId}/messages:send";

        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, fcmUrl);
        request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {accessToken}");

        // Build FCM v1 message payload
        object message;
        if (platform?.ToLower() == "ios")
        {
            // iOS: notification + data payload (shows in system tray)
            message = new
            {
                message = new
                {
                    token = deviceToken,
                    notification = new
                    {
                        title = title ?? data.GetValueOrDefault("entityName", "TouchBase"),
                        body = body ?? data.GetValueOrDefault("Message", "")
                    },
                    apns = new
                    {
                        payload = new
                        {
                            aps = new
                            {
                                badge = 1,
                                sound = "default"
                            }
                        }
                    },
                    data
                }
            };
        }
        else
        {
            // Android: data-only payload (app constructs notification locally)
            message = new
            {
                message = new
                {
                    token = deviceToken,
                    android = new
                    {
                        priority = "high",
                        ttl = "108s",
                        collapse_key = "score_update"
                    },
                    data
                }
            };
        }

        var json = JsonSerializer.Serialize(message);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("FCM v1 sent successfully to {Platform} device", platform);
                return true;
            }

            _logger.LogWarning("FCM v1 send failed: {Status} - {Response}", response.StatusCode, responseJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "FCM v1 send error");
        }

        return false;
    }

    public async Task<int> SendToMultipleDevices(IEnumerable<(string token, string platform)> devices, Dictionary<string, string> data, string? title = null, string? body = null)
    {
        var successCount = 0;
        foreach (var (token, platform) in devices)
        {
            if (await SendToDevice(token, platform, data, title, body))
                successCount++;
        }
        return successCount;
    }
}