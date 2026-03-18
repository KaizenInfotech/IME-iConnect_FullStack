# IMEI Notification Service - Use Cases

## 1. Event Notifications

### UC-1.1: New Event Push Notification
- When an admin creates/publishes an event, all group members receive a push notification
- Contains: event title, description, venue, date, image, RSVP status, question (if enabled)
- **Project**: `Event_Notification`
- **Stored Proc**: `Web_Event_Notification_Get`
- **FCM Payload**:
  - `type: "Event"`, `moduleName: "Event"`
  - `eventTitle`, `eventDesc`, `venue`, `eventDate`, `eventImg`
  - `rsvpEnable`, `goingCount`, `maybeCount`, `notgoingCount`
  - `questionID`, `questionType`, `questionText`, `isQuesEnable`, `option1`, `option2`

### UC-1.2: Repeating Event Push Notification
- For recurring events, notification is re-sent on each repeat date
- **Project**: `EventRepeat_Notification`
- **Stored Proc**: `Web_EventRepeat_Notification_Get`

---

## 2. Announcement Notifications

### UC-2.1: New Announcement Push Notification
- When an admin posts an announcement, group members receive a push notification
- Contains: announcement title, description, image
- **Project**: `Announcement_Notification`
- **Stored Proc**: `Web_Announcement_Notification_Get`
- **FCM Payload**:
  - `type: "Announcement"`, `moduleName: "Announcement"`
  - `announcementTitle`, `announcementDesc`, `announcementImg`

### UC-2.2: Repeating Announcement Push Notification
- For scheduled/recurring announcements, re-sent on each repeat date
- **Project**: `AnnouncementRepeat_Notification`
- **Stored Proc**: `Web_AnnouncementReapet_Notification_Get`

---

## 3. Birthday & Anniversary Notifications

### UC-3.1: Daily Birthday/Anniversary Data Preparation
- Runs daily to populate temp notification table with today's birthdays and anniversaries
- Inserts records for all members who need to be notified
- **Project**: `Insert_BD_Ann_Into_Temp_Table`
- **Stored Proc**: `Web_All_BD_Ann_InsertInto_TempTable`

### UC-3.2: Individual Birthday/Anniversary Data Preparation
- Same as UC-3.1 but for individual/single person scope
- **Project**: `Insert_BD_Ann_Into_Temp_TableOneperson`

### UC-3.3: Global Birthday/Anniversary Push Notification
- Sends push notifications to all group members about today's birthdays/anniversaries
- **Project**: `Send_Bday_Anni_Notification`
- **Stored Proc**: `Web_Bday_Anni_Notification_Get`
- **FCM Payload**:
  - `type: "Birthday"` or `type: "Anniversary"`
  - Celebration member name, greeting message

### UC-3.4: District-Level Birthday/Anniversary Push Notification
- Sends birthday/anniversary notifications at district level (broader scope)
- **Project**: `Send_Districtlvl_Bday_Anni_Notification`

### UC-3.5: Individual Birthday/Anniversary Push Notification (Club Level)
- Sends notification for a specific person's birthday/anniversary to their club members
- **Project**: `Bday_Ann_for_One_peroson`

### UC-3.6: Individual Birthday/Anniversary Push Notification (District Level)
- Same as UC-3.5 but at district scope
- **Project**: `Bday_Ann_for_One_peroson_Districtlvl`

---

## 4. Activity Notifications

### UC-4.1: Activity Completion Push Notification
- When a club completes/reports an activity, members receive notification
- Includes year parameter for financial year context
- **Project**: `Send_Activity_Notification`
- **Stored Proc**: `Web_Activity_Notification_Get`
- **FCM Payload**:
  - `type: "Activity"`, `moduleName: "Activity"`
  - Activity title, description

---

## 5. Newsletter / E-Bulletin Notifications

### UC-5.1: New Newsletter Push Notification
- When an e-bulletin/newsletter is published, group members receive push notification
- **Project**: `Newsletter_Notification`
- **Stored Proc**: `Web_Newsletter_Notification_Get`
- **FCM Payload**:
  - `type: "Newsletter"`, `moduleName: "Newsletter"`
  - Newsletter title, link

### UC-5.2: Newsletter WhatsApp Balance Check
- Before sending, checks WhatsApp/Email package balance for the group
- **Method**: `GetWhatsAPPEmailCount()`
- **Stored Proc**: `getwhatsappemailbalance`

---

## 6. Document Notifications

### UC-6.1: New Document Push Notification
- When a document is shared in a group, members receive push notification
- **Project**: `Document_Notification`
- **Stored Proc**: `Web_Document_Notification_Get`
- **FCM Payload**:
  - `type: "Document"`, `moduleName: "Document"`
  - Document title

---

## 7. WhatsApp Notifications

