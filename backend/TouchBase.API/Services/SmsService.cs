using System.Web;

namespace TouchBase.API.Services;

/// <summary>
/// SMS service for sending OTP and general SMS messages.
/// National (India): MDS SMS Gateway (https://mdssend.in/api.php)
/// International: Sinfini Global SMS API
/// Matches old IMEI Connect API GlobalFuns.SendSMSAdd / SendSMSInternational
/// </summary>
public interface ISmsService
{
    Task<bool> SendOtp(string mobileNo, string countryCode, string otp);
    Task<bool> SendSms(string mobileNo, string countryCode, string message);
}

public class SmsService : ISmsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<SmsService> _logger;

    public SmsService(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<SmsService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    public async Task<bool> SendOtp(string mobileNo, string countryCode, string otp)
    {
        if (countryCode == "1") // India
        {
            var message = $"Dear User,Your OTP for login to IME(I) CONNECT app is {otp}.Please do not share this OTP.Regards,IMEICC";
            return await SendNationalSms(mobileNo, message);
        }
        else
        {
            var message = $"{otp} is your one time password (OTP). Please enter your OTP to proceed.\nThank you,\nTeam Rotary India";
            return await SendInternationalSms(mobileNo, countryCode, message);
        }
    }

    public async Task<bool> SendSms(string mobileNo, string countryCode, string message)
    {
        if (countryCode == "1")
            return await SendNationalSms(mobileNo, message);
        else
            return await SendInternationalSms(mobileNo, countryCode, message);
    }

    /// <summary>
    /// Send SMS via MDS SMS Gateway (National/India).
    /// Matches old API: GlobalFuns.SendSMSAdd (line 1282 in GlobalFuns.cs)
    /// GET https://mdssend.in/api.php?username=...&apikey=...&senderid=...&route=OTP&mobile=...&text=...
    /// </summary>
    private async Task<bool> SendNationalSms(string mobileNo, string message)
    {
        var smsConfig = _config.GetSection("Sms:National");
        var url = smsConfig["Url"] ?? "https://mdssend.in/api.php";
        var username = smsConfig["Username"];
        var apiKey = smsConfig["ApiKey"];
        var senderId = smsConfig["SenderId"] ?? "IMEICC";
        var templateId = smsConfig["TemplateId"] ?? "1207167524566099424";
        var entityId = smsConfig["EntityId"] ?? "1201161718551079815";

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("SMS National credentials not configured");
            return false;
        }

        var encodedMessage = HttpUtility.UrlEncode(message);
        var requestUrl = $"{url}?username={username}&apikey={apiKey}&senderid={senderId}&route=OTP&mobile={mobileNo}&text={encodedMessage}&tempid={templateId}&entityid={entityId}";

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(requestUrl);
            var responseText = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("SMS National response: {Response}", responseText);

            if (responseText.Contains("Message Submitted successfully", StringComparison.OrdinalIgnoreCase) ||
                responseText.Contains("OK", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            _logger.LogWarning("SMS send failed: {Response}", responseText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending national SMS to {Mobile}", mobileNo);
        }

        return false;
    }

    /// <summary>
    /// Send SMS via International SMS Gateway (Sinfini).
    /// Matches old API: GlobalFuns.SendSMSInter
    /// GET {domain}?api_key=...&method=sms&sender=...&to=...&message=...
    /// </summary>
    private async Task<bool> SendInternationalSms(string mobileNo, string countryCode, string message)
    {
        var smsConfig = _config.GetSection("Sms:International");
        var domain = smsConfig["Url"] ?? "http://global.sinfini.com/api/v1/";
        var apiKey = smsConfig["ApiKey"];
        var senderId = smsConfig["SenderId"] ?? "KAIZEN";

        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("SMS International credentials not configured");
            return false;
        }

        var fullNumber = countryCode + mobileNo;
        var encodedMessage = HttpUtility.UrlEncode(message);
        var requestUrl = $"{domain}?api_key={apiKey}&method=sms&sender={senderId}&to={fullNumber}&message={encodedMessage}";

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(requestUrl);
            var responseText = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("SMS International response: {Response}", responseText);

            if (responseText.Contains("Ok", StringComparison.OrdinalIgnoreCase))
                return true;

            _logger.LogWarning("International SMS send failed: {Response}", responseText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending international SMS to {Mobile}", fullNumber);
        }

        return false;
    }
}