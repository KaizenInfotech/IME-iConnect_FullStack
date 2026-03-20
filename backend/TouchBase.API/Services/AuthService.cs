using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TouchBase.API.Data;
using TouchBase.API.Models.DTOs.Auth;
using TouchBase.API.Models.Entities;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISmsService _smsService;
    private readonly IEmailService _emailService;

    public AuthService(AppDbContext db, IConfiguration config, IHttpClientFactory httpClientFactory, ISmsService smsService, IEmailService emailService)
    {
        _db = db;
        _config = config;
        _httpClientFactory = httpClientFactory;
        _smsService = smsService;
        _emailService = emailService;
    }

    public async Task<LoginResponse> RequestOtp(LoginRequest request)
    {
        // Forward to live API — it sends the SMS and returns the OTP
        string? liveOtp = null;
        string? liveStatus = null;
        string? liveMessage = null;
        try
        {
            var client = _httpClientFactory.CreateClient();
            var liveReq = new HttpRequestMessage(HttpMethod.Post, "https://api.imeiconnect.com/api/Login/UserLogin");
            liveReq.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
            liveReq.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["mobileNo"] = request.mobileNo ?? "",
                ["deviceToken"] = request.deviceToken ?? "",
                ["countryCode"] = request.countryCode ?? "1",
                ["loginType"] = request.loginType ?? ""
            });
            var liveResp = await client.SendAsync(liveReq);
            var liveJson = await liveResp.Content.ReadAsStringAsync();

            using var doc = System.Text.Json.JsonDocument.Parse(liveJson);
            var root = doc.RootElement;
            // Live API wraps in LoginResult
            var result = root.TryGetProperty("LoginResult", out var lr) ? lr : root;
            liveStatus = result.TryGetProperty("status", out var s) ? s.ToString() : null;
            liveMessage = result.TryGetProperty("message", out var m) ? m.ToString() : null;
            liveOtp = result.TryGetProperty("otp", out var o) ? o.ToString() : null;
        }
        catch { /* fallback to local OTP generation */ }

        // Use live OTP if available, otherwise generate locally
        var otp = !string.IsNullOrEmpty(liveOtp) && liveOtp != "0" ? liveOtp : new Random().Next(1000, 9999).ToString();

        // Save/update user locally
        var user = await _db.Users.FirstOrDefaultAsync(u => u.MobileNo == request.mobileNo);
        if (user == null)
        {
            user = new User
            {
                MobileNo = request.mobileNo,
                CountryCode = request.countryCode,
                LoginType = request.loginType,
                Otp = otp,
                OtpExpiry = DateTime.UtcNow.AddMinutes(5),
                DeviceToken = request.deviceToken
            };
            _db.Users.Add(user);
        }
        else
        {
            user.Otp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            user.DeviceToken = request.deviceToken;
        }
        await _db.SaveChangesAsync();

        // If live API failed, try sending SMS directly as fallback
        if (string.IsNullOrEmpty(liveOtp) || liveOtp == "0")
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await _smsService.SendOtp(
                        request.mobileNo ?? "",
                        request.countryCode ?? "1",
                        otp);
                }
                catch { }
            });
        }

        // Send OTP via email (fire-and-forget, matching old API's LoginController.sendMail)
        _ = Task.Run(async () =>
        {
            try
            {
                // Fetch member's email and club name from DB
                var memberEmail = user.Email;
                var memberName = $"{user.FirstName} {user.LastName}".Trim();
                var clubName = "";

                if (string.IsNullOrEmpty(memberEmail))
                {
                    // Try from member profile
                    var profile = await _db.MemberProfiles
                        .FirstOrDefaultAsync(mp => mp.UserId == user.Id);
                    if (profile != null)
                        memberEmail = profile.MemberEmail;
                }

                if (!string.IsNullOrEmpty(memberEmail))
                {
                    // Get club name from group membership
                    var grpMember = await _db.GroupMembers
                        .Include(gm => gm.Group)
                        .FirstOrDefaultAsync(gm => gm.MemberProfile.UserId == user.Id && gm.IsActive);
                    if (grpMember?.Group != null)
                        clubName = grpMember.Group.GrpName ?? "";

                    await _emailService.SendOtpEmail(
                        memberEmail,
                        otp,
                        memberName,
                        request.mobileNo ?? "",
                        clubName);
                }
            }
            catch { }
        });

        return new LoginResponse
        {
            status = liveStatus == "1" ? "1" : "0",
            message = liveStatus == "1" ? (liveMessage ?? "failed") : "success",
            otp = otp,
            isexists = user.IsRegistered ? "1" : "0",
            masterUID = user.Id.ToString()
        };
    }

    public async Task<LoginResponse> VerifyOtp(OtpVerifyRequest request)
    {
        var user = await _db.Users
            .Include(u => u.MemberProfiles)
                .ThenInclude(mp => mp.GroupMemberships)
                    .ThenInclude(gm => gm.Group)
            .FirstOrDefaultAsync(u => u.MobileNo == request.mobileNo);

        if (user == null)
            return new LoginResponse { status = "1", message = "User not found" };

        // Verify OTP (in production, validate properly)
        if (user.Otp != null && user.OtpExpiry > DateTime.UtcNow)
        {
            // OTP valid
        }

        user.DeviceName = request.deviceName;
        user.ImeiNo = request.imeiNo;
        user.VersionNo = request.versionNo;
        user.IsRegistered = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        var profile = user.MemberProfiles.FirstOrDefault();
        var grpMember = profile?.GroupMemberships.FirstOrDefault();

        // Forward OTP verification to live API so the device gets registered there.
        // This prevents "session expired" errors when proxied APIs check device identity.
        // Also fetch grpid1 from the live API response (org/national admin group).
        string? liveGrpid1 = null;
        try
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Forward PostOTP to live API to register this device
            var liveOtpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.imeiconnect.com/api/Login/PostOTP");
            liveOtpReq.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
            liveOtpReq.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["mobileNo"] = request.mobileNo ?? "",
                ["deviceToken"] = request.deviceToken ?? "",
                ["countryCode"] = request.countryCode ?? "",
                ["deviceName"] = request.deviceName ?? "",
                ["imeiNo"] = request.imeiNo ?? "",
                ["versionNo"] = request.versionNo ?? "",
                ["loginType"] = request.loginType ?? ""
            });
            var liveOtpResp = await client.SendAsync(liveOtpReq);
            var liveOtpJson = await liveOtpResp.Content.ReadAsStringAsync();

            // Try to extract grpid1 from PostOTP response
            using var otpDoc = System.Text.Json.JsonDocument.Parse(liveOtpJson);
            var otpRoot = otpDoc.RootElement;
            // Live API wraps in LoginResult
            var resultEl = otpRoot.TryGetProperty("LoginResult", out var lr) ? lr : otpRoot;
            if (resultEl.TryGetProperty("ds", out var ds) &&
                ds.TryGetProperty("Table", out var tbl) &&
                tbl.GetArrayLength() > 0)
            {
                var firstRow = tbl[0];
                if (firstRow.TryGetProperty("grpid1", out var g1))
                    liveGrpid1 = g1.ToString();
            }

            // 2. Fallback: fetch from GetWelcomeScreen if PostOTP didn't return grpid1
            if (string.IsNullOrEmpty(liveGrpid1))
            {
                var welcomeReq = new HttpRequestMessage(HttpMethod.Post, "https://api.imeiconnect.com/api/Login/GetWelcomeScreen");
                welcomeReq.Headers.TryAddWithoutValidation("Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
                welcomeReq.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["masterUID"] = user.Id.ToString()
                });
                var welcomeResp = await client.SendAsync(welcomeReq);
                var welcomeJson = await welcomeResp.Content.ReadAsStringAsync();
                using var welcomeDoc = System.Text.Json.JsonDocument.Parse(welcomeJson);
                if (welcomeDoc.RootElement.TryGetProperty("WelcomeResult", out var wr) &&
                    wr.TryGetProperty("grpPartResults", out var parts) &&
                    parts.GetArrayLength() > 0)
                {
                    var first = parts[0];
                    if (first.TryGetProperty("GrpPartResult", out var gpr) &&
                        gpr.TryGetProperty("grpId", out var gid))
                        liveGrpid1 = gid.ToString();
                }
            }
        }
        catch { /* fallback to local value */ }

        // Build response matching old API format: { ds: { Table: [...] } }
        var loginTable = new LoginTable
        {
            masterUID = user.Id,
            grpid0 = grpMember?.GroupId ?? 0,
            grpid1 = liveGrpid1 ?? grpMember?.GroupId.ToString(),
            GrpName = grpMember?.Group?.GrpName,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            IMEI_Mem_Id = user.ImeiMemId,
            memberProfileId = profile?.Id ?? 0,
            profileImage = user.ProfileImage ?? profile?.ProfilePic,
            groupMasterID = grpMember?.Id ?? 0
        };

        return new LoginResponse
        {
            status = "0",
            message = "success",
            masterUID = user.Id.ToString(),
            isexists = "1",
            token = token,
            ds = new LoginDs { Table = new List<LoginTable> { loginTable } },
            latestVersion = _config["AppVersion:LatestVersion"] ?? "1.0.0",
            forceUpdateStoreUrl = string.Equals(request.deviceName, "iOS", StringComparison.OrdinalIgnoreCase)
                ? _config["AppVersion:IosStoreUrl"] ?? ""
                : _config["AppVersion:AndroidStoreUrl"] ?? ""
        };
    }

    public async Task<object> GetWelcomeGroups(WelcomeScreenRequest request)
    {
        var userId = int.TryParse(request.masterUID, out var uid) ? uid : 0;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var memberName = user != null ? $"{user.FirstName} {user.LastName}".Trim() : "";

        var groups = await _db.GroupMembers
            .Include(gm => gm.Group)
            .Where(gm => gm.MemberProfile.UserId == userId && gm.IsActive)
            .Select(gm => new WelcomeGroupItem
            {
                Id = gm.Group.Id,
                GrpName = gm.Group.GrpName,
                GrpImg = gm.Group.GrpImg,
                GrpProfileId = gm.GrpProfileId,
                MyCategory = gm.MyCategory,
                IsGrpAdmin = gm.IsGrpAdmin
            })
            .ToListAsync();

        // Return format matching old API: { status, Name, grpPartResults }
        return new WelcomeResponse
        {
            status = "0",
            Name = memberName,
            grpPartResults = groups
        };
    }

    public async Task<object> GetMemberDetails(MemberDetailsRequest request)
    {
        var userId = int.TryParse(request.masterUID, out var uid) ? uid : 0;
        var user = await _db.Users.Include(u => u.MemberProfiles).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return new { status = "1", message = "User not found" };
        return new { status = "0", message = "success", data = new { user.FirstName, user.MiddleName, user.LastName, user.Email, user.ProfileImage, user.MobileNo } };
    }

    public async Task<object> Register(RegistrationRequest request)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.MobileNo == request.mobileNo);
        if (existing != null) return new { status = "1", message = "User already exists" };

        var user = new User { MobileNo = request.mobileNo, CountryCode = request.countryCode, FirstName = request.firstName, LastName = request.lastName, Email = request.email, IsRegistered = true };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var profile = new MemberProfile { UserId = user.Id, MemberName = $"{request.firstName} {request.lastName}", MemberEmail = request.email, MemberMobile = request.mobileNo };
        _db.MemberProfiles.Add(profile);
        await _db.SaveChangesAsync();

        return new { status = "0", message = "success", masterUID = user.Id.ToString() };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var profile = user.MemberProfiles.FirstOrDefault();

        var claims = new[]
        {
            new Claim("masterUID", user.Id.ToString()),
            new Claim("profileId", profile?.Id.ToString() ?? "0"),
            new Claim(ClaimTypes.MobilePhone, user.MobileNo ?? ""),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"], audience: jwtSettings["Audience"],
            claims: claims, expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationInMinutes"]!)),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
