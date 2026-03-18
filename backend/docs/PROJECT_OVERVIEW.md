# IMEI Connect API - Project Overview

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | ASP.NET Web API 2 (MVC5) |
| Runtime | .NET Framework 4.7.2 |
| Language | C# |
| Database | MySQL 8.0+ (imei_production) |
| ORM | Entity Framework 6.4.4 |
| JSON | Newtonsoft.Json 13.0.1 |
| Hosting | IIS (Windows Server) |
| Auth | OTP-based (SMS/Email) |

## Architecture

```
Controller (31) → BusinessEntity (29) → MySqlHelper / EF → MySQL (250+ tables)
```

- **All endpoints are POST** (even read operations)
- **Base URL**: `https://api.imeiconnect.com/api/`
- **Auth Header**: `Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==`
- **Response format**: `{ "XxxResult": { "status": "0", "message": "success", ... } }`
- **Stored Procedures**: Versioned with `V1_` through `V7_` prefixes
- **Data Access**: Mix of Entity Framework LINQ and raw stored procedures

## Project Structure

```
TouchBaseWebAPI/
├── App_Start/              # WebApiConfig, RouteConfig, FilterConfig
├── BusinessEntities/       # Business logic layer (29 service files)
├── Controllers/            # API Controllers (31 controllers, 200+ endpoints)
├── Data/                   # Entity Framework EDMX + auto-generated entities (248 files)
├── Models/                 # DTOs and request/response models (28 files)
├── Documents/              # File upload storage
├── TempDocuments/          # Temporary file storage (ZIP sync)
├── Views/                  # MVC Views (minimal)
├── Global.asax             # Application entry point
└── Web.config              # Configuration (DB, SMTP, file paths)
```

## Database

- **Server**: `101.53.148.126` (production)
- **Database**: `imei_production`
- **Connection Pool**: Max 100 connections
- **Key Tables**: `main_member_master`, `member_master_profile`, `group_master`, `event_master`, `announcements_master`, `attentance_master`, `gallery_image_details_*`

## External Integrations

- **Email**: SMTP via Gmail (`membership@imare.in`)
- **SMS**: Configured via database credentials table
- **File Storage**: `C:\inetpub\vhosts\UpgradeRI_API\` → served as `https://imeiconnect.com/Documents/`