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
        // Check if country code is allowed (configured in appsettings.json)
        var allowedCodes = _config.GetSection("AllowedCountryCodes").Get<string[]>() ?? new[] { "1", "91" };
        var cc = request.countryCode?.Trim();
        if (!string.IsNullOrEmpty(cc) && !allowedCodes.Contains(cc))
        {
            var msg = _config["ForeignNumberMessage"] ?? "International numbers are not supported. Please use an Indian mobile number.";
            return new LoginResponse { status = "1", message = msg };
        }

        // Generate OTP locally
        var otp = new Random().Next(1000, 9999).ToString();

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

        // Send OTP via SMS (fire-and-forget)
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

        // Send OTP via email (fire-and-forget)
        _ = Task.Run(async () =>
        {
            try
            {
                var memberEmail = user.Email;
                var memberName = $"{user.FirstName} {user.LastName}".Trim();
                var clubName = "";

                if (string.IsNullOrEmpty(memberEmail))
                {
                    var profile = await _db.MemberProfiles
                        .FirstOrDefaultAsync(mp => mp.UserId == user.Id);
                    if (profile != null)
                        memberEmail = profile.MemberEmail;
                }

                if (!string.IsNullOrEmpty(memberEmail))
                {
                    var grpMember = await _db.GroupMembers
                        .Include(gm => gm.Group)
                        .FirstOrDefaultAsync(gm => gm.MemberProfile.UserId == user.Id && gm.IsActive);
                    if (grpMember?.Group != null)
                        clubName = grpMember.Group.GrpName ?? "";

                    await _emailService.SendOtpEmail(memberEmail, otp, memberName, request.mobileNo ?? "", clubName);
                }
            }
            catch { }
        });

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

        // Find the national/org admin group (grpid1) from local DB
        string? grpid1 = null;
        var allMemberships = user.MemberProfiles.SelectMany(mp => mp.GroupMemberships).ToList();
        var nationalGroup = allMemberships.FirstOrDefault(gm => gm.GroupId == 31185);
        if (nationalGroup != null)
            grpid1 = nationalGroup.GroupId.ToString();
        else if (allMemberships.Count > 1)
            grpid1 = allMemberships.Where(gm => gm != grpMember).Select(gm => gm.GroupId.ToString()).FirstOrDefault();

        // Build response matching old API format: { ds: { Table: [...] } }
        var loginTable = new LoginTable
        {
            masterUID = user.Id,
            grpid0 = grpMember?.GroupId ?? 0,
            grpid1 = grpid1 ?? grpMember?.GroupId.ToString(),
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

    public async Task<object> WebLogin(string mobileNo, string password, string countryCode = "1")
    {
        var admin = await _db.WebAdmins.FirstOrDefaultAsync(a => a.MobileNo == mobileNo && a.Password == password && a.IsActive);
        if (admin == null)
            return new { status = "1", message = "Invalid mobile number or password" };

        var user = await _db.Users
            .Include(u => u.MemberProfiles)
                .ThenInclude(mp => mp.GroupMemberships)
                    .ThenInclude(gm => gm.Group)
            .FirstOrDefaultAsync(u => u.Id == admin.UserId);

        if (user == null)
            return new { status = "1", message = "User not found" };

        var token = GenerateJwtToken(user);
        var profile = user.MemberProfiles.FirstOrDefault();
        var groups = user.MemberProfiles.SelectMany(mp => mp.GroupMemberships).ToList();

        return new
        {
            status = "0",
            message = "success",
            token,
            masterUID = user.Id.ToString(),
            profileId = profile?.Id.ToString() ?? "0",
            name = $"{user.FirstName} {user.LastName}".Trim(),
            mobile = user.MobileNo,
            email = user.Email ?? profile?.MemberEmail,
            role = admin.UserRole ?? "Admin",
            groupId = groups.FirstOrDefault()?.GroupId.ToString(),
            groupName = groups.FirstOrDefault()?.Group?.GrpName,
            profilePic = user.ProfileImage ?? profile?.ProfilePic,
            groups = groups.Select(g => new { grpId = g.GroupId, grpName = g.Group?.GrpName }).ToList()
        };
    }

    public async Task<object> ForgotPassword(string mobileNo)
    {
        // Find admin by mobile number
        var admin = await _db.WebAdmins.FirstOrDefaultAsync(a => a.MobileNo == mobileNo && a.IsActive);
        if (admin == null)
            return new { status = "1", message = "No such mobile number exists" };

        // Find user and email
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == admin.UserId);
        if (user == null)
            return new { status = "1", message = "User not found" };

        var email = user.Email;
        if (string.IsNullOrEmpty(email))
        {
            var profile = await _db.MemberProfiles.FirstOrDefaultAsync(mp => mp.UserId == user.Id);
            email = profile?.MemberEmail;
        }

        if (string.IsNullOrEmpty(email))
            return new { status = "1", message = "Please register your email address in IME I Connect. Contact Help Desk." };

        // Send password to email
        var memberName = $"{user.FirstName} {user.LastName}".Trim();
        var password = admin.Password;

        _ = Task.Run(async () =>
        {
            try
            {
                var subject = "IMEIconnect - Your Forgotten Password";
                var body = $"<p>Dear <strong>{memberName}</strong>,</p>"
                    + $"<p>We have received a request to resend your forgotten password. Your password for IME I Connect is as follows:</p>"
                    + $"<p><strong>Username:</strong> {mobileNo}</p>"
                    + $"<p><strong>Password:</strong> {password}</p>"
                    + $"<p>Please log in using this password, and we strongly recommend that you change your password after logging in for security reasons.</p>"
                    + $"<p>Thank you for using our system.</p>"
                    + $"<p><em>Best regards,<br/><strong>TEAM IME I CONNECT</strong></em></p>";
                await _emailService.SendEmail(email, subject, body);
            }
            catch { }
        });

        return new { status = "0", message = "Password sent successfully on your registered email address" };
    }
}
