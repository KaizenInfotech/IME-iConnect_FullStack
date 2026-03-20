using System.Net;
using System.Net.Mail;
using System.Text;

namespace TouchBase.API.Services;

public interface IEmailService
{
    Task<bool> SendOtpEmail(string toEmail, string otp, string memberName, string mobile, string clubName);
    Task<bool> SendEmail(string toEmail, string subject, string htmlBody);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<bool> SendOtpEmail(string toEmail, string otp, string memberName, string mobile, string clubName)
    {
        if (string.IsNullOrWhiteSpace(toEmail)) return false;

        var subject = "IME I Connect - OTP for mobile application";
        var body = BuildOtpEmailBody(memberName, mobile, otp, clubName);
        return await SendEmail(toEmail, subject, body);
    }

    public async Task<bool> SendEmail(string toEmail, string subject, string htmlBody)
    {
        try
        {
            var emailConfig = _config.GetSection("Email");
            var smtpServer = emailConfig["SmtpServer"] ?? "smtp.gmail.com";
            var port = int.Parse(emailConfig["Port"] ?? "587");
            var fromEmail = emailConfig["FromEmail"] ?? "";
            var password = emailConfig["Password"] ?? "";
            var enableSsl = bool.Parse(emailConfig["EnableSsl"] ?? "true");

            if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("Email credentials not configured in appsettings.json");
                return false;
            }

            using var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };

            using var message = new MailMessage
            {
                From = new MailAddress(fromEmail, "IME I Connect"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
            return false;
        }
    }

    /// <summary>
    /// Email template matching old API's LoginController.mailbody()
    /// </summary>
    private string BuildOtpEmailBody(string memberName, string mobile, string otp, string clubName)
    {
        var sb = new StringBuilder();
        sb.Append("<table width='100%' border='0'>");
        sb.Append("<tr><td><strong>Dear ");
        sb.Append(string.IsNullOrEmpty(memberName) ? "Member" : memberName);
        sb.Append("(");
        sb.Append(mobile);
        sb.Append("),</strong></td></tr>");

        sb.Append("<tr><td><br />The security code for accessing IME I Connect on your mobile is: ");
        sb.Append("<strong>");
        sb.Append(otp);
        sb.Append("</strong>");
        sb.Append("<br /><br />Please type this code when prompted while installing the app.</td></tr>");

        sb.Append("<tr><td><br />Chapter / Branch Name : ");
        sb.Append(string.IsNullOrEmpty(clubName) ? "N/A" : clubName);
        sb.Append("</td></tr>");

        sb.Append("<tr><td><br /><p>Thank you<br /><br />Regards,<br /><strong>Team IME I Connect</strong></p></td></tr>");
        sb.Append("</table>");

        return sb.ToString();
    }
}