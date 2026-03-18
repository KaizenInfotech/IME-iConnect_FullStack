# IMEI Notification Service - Project Overview

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | .NET Framework 4.7.2 |
| Language | C# |
| Type | 14 Console Applications (scheduled via Windows Task Scheduler) |
| Database | MySQL 8.0+ (imei_production) |
| Push Notifications | Firebase Cloud Messaging (FCM) |
| WhatsApp | Gallabox API (server.gallabox.com) |
| Architecture | x86 Console Apps |

## Architecture

```
Windows Task Scheduler
    ↓ (triggers on schedule)
Console App (Program.cs)
    ↓
Stored Procedure → MySQL (pending notifications from temp_tbl_for_all_notification)
    ↓
FCM / WhatsApp API → Mobile Devices
    ↓
Update DB status (SentFlag: 0=pending, 1=sent, 2=failed)
```

## Project Structure

```
IMEI-Notification/
├── Event_Notification/                    # Push for events
├── EventRepeat_Notification/              # Push for repeating events
├── Announcement_Notification/             # Push for announcements
├── AnnouncementRepeat_Notification/       # Push for repeating announcements
├── Send_Bday_Anni_Notification/           # Push for birthdays/anniversaries (global)
├── Send_Districtlvl_Bday_Anni_Notification/ # Push for B/A (district level)
├── Bday_Ann_for_One_peroson/              # Push for B/A (individual/club level)
├── Bday_Ann_for_One_peroson_Districtlvl/  # Push for B/A (individual district)
├── Send_Activity_Notification/            # Push for activity completions
├── Newsletter_Notification/               # Push for e-bulletins/newsletters
├── Document_Notification/                 # Push for documents
├── Whatsapp_notification/                 # WhatsApp messages via Gallabox
├── Insert_BD_Ann_Into_Temp_Table/         # Populates temp table with today's B/A
├── Insert_BD_Ann_Into_Temp_TableOneperson/ # Populates temp table (individual)
└── SMSAndNotification.sln                 # Visual Studio solution
```

Each project follows the same pattern:
```
<Project>/
├── Program.cs                  # Entry point + FCM/WhatsApp sending logic
├── cls_<Name>Notification.cs   # DB query wrapper (stored procedure calls)
├── GlobalFuns.cs               # WhatsApp API HTTP helper
├── GlobalVars.cs               # Connection string accessor
├── MySqlHelper.cs              # MySQL utility
└── App.config                  # Connection string, image path config
```

## Database

- **Database**: `imei_production` (same as main API)
- **User**: `IMEI_Notification_user`
- **Key Table**: `temp_tbl_for_all_notification` (notification queue)
- **Quota Table**: `tbl_whatsappmail_package_master` (WhatsApp/Email balance per group)
- **Status Flags**: `SentFlag` (0=pending, 1=sent, 2=failed), `sentflag_whatsapp`

## External Services

### FCM (Firebase Cloud Messaging)
- **Endpoint**: `https://fcm.googleapis.com/fcm/send`
- **Android**: Data-only payload (app handles display)
- **iOS**: Notification + data payload (system tray display)

### WhatsApp (Gallabox API)
- **Endpoint**: `https://server.gallabox.com/devapi/messages/whatsapp`
- **Type**: Template-based messages
- **Balance checked** before sending; count decremented per message

## Execution Model

These are **not long-running services**. Each console app:
1. Starts (triggered by Task Scheduler or manually)
2. Queries DB for pending notifications
3. Sends all pending items via FCM/WhatsApp
4. Updates status in DB
5. Exits