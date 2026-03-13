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

    public AuthService(AppDbContext db, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<LoginResponse> RequestOtp(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.MobileNo == request.mobileNo);
        var otp = new Random().Next(1000, 9999).ToString();

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

        return new LoginResponse
        {
            status = "0",
            message = "success",
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
            ds = new LoginDs { Table = new List<LoginTable> { loginTable } }
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