### UC-7.1: Event WhatsApp Message
- Sends WhatsApp message for club events via Gallabox API template
- Checks balance before sending; decrements count after success
- **Project**: `Whatsapp_notification`
- **Stored Proc**: `Get_SMS_Whatsapp` (queue), `Get_event_detalis_Whatsapp` (details)
- **API**: `POST https://server.gallabox.com/devapi/messages/whatsapp`
- **Template params**: `name`, `activity_title`, `activity_date`, `activity_category`
- **Status**: `sentflag_whatsapp` (0=pending, 1=sent, 2=failed)

### UC-7.2: WhatsApp Balance Management
- Each group has a WhatsApp/Email package with limited message count
- Balance checked via `getwhatsappemailbalance` stored procedure
- Count decremented from `tbl_whatsappmail_package_master` on successful send

---

## 8. Cross-Cutting Concerns

### UC-8.1: Platform-Specific Payload Handling
- **Android**: FCM data-only message → app constructs notification locally
  ```json
  { "data": { "Message": "...", "type": "Event", ... }, "registration_ids": ["token"] }
  ```
- **iOS**: FCM notification + data message → system tray + app data
  ```json
  { "notification": { "title": "...", "body": "..." }, "data": { ... }, "to": "token" }
  ```
- Device type determined by `device_name` field ("Android" vs "iOS")

### UC-8.2: Notification Status Tracking
- All notifications tracked in `temp_tbl_for_all_notification`
- **SentFlag**: `0` = pending, `1` = sent successfully, `2` = failed
- **sentflag_whatsapp**: Same pattern for WhatsApp messages
- Failed notifications can be retried by resetting flag to 0

### UC-8.3: FCM Token Management
- Each member's device token (`DeviceToken`) stored in member profile
- Token used as `registration_ids` (Android) or `to` (iOS) in FCM request
- Null/empty tokens are skipped

---

## Notification Flow Diagram

```
┌─────────────────┐     ┌──────────────┐     ┌─────────────────┐
│  Main API       │     │  MySQL DB    │     │  Notification   │
│  (Event/Annc/   │────>│  Insert into │<────│  Console App    │
│   Document etc) │     │  temp table  │     │  (Scheduled)    │
└─────────────────┘     └──────────────┘     └────────┬────────┘
                                                       │
                                              ┌────────┴────────┐
                                              │                 │
                                        ┌─────┴─────┐   ┌──────┴──────┐
                                        │  FCM API  │   │  Gallabox   │
                                        │  (Push)   │   │  (WhatsApp) │
                                        └─────┬─────┘   └──────┬──────┘
                                              │                 │
                                        ┌─────┴─────┐   ┌──────┴──────┐
                                        │  Android  │   │  WhatsApp   │
                                        │  iOS App  │   │  Message    │
                                        └───────────┘   └─────────────┘
```

---

## Scheduling (Windows Task Scheduler)

| Console App | Typical Schedule | Purpose |
|-------------|-----------------|---------|
| Insert_BD_Ann_Into_Temp_Table | Daily (early morning) | Prepare B/A data |
| Send_Bday_Anni_Notification | Daily (after insert) | Send B/A push |
| Event_Notification | Every few minutes | Send event push |
| Announcement_Notification | Every few minutes | Send announcement push |
| EventRepeat_Notification | Daily | Repeating events |
| AnnouncementRepeat_Notification | Daily | Repeating announcements |
| Newsletter_Notification | Every few minutes | Newsletter push |
| Document_Notification | Every few minutes | Document push |
| Send_Activity_Notification | Every few minutes | Activity push |
| Whatsapp_notification | Every few minutes | WhatsApp messages |

---

## Stored Procedures Reference

| Procedure | Project | Purpose |
|-----------|---------|---------|
| `Web_Event_Notification_Get` | Event_Notification | Fetch pending event notifications |
| `Web_EventRepeat_Notification_Get` | EventRepeat_Notification | Fetch repeating event notifications |
| `Web_Announcement_Notification_Get` | Announcement_Notification | Fetch pending announcements |
| `Web_AnnouncementReapet_Notification_Get` | AnnouncementRepeat_Notification | Fetch repeating announcements |
| `Web_Bday_Anni_Notification_Get` | Send_Bday_Anni_Notification | Fetch B/A notifications |
| `Web_Activity_Notification_Get` | Send_Activity_Notification | Fetch activity notifications |
| `Web_Newsletter_Notification_Get` | Newsletter_Notification | Fetch newsletter notifications |
| `Web_Document_Notification_Get` | Document_Notification | Fetch document notifications |
| `Web_All_BD_Ann_InsertInto_TempTable` | Insert_BD_Ann_Into_Temp_Table | Populate temp table |
| `Get_SMS_Whatsapp` | Whatsapp_notification | Fetch WhatsApp queue |
| `Get_event_detalis_Whatsapp` | Whatsapp_notification | Get event details for WhatsApp |
| `getwhatsappemailbalance` | All | Check WhatsApp/Email balance |