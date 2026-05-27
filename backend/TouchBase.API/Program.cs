using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TouchBase.API.Data;
using TouchBase.API.Services;
using TouchBase.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// --- Database ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
if (!connectionString.Contains("AllowZeroDateTime", StringComparison.OrdinalIgnoreCase))
    connectionString += ";AllowZeroDateTime=True;ConvertZeroDateTime=True";
if (!connectionString.Contains("ConnectionReset", StringComparison.OrdinalIgnoreCase))
    connectionString += ";ConnectionReset=true";
if (!connectionString.Contains("UseAffectedRows", StringComparison.OrdinalIgnoreCase))
    connectionString += ";UseAffectedRows=false;NoBackslashEscapes=false";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

// --- JWT Authentication ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

// --- CORS ---
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        if (allowedOrigins.Length > 0)
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        else
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// --- Services Registration ---
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IEbulletinService, EbulletinService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ICelebrationsService, CelebrationsService>();
builder.Services.AddScoped<IServiceDirectoryService, ServiceDirectoryService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IFindClubService, FindClubService>();
builder.Services.AddScoped<IFindRotarianService, FindRotarianService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IWebLinkService, WebLinkService>();
builder.Services.AddScoped<IPastPresidentService, PastPresidentService>();
builder.Services.AddScoped<IMerService, MerService>();
builder.Services.AddScoped<IFcmService, FcmService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Background service: sends push notifications at scheduled publishDate
builder.Services.AddHostedService<TouchBase.API.Services.ScheduledNotificationService>();

// Background service: daily 08:00 push for birthdays and anniversaries
builder.Services.AddHostedService<TouchBase.API.Services.CelebrationNotificationService>();

// --- Controllers (JSON only, no views) ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve exact casing
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TouchBase API",
        Version = "v1",
        Description = "TouchBase Rotary Organization Management API"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// --- Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // For serving uploaded files from wwwroot

app.UseCors("AllowAll");

// Short-circuit OPTIONS preflights so they never reach auth or controllers.
// Prevents 500 errors caused by IIS WebDAV or missing-token exceptions.
app.Use(async (ctx, next) =>
{
    if (HttpMethods.IsOptions(ctx.Request.Method))
    {
        var origin = ctx.Request.Headers["Origin"].ToString();
        if (!string.IsNullOrEmpty(origin))
            ctx.Response.Headers["Access-Control-Allow-Origin"] = origin;
        ctx.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
        ctx.Response.Headers["Access-Control-Allow-Headers"] = "Authorization, Content-Type";
        ctx.Response.Headers["Access-Control-Max-Age"] = "600";
        ctx.Response.Headers["Vary"] = "Origin";
        ctx.Response.StatusCode = StatusCodes.Status204NoContent;
        return;
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
