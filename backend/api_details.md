# TouchBase Flutter — API Reference

**Base URL:** `https://api.imeiconnect.com/api/`
**Auth Header:** `Authorization: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==`
**Content-Type:** `application/json` (unless noted as form-encoded or multipart)
**HTTP Method:** All requests are **POST** unless noted otherwise
**Timeouts:** 30s default, 120s for Celebrations endpoints

> User identity is sent via request body parameters (`masterUID`, `grpID`, `profileId`) — NOT headers.

---

## Table of Contents

1. [Login / Auth](#1-login--auth)
2. [Group / Dashboard](#2-group--dashboard)
3. [Member / Directory](#3-member--directory)
4. [Profile](#4-profile)
5. [Events](#5-events)
6. [Announcements](#6-announcements)
7. [Gallery](#7-gallery)
8. [Documents](#8-documents)
9. [E-Bulletin](#9-e-bulletin)
10. [Celebrations](#10-celebrations)
11. [Attendance](#11-attendance)
12. [Notifications / Settings](#12-notifications--settings)
13. [Find Club](#13-find-club)
14. [Find Rotarian](#14-find-rotarian)
15. [District](#15-district)
16. [Service Directory](#16-service-directory)
17. [Leaderboard](#17-leaderboard)
18. [Subgroups](#18-subgroups)
19. [Governing Council](#19-governing-council)
20. [Sub Committee](#20-sub-committee)
21. [Branch / Chapter](#21-branch--chapter)
22. [MER / iMélange](#22-mer--imélange)
23. [Web Links](#23-web-links)
24. [Past Presidents](#24-past-presidents)
25. [Upload](#25-upload)
26. [Version Check](#26-version-check)
27. [District Committee](#27-district-committee)
28. [Monthly Report](#28-monthly-report)

---

## 1. Login / Auth

> Auth endpoints use **form-encoded** (`application/x-www-form-urlencoded`) instead of JSON.

### 1.1 `Login/UserLogin`
**Purpose:** Send OTP to mobile number for login.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `mobileNo`    | String | User's mobile number       |
| `deviceToken` | String | FCM device token           |
| `countryCode`  | String | Country calling code (e.g. "91") |
| `loginType`   | String | Login type identifier      |

### 1.2 `Login/PostOTP`
**Purpose:** Verify OTP and authenticate user.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `mobileNo`    | String | User's mobile number       |
| `deviceToken` | String | FCM device token           |
| `countryCode`  | String | Country calling code       |
| `deviceName`  | String | "iOS" or "Android"         |
| `imeiNo`      | String | Device unique identifier   |
| `versionNo`   | String | App version number         |
| `loginType`   | String | Login type identifier      |

### 1.3 `Login/GetWelcomeScreen`
**Purpose:** Get list of groups/clubs for the welcome screen after login.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `masterUID`   | String | User's master unique ID    |
| `mobileno`    | String | User's mobile number       |
| `loginType`   | String | Login type identifier      |

### 1.4 `Login/GetMemberDetails`
**Purpose:** Get member details after login.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `masterUID`   | String | User's master unique ID    |

### 1.5 `Login/Registration`
**Purpose:** Register a new user.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `mobileNo`    | String | Mobile number              |
| `countryCode`  | String | Country calling code       |
| `firstName`   | String | First name                 |
| `lastName`    | String | Last name                  |
| `email`       | String | Email address              |

---

## 2. Group / Dashboard

### 2.1 `Group/GetAllGroupsList`
**Purpose:** Fetch all groups/clubs the user belongs to.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `masterUID`   | String | User's master unique ID    |
| `imeiNo`      | String | Device identifier (empty string) |

### 2.2 `Group/GetGroupModulesList`
**Purpose:** Fetch module list for a group's dashboard.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `groupId`          | String | Group ID                   |
| `memberProfileId`  | String | Member's profile ID in group |

### 2.3 `Group/GetNewDashboard`
**Purpose:** Fetch dashboard banners and data.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `groupId`          | String | Group ID                   |
| `memberProfileId`  | String | Member's profile ID        |

*Also called from BannerListScreen with:*
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `MasterId`  | String | Master UID                 |

### 2.4 `Group/GetNotificationCount`
**Purpose:** Get unread notification count for badge display.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `groupId`          | String | Group ID                   |
| `memberProfileId`  | String | Member's profile ID        |

### 2.5 `Group/getAdminSubModules`
**Purpose:** Fetch admin sub-module list.
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `Fk_groupID`    | String | Group ID                   |
| `fk_ProfileID`  | String | Profile ID                 |

### 2.6 `Group/UpdateModuleDashboard`
**Purpose:** Save reordered module list on dashboard.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `memberProfileId`  | String | Member's profile ID        |
| `modulelist`       | String | Comma-separated module IDs |

### 2.7 `Group/GetAssistanceGov`
**Purpose:** Get Assistance Governor details.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `grpID`     | String | Group ID                   |
| `profileId` | String | Profile ID                 |

### 2.8 `Group/UpdateMemberGroupCategory`
**Purpose:** Update member's group category (My Clubs/Others).
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `memberProfileId`  | String | Member's profile ID        |
| `mycategory`       | String | Category value             |
| `memberMainId`     | String | Member's main ID           |

### 2.9 `Group/GetAllGroupListSync`
**Purpose:** Sync groups and check session validity.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `masterUID`   | String | User's master unique ID    |
| `imeiNo`      | String | Device identifier          |
| `loginType`   | String | Login type                 |
| `mobileNo`    | String | Mobile number              |
| `countryCode`  | String | Country code               |
| `updatedOn`   | String | Last sync date             |

### 2.10 `Group/UpdateDeviceTokenNumber`
**Purpose:** Update FCM token on server after login.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `MobileNumber` | String | User's mobile number       |
| `DeviceToken`  | String | FCM device token           |

### 2.11 `Group/GetEntityInfo`
**Purpose:** Get entity information (e.g. About Developer).
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `grpID`     | String | Group ID (e.g. "11111" for About Developer) |
| `moduleID`  | String | Module ID (e.g. "101")     |

### 2.12 `Group/GetAllCountriesAndCategories`
**Purpose:** Fetch all countries and group categories for group creation.
| Parameter | Type | Description |
|-----------|------|-------------|
| *(none)*  | —    | Empty body  |

### 2.13 `Group/CreateGroup`
**Purpose:** Create a new group/club.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `grpId`        | String | Group ID (empty for new)   |
| `grpName`      | String | Group name                 |
| `grpImageID`   | String | Group image ID             |
| `grpType`      | String | Group type                 |
| `grpCategory`  | String | Group category             |
| `addrss1`      | String | Address line 1             |
| `addrss2`      | String | Address line 2             |
| `city`         | String | City                       |
| `state`        | String | State                      |
| `pincode`      | String | PIN/ZIP code               |
| `country`      | String | Country                    |
| `emailid`      | String | Email address              |
| `mobile`       | String | Mobile number              |
| `userId`       | String | User ID (creator)          |
| `website`      | String | Website URL                |
| `other`        | String | Other info                 |

### 2.14 `Group/GetGroupDetail`
**Purpose:** Get details of a specific group.
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `memberMainId`  | String | Member's main ID           |
| `groupId`       | String | Group ID                   |

### 2.15 `Group/AddMemberToGroup`
**Purpose:** Add a single member to a group.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `mobile`       | String | Member's mobile number     |
| `userName`     | String | Member's name              |
| `groupId`      | String | Group ID                   |
| `masterID`     | String | Master user ID             |
| `countryId`    | String | Country ID                 |
| `memberEmail`  | String | Member's email             |

### 2.16 `Group/AddMultipleMemberToGroup`
**Purpose:** Add multiple members to a group at once.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `groupId`     | String | Group ID                   |
| `masterID`    | String | Master user ID             |
| `memberData`  | String | Bulk member data           |

### 2.17 `Group/GlobalSearchGroup`
**Purpose:** Search groups globally.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `memId`       | String | Current member ID          |
| `otherMemId`  | String | Other member ID to search  |

### 2.18 `Group/DeleteByModuleName`
**Purpose:** Delete a module item (Event, Document, Ebulletin, etc.).
| Parameter     | Type   | Description                          |
|---------------|--------|--------------------------------------|
| `typeID`      | String | ID of the item to delete             |
| `type`        | String | Module name ("Document", "Ebulletin", etc.) |
| `profileID`   | String | Profile ID performing the delete     |

*Also called from GroupsProvider with different params:*
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `groupId`     | String | Group ID                   |
| `moduleName`  | String | Module name                |
| `moduleId`    | String | Module ID                  |

### 2.19 `Group/DeleteImage`
**Purpose:** Delete a group image.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |
| `imageId`   | String | Image ID to delete         |

### 2.20 `Group/RemoveGroupCategory`
**Purpose:** Remove a member's group category.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `memberProfileId`  | String | Member's profile ID        |
| `memberMainId`     | String | Member's main ID           |

### 2.21 `Group/GetEmail`
**Purpose:** Get email details for a group.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |
| `profileId` | String | Profile ID                 |

### 2.22 `Group/GetRotaryLibraryData`
**Purpose:** Fetch Rotary Library data.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |

### 2.23 `Group/getMobilePupup`
**Purpose:** Get mobile popup data.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |
| `profileId` | String | Profile ID                 |

### 2.24 `Group/UpdateMobilePupupflag`
**Purpose:** Mark mobile popup as seen.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |
| `profileId` | String | Profile ID                 |
| `popupId`   | String | Popup ID to mark as read   |

### 2.25 `Group/Feedback`
**Purpose:** Submit user feedback.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |
| `profileId` | String | Profile ID                 |
| `feedback`  | String | Feedback text              |

### 2.26 `Group/GetClubDetails`
**Purpose:** Get club details.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |

### 2.27 `Group/GetClubHistory`
**Purpose:** Get club history.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |

### 2.28 `Group/getZonelist`
**Purpose:** Get list of zones.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `grpId`     | String | Group ID                   |

### 2.29 `Group/RemoveSelfFromGroup`
**Purpose:** Remove self from a group.
*(Parameters not yet implemented in provider)*

### 2.30 `Group/GetRotaryIndiaAdminModules`
**Purpose:** Get Rotary India admin modules.
*(Parameters not yet implemented in provider)*

---

## 3. Member / Directory

### 3.1 `Member/GetDirectoryList`
**Purpose:** Fetch paginated directory member list.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `masterUID`   | String | User's master unique ID    |
| `grpID`       | String | Group ID                   |
| `searchText`  | String | Search keyword             |
| `page`        | String | Page number                |

### 3.2 `Member/GetMember`
**Purpose:** Get a single member's full details.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `memberProfileId`  | String | Member's profile ID        |
| `groupId`          | String | Group ID                   |

### 3.3 `Member/UpdateProfile`
**Purpose:** Quick update member profile.
| Parameter        | Type   | Description                |
|------------------|--------|----------------------------|
| `ProfileId`      | String | Profile ID                 |
| `memberMobile`   | String | Mobile number              |
| `memberName`     | String | Full name                  |
| `memberEmailid`  | String | Email address              |
| `ProfilePicPath` | String | Profile picture path       |
| `ImageId`        | String | Image ID                   |

### 3.4 `Member/UpdateProfilePersonalDetails`
**Purpose:** Update member's personal detail fields.
| Parameter   | Type   | Description                        |
|-------------|--------|------------------------------------|
| `profileID` | String | Profile ID                         |
| `key`       | String | JSON-encoded list of `{key, value}` maps |

### 3.5 `Member/UpdateAddressDetails`
**Purpose:** Add or update member address.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `addressID`   | String | Address ID (empty for new) |
| `addressType` | String | Type (Home/Office etc.)    |
| `address`     | String | Street address             |
| `city`        | String | City                       |
| `state`       | String | State                      |
| `country`     | String | Country                    |
| `pincode`     | String | PIN/ZIP code               |
| `phoneNo`     | String | Phone number               |
| `fax`         | String | Fax number                 |
| `profileID`   | String | Profile ID                 |
| `groupID`     | String | Group ID                   |

### 3.6 `Member/UpdateFamilyDetails`
**Purpose:** Add or update family member details.
| Parameter        | Type   | Description                |
|------------------|--------|----------------------------|
| `familyMemberId` | String | Family member ID (empty for new) |
| `memberName`     | String | Family member name         |
| `relationship`   | String | Relationship               |
| `dOB`            | String | Date of birth              |
| `anniversary`    | String | Anniversary date           |
| `contactNo`      | String | Contact number             |
| `particulars`    | String | Additional details         |
| `profileId`      | String | Profile ID                 |
| `emailID`        | String | Email address              |
| `bloodGroup`     | String | Blood group                |

### 3.7 `Member/GetMemberListSync`
**Purpose:** Sync full member list for a group (offline data).
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `grpID`       | String | Group ID                   |
| `updatedOn`   | String | Last sync timestamp (e.g. "1970-01-01 00:00:00") |

### 3.8 `Member/GetMemberDetails` *(GET request)*
**Purpose:** Get member details with photo (used on Dashboard).
| Parameter       | Type   | Sent As             |
|-----------------|--------|---------------------|
| `MemProfileId`  | String | Query param + Header |
| `GrpID`         | String | Query param + Header |

### 3.9 `Member/GetBODList`
**Purpose:** Get Board of Directors list.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `grpId`       | String | Group ID                   |
| `searchText`  | String | Search keyword             |
| `YearFilter`  | String | Year filter                |

### 3.10 `Member/GetGoverningCouncl`
**Purpose:** Get Governing Council members.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `searchText`  | String | Search keyword             |
| `YearFilter`  | String | Year filter                |

### 3.11 `Member/UpdateMemebr`
**Purpose:** Update member toggle settings (hide mobile/email, etc.).
| Parameter            | Type   | Description                |
|----------------------|--------|----------------------------|
| `mobile_num_hide`    | String | "0" or "1"                 |
| `secondary_num_hide` | String | "0" or "1"                 |
| `email_hide`         | String | "0" or "1"                 |
| `DOB`                | String | Date of birth              |
| `DOA`                | String | Date of anniversary        |
| `company_name`       | String | Company name               |
| `MemProfileId`       | String | Member profile ID          |

### 3.12 `Member/UploadProfilePhoto` *(Multipart POST)*
**Purpose:** Upload member's profile photo.
| Parameter       | Type      | Sent As         |
|-----------------|-----------|-----------------|
| `ProfileID`     | String    | Query param + Form field |
| `GrpID`         | String    | Query param + Form field |
| `Type`          | String    | Query param + Form field (value: "profile") |
| `profile_image` | File      | Multipart file   |

### 3.13 `Member/GetUpdatedmemberProfileDetails`
**Purpose:** Get updated profile details for edit.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `profileId` | String | Profile ID                 |
| `groupId`   | String | Group ID                   |

### 3.14 `member/UpdateProfileDetails`
**Purpose:** Update profile detail fields.
| Parameter   | Type   | Description                        |
|-------------|--------|------------------------------------|
| `key`       | String | JSON-encoded list of `{key, value}` maps |
| `profileID` | String | Profile ID                         |

### 3.15 `Member/Saveprofile`
**Purpose:** Submit profile change request.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `MemberId`  | String | Member ID                  |
| `remark`    | String | Remark/reason for change   |
| `Category`  | String | Category ID                |

---

## 4. Profile

> Profile APIs are a subset of Member APIs — see Section 3 for `UpdateFamilyDetails`, `UpdateAddressDetails`, `UploadProfilePhoto`, etc.

### 4.1 `FindRotarian/GetCategoryList` *(used in Profile for categories dropdown)*
| Parameter | Type | Description |
|-----------|------|-------------|
| *(none)*  | —    | Empty body  |

---

## 5. Events

### 5.1 `Event/GetEventList`
**Purpose:** Fetch paginated event list.
| Parameter         | Type   | Description                |
|-------------------|--------|----------------------------|
| `groupProfileID`  | String | Group profile ID           |
| `grpId`           | String | Group ID                   |
| `Type`            | String | Event type filter          |
| `Admin`           | String | Admin flag                 |
| `searchText`      | String | Search keyword             |
| `pageIndex`       | String | Page number                |

### 5.2 `Event/GetEventDetails`
**Purpose:** Get details of a specific event.
| Parameter         | Type   | Description                |
|-------------------|--------|----------------------------|
| `groupProfileID`  | String | Group profile ID           |
| `eventID`         | String | Event ID                   |
| `grpId`           | String | Group ID                   |

### 5.3 `Event/AddEvent_New`
**Purpose:** Create or update an event.
| Parameter            | Type   | Description                |
|----------------------|--------|----------------------------|
| `eventID`            | String | Event ID (empty for new)   |
| `questionEnable`     | String | Enable question flag       |
| `eventType`          | String | Event type                 |
| `membersIDs`         | String | Comma-separated member IDs |
| `eventImageID`       | String | Event image ID             |
| `evntTitle`          | String | Event title                |
| `evntDesc`           | String | Event description          |
| `eventVenue`         | String | Event venue                |
| `venueLat`           | String | Venue latitude             |
| `venueLong`          | String | Venue longitude            |
| `evntDate`           | String | Event date                 |
| `evntTime`           | String | Event time (empty string)  |
| `publishDate`        | String | Publish date               |
| `publishTime`        | String | Publish time (empty string)|
| `expiryDate`         | String | Expiry date                |
| `expiryTime`         | String | Expiry time (empty string) |
| `notifyDate`         | String | Notification date          |
| `notifyTime`         | String | Notify time (empty string) |
| `userID`             | String | Creator user ID            |
| `grpID`              | String | Group ID                   |
| `RepeatDateTime`     | String | Repeat date/time           |
| `questionType`       | String | Question type              |
| `questionText`       | String | Question text              |
| `option1`            | String | Option 1 text              |
| `option2`            | String | Option 2 text              |
| `sendSMSNonSmartPh`  | String | Send SMS to non-smartphone |
| `sendSMSAll`         | String | Send SMS to all            |
| `rsvpEnable`         | String | RSVP enable flag           |
| `displayonbanner`    | String | Show on banner flag        |
| `reglink`            | String | Registration link          |
| `isSubGrpAdmin`      | String | Is sub-group admin         |

### 5.4 `Event/AnsweringEvent`
**Purpose:** RSVP / answer event question.
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `grpId`         | String | Group ID                   |
| `profileID`     | String | Profile ID                 |
| `eventId`       | String | Event ID                   |
| `joiningStatus` | String | RSVP status                |
| `questionId`    | String | Question ID                |
| `answerByme`    | String | User's answer              |

### 5.5 `Event/Getsmscountdetails`
**Purpose:** Get SMS count details for notifications.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `grpId`     | String | Group ID                   |
| `profileID` | String | Profile ID                 |

---

## 6. Announcements

### 6.1 `Announcement/GetAnnouncementList`
**Purpose:** Fetch announcement list.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `groupId`          | String | Group ID                   |
| `memberProfileId`  | String | Member profile ID          |
| `searchText`       | String | Search keyword             |
| `moduleId`         | String | Module ID                  |

### 6.2 `Announcement/GetAnnouncementDetails`
**Purpose:** Get details of a specific announcement.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `announID`         | String | Announcement ID            |
| `grpID`            | String | Group ID                   |
| `memberProfileID`  | String | Member profile ID          |

### 6.3 `Announcement/AddAnnouncement`
**Purpose:** Create or update an announcement.
| Parameter                    | Type   | Description                |
|------------------------------|--------|----------------------------|
| `announID`                   | String | Announcement ID (empty for new) |
| `annType`                    | String | Announcement type          |
| `announTitle`                | String | Title                      |
| `announceDEsc`               | String | Description                |
| `memID`                      | String | Creator member ID          |
| `grpID`                      | String | Group ID                   |
| `inputIDs`                   | String | Input IDs                  |
| `announImg`                  | String | Image ID                   |
| `publishDate`                | String | Publish date               |
| `expiryDate`                 | String | Expiry date                |
| `sendSMSNonSmartPh`          | String | Send SMS to non-smartphone |
| `sendSMSAll`                 | String | Send SMS to all            |
| `moduleId`                   | String | Module ID                  |
| `AnnouncementRepeatDates`    | String | Repeat dates               |
| `reglink`                    | String | Registration link          |
| `isSubGrpAdmin`              | String | Is sub-group admin         |

---

## 7. Gallery

### 7.1 `Gallery/GetAlbumsList`
**Purpose:** Fetch album list for a group.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `profileId`   | String | Profile ID                 |
| `groupId`     | String | Group ID                   |
| `updatedOn`   | String | Last sync date             |
| `moduleId`    | String | Module ID                  |

### 7.2 `Gallery/GetAlbumsList_New`
**Purpose:** Fetch showcase/filtered album list.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `groupId`      | String | Group ID                   |
| `district_id`  | String | District ID                |
| `club_id`      | String | Club ID                    |
| `category_id`  | String | Category ID                |
| `year`         | String | Year filter                |
| `SharType`     | String | Share type                 |
| `profileId`    | String | Profile ID                 |
| `moduleId`     | String | Module ID                  |
| `searchText`   | String | Search keyword             |

### 7.3 `Gallery/GetAlbumPhotoList_New`
**Purpose:** Fetch photos in an album.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `albumId`     | String | Album ID                   |
| `groupId`     | String | Group ID                   |
| `updatedOn`   | String | Last sync date             |

### 7.4 `Gallery/GetAlbumDetails_New`
**Purpose:** Get details of a specific album.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `albumId`   | String | Album ID                   |

### 7.5 `Gallery/AddUpdateAlbum_New`
**Purpose:** Create or update an album.
| Parameter            | Type   | Description                |
|----------------------|--------|----------------------------|
| `albumId`            | String | Album ID (empty for new)   |
| `groupId`            | String | Group ID                   |
| `type`               | String | Album type                 |
| `memberIds`          | String | Comma-separated member IDs |
| `albumTitle`         | String | Album title                |
| `albumDescription`   | String | Album description          |
| `albumImage`         | String | Album cover image          |
| `createdBy`          | String | Creator profile ID         |
| `isSubGrpAdmin`      | String | Is sub-group admin         |
| `subgrpIDs`          | String | Sub-group IDs              |
| `moduleId`           | String | Module ID                  |
| `shareType`          | String | Share type                 |
| `categoryId`         | String | Category ID                |
| `dateofproject`      | String | Project date               |
| `costofproject`      | String | Project cost               |
| `beneficiary`        | String | Beneficiary count          |
| `manhourspent`       | String | Man hours spent            |
| `Manhourspenttype`   | String | Man hours type             |
| `NumberofRotarian`   | String | Number of Rotarians        |
| `OtherCategorytext`  | String | Other category text        |
| `costofprojecttype`  | String | Cost of project type       |

### 7.6 `Gallery/AddUpdateAlbumPhoto` *(Multipart POST)*
**Purpose:** Upload a photo to an album.
| Parameter   | Type   | Sent As         |
|-------------|--------|-----------------|
| `photoId`   | String | Query parameter  |
| `desc`      | String | Query parameter (URL-encoded) |
| `albumId`   | String | Query parameter  |
| `groupId`   | String | Query parameter  |
| `createdBy` | String | Query parameter  |
| `userfile`  | File   | Multipart file   |

### 7.7 `Gallery/DeleteAlbumPhoto`
**Purpose:** Delete a photo from an album.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `photoId`   | String | Photo ID to delete         |
| `albumId`   | String | Album ID                   |
| `deletedBy` | String | Profile ID of deleter      |

### 7.8 `Gallery/GetYear`
**Purpose:** Get available years for MER/iMélange.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `Type`    | String | "1" for MER, "2" for iMélange |

### 7.9 `Gallery/Fillyearlist`
**Purpose:** Get year list for a group.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `grpID`   | String | Group ID                   |

### 7.10 `Gallery/GetMER_List`
**Purpose:** Fetch MER list.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `FinanceYear`  | String | Finance year               |
| `TransType`    | String | "1" for MER, "2" for iMélange |

---

## 8. Documents

### 8.1 `DocumentSafe/GetDocumentList`
**Purpose:** Fetch document list.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `grpID`            | String | Group ID                   |
| `memberProfileID`  | String | Member profile ID          |
| `type`             | String | Document type filter       |
| `isAdmin`          | String | Admin flag                 |
| `searchText`       | String | Search keyword             |

### 8.2 `DocumentSafe/AddDocument` *(Multipart POST)*
**Purpose:** Upload a document.
| Parameter   | Type   | Sent As         |
|-------------|--------|-----------------|
| `grpID`     | String | Form field       |
| `profileID` | String | Form field       |
| `docTitle`  | String | Form field       |
| `userfile`  | File   | Multipart file (filename: `{name}.{docType}`) |

### 8.3 `DocumentSafe/UpdateDocumentIsRead`
**Purpose:** Mark a document as read.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `DocID`            | String | Document ID                |
| `memberProfileID`  | String | Member profile ID          |

---

## 9. E-Bulletin

### 9.1 `Ebulletin/GetYearWiseEbulletinList`
**Purpose:** Fetch e-bulletins filtered by year.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `memberProfileId`  | String | Member profile ID          |
| `groupId`          | String | Group ID                   |
| `YearFilter`       | String | Selected year              |

### 9.2 `Ebulletin/AddEbulletin`
**Purpose:** Create or update an e-bulletin.
| Parameter           | Type   | Description                |
|---------------------|--------|----------------------------|
| `ebulletinID`       | String | E-bulletin ID (empty for new) |
| `ebulletinType`     | String | Type                       |
| `ebulletinTitle`    | String | Title                      |
| `ebulletinlink`     | String | External link              |
| `ebulletinfileid`   | String | Uploaded file ID           |
| `memID`             | String | Creator member ID          |
| `grpID`             | String | Group ID                   |
| `inputIDs`          | String | Input IDs                  |
| `publishDate`       | String | Publish date               |
| `expiryDate`        | String | Expiry date                |
| `sendSMSAll`        | String | Send SMS to all            |
| `isSubGrpAdmin`     | String | Is sub-group admin         |

---

## 10. Celebrations

> Celebrations endpoints use **120-second timeout**.

### 10.1 `Celebrations/GetMonthEventList`
**Purpose:** Fetch month-wise celebration events (birthdays, anniversaries).
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `profileId`     | String | Profile ID                 |
| `groupIds`      | String | Group ID                   |
| `selectedDate`  | String | Date (format: YYYY-MM-01)  |
| `updatedOns`    | String | Last sync date (default: "2019/01/01 00:00:00") |
| `groupCategory` | String | Group category ("2")       |

### 10.2 `Celebrations/GetMonthEventListTypeWise_National`
**Purpose:** Fetch celebrations filtered by type (birthday/anniversary/event).
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `GroupID`        | String | Group ID                   |
| `groupCategory` | String | Group category ("2")       |
| `SelectedDate`  | String | Date (format: YYYY-MM-DD)  |
| `Type`          | String | "B" (birthday), "A" (anniversary), "E" (event) |

### 10.3 `Celebrations/GetMonthEventListDetails_National`
**Purpose:** Fetch date-wise celebration details.
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `GroupID`        | String | Group ID                   |
| `SelectedDate`  | String | Selected date              |
| `GroupCategory` | String | Group category ("2")       |

### 10.4 `Celebrations/GetEventMinDetails`
**Purpose:** Get minimal event details.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `eventID`   | String | Event ID                   |

### 10.5 `Celebrations/GetTodaysBirthday`
**Purpose:** Get today's birthday list.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupID`   | String | Group ID                   |

---

## 11. Attendance

### 11.1 `Attendance/GetAttendanceListNew`
**Purpose:** Fetch attendance list.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `GroupId`   | String | Group ID                   |

### 11.2 `Attendance/getAttendanceDetails`
**Purpose:** Get attendance record details.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `AttendanceID` | String | Attendance record ID       |

### 11.3 `Attendance/AttendanceDelete`
**Purpose:** Delete an attendance record.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `AttendanceID` | String | Attendance record ID       |
| `createdBy`    | String | Creator profile ID         |

### 11.4 `Attendance/getAttendanceMemberDetails`
**Purpose:** Get member attendance details.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `AttendanceID` | String | Attendance record ID       |
| `type`         | String | Type ("1" for members)     |

### 11.5 `Attendance/getAttendanceVisitorsDetails`
**Purpose:** Get visitor attendance details.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `AttendanceID` | String | Attendance record ID       |
| `type`         | String | Type ("2" for visitors)    |

---

## 12. Notifications / Settings

### 12.1 `setting/GetTouchbaseSetting`
**Purpose:** Get user's TouchBase notification settings.
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `mainMasterId`  | String | Main master ID             |

### 12.2 `setting/TouchbaseSetting`
**Purpose:** Update a TouchBase notification setting.
| Parameter       | Type   | Description                |
|-----------------|--------|----------------------------|
| `GroupId`        | String | Group ID                   |
| `UpdatedValue`  | String | "0" (off) or "1" (on)      |
| `mainMasterId`  | String | Main master ID             |

### 12.3 `Setting/GetGroupSetting`
**Purpose:** Get group-level notification settings.
| Parameter        | Type   | Description                |
|------------------|--------|----------------------------|
| `GroupProfileId`  | String | Group profile ID           |
| `GroupId`         | String | Group ID                   |

### 12.4 `Setting/GroupSetting`
**Purpose:** Update a group-level notification setting.
| Parameter                | Type   | Description                |
|--------------------------|--------|----------------------------|
| `GroupId`                | String | Group ID                   |
| `ModuleId`               | String | Module ID                  |
| `GroupProfileId`          | String | Group profile ID           |
| `UpdatedValue`           | String | "0" (off) or "1" (on)      |
| `showMobileSeflfClub`    | String | Show mobile to own club    |
| `showMobileOutsideClub`  | String | Show mobile to other clubs |
| `showEmailSeflfClub`     | String | Show email to own club     |
| `showEmailOutsideClub`   | String | Show email to other clubs  |
| `isMob`                  | String | Mobile visibility flag     |
| `isEmail`                | String | Email visibility flag      |
| `isPersonal`             | String | Personal info flag         |
| `isFamily`               | String | Family info flag           |
| `isBusiness`             | String | Business info flag         |

---

## 13. Find Club

### 13.1 `FindClub/GetClubList`
**Purpose:** Search for clubs.
| Parameter            | Type   | Description                |
|----------------------|--------|----------------------------|
| `keyword`            | String | Search keyword             |
| `country`            | String | Country filter             |
| `stateProvinceCity`  | String | State/Province/City filter |
| `district`           | String | District filter            |

### 13.2 `FindClub/GetClubsNearMe`
**Purpose:** Find clubs near a location.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `lat`     | String | Latitude                   |
| `longi`   | String | Longitude                  |

### 13.3 `FindClub/GetClubDetails`
**Purpose:** Get club details.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `grpId`   | String | Group ID                   |

### 13.4 `FindClub/GetClubMembers`
**Purpose:** Get members of a club.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `grpId`   | String | Group ID                   |

### 13.5 `FindClub/GetPublicAlbumsList`
**Purpose:** Get public albums of a club.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `grpId`   | String | Group ID                   |

### 13.6 `FindClub/GetPublicEventsList`
**Purpose:** Get public events of a club.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `grpId`   | String | Group ID                   |

### 13.7 `FindClub/GetPublicNewsletterList`
**Purpose:** Get public newsletters of a club.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `grpId`   | String | Group ID                   |

### 13.8 `FindClub/GetCommitteelist` *(GET request)*
**Purpose:** Get committee list for a club.
| Parameter | Type | Description |
|-----------|------|-------------|
| *(none)*  | —    | No parameters (GET request) |

---

## 14. Find Rotarian

### 14.1 `FindRotarian/GetZonechapterlist`
**Purpose:** Get zone/chapter list dropdown.
| Parameter | Type | Description |
|-----------|------|-------------|
| *(none)*  | —    | Empty body  |

### 14.2 `FindRotarian/GetRotarianList`
**Purpose:** Search for Rotarians.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `name`         | String | Name search                |
| `Grade`        | String | Member grade filter        |
| `memberMobile` | String | Mobile number search       |
| `club`         | String | Club filter                |
| `Category`     | String | Category filter            |

### 14.3 `FindRotarian/GetrotarianDetails`
**Purpose:** Get Rotarian member details.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `memberProfileId`  | String | Member's profile ID        |

### 14.4 `FindRotarian/GetMemberGradeList`
**Purpose:** Get member grade dropdown values.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `name`         | String | Empty string               |
| `Grade`        | String | Empty string               |
| `club`         | String | Empty string               |
| `Category`     | String | Empty string               |
| `memberMobile` | String | Empty string               |

### 14.5 `FindRotarian/GetClubList`
**Purpose:** Get club dropdown values.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `name`         | String | Empty string               |
| `Grade`        | String | Empty string               |
| `club`         | String | Empty string               |
| `Category`     | String | Empty string               |
| `memberMobile` | String | Empty string               |

### 14.6 `FindRotarian/GetCategoryList`
**Purpose:** Get category dropdown values.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `name`         | String | Empty string               |
| `Grade`        | String | Empty string               |
| `club`         | String | Empty string               |
| `Category`     | String | Empty string               |
| `memberMobile` | String | Empty string               |

---

## 15. District

### 15.1 `District/GetDistrictMemberListSync`
**Purpose:** Fetch paginated district member list.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `masterUID`   | String | Master UID                 |
| `grpID`       | String | Group ID                   |
| `searchText`  | String | Search keyword             |
| `pageNo`      | String | Page number                |
| `recordCount` | String | Records per page ("20")    |

### 15.2 `District/GetClassificationList_New`
**Purpose:** Get classification list for a district.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `grpID`       | String | Group ID                   |
| `pageNo`      | String | Page number ("1")          |
| `recordCount` | String | Records per page ("100")   |
| `searchText`  | String | Search keyword             |

### 15.3 `District/GetMemberByClassification`
**Purpose:** Get members filtered by classification.
| Parameter        | Type   | Description                |
|------------------|--------|----------------------------|
| `classification` | String | Classification name        |
| `grpID`          | String | Group ID                   |

### 15.4 `District/GetClubs`
**Purpose:** Get clubs in a district.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | District group ID          |
| `search`    | String | Search keyword (empty)     |

### 15.5 `District/GetMemberWithDynamicFields`
**Purpose:** Get district member details with dynamic fields.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `memberProfileId`  | String | Member profile ID          |
| `groupId`          | String | Group ID                   |

### 15.6 `District/GetDistrictCommittee`
**Purpose:** Get district committee list.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |

---

## 16. Service Directory

### 16.1 `ServiceDirectory/GetServiceCategoriesData`
**Purpose:** Get service directory categories.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Group ID                   |

### 16.2 `ServiceDirectory/GetServiceDirectoryDetails`
**Purpose:** Get details of a service directory entry.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `groupId`      | String | Group ID                   |
| `serviceDirId` | String | Service directory ID       |

### 16.3 `ServiceDirectory/AddServiceDirectory`
**Purpose:** Create or update a service directory entry.
| Parameter        | Type   | Description                |
|------------------|--------|----------------------------|
| `serviceId`      | String | Service ID (empty for new) |
| `groupId`        | String | Group ID                   |
| `memberName`     | String | Member/business name       |
| `description`    | String | Description                |
| `image`          | String | Image ID                   |
| `countryCode1`   | String | Country code (primary)     |
| `mobileNo1`      | String | Mobile number (primary)    |
| `countryCode2`   | String | Country code (secondary)   |
| `mobileNo2`      | String | Mobile number (secondary)  |
| `paxNo`          | String | PAX number                 |
| `email`          | String | Email address              |
| `keywords`       | String | Search keywords            |
| `address`        | String | Street address             |
| `latitude`       | String | Latitude                   |
| `longitude`      | String | Longitude                  |
| `createdBy`      | String | Creator profile ID         |
| `city`           | String | City                       |
| `state`          | String | State                      |
| `addressCountry` | String | Country                    |
| `zipcode`        | String | ZIP/PIN code               |
| `moduleId`       | String | Module ID                  |
| `website`        | String | Website URL                |

---

## 17. Leaderboard

### 17.1 `LeaderBoard/GetLeaderBoardDetails`
**Purpose:** Fetch leaderboard data.
| Parameter    | Type   | Description                |
|--------------|--------|----------------------------|
| `GroupID`    | String | Group ID                   |
| `RowYear`    | String | Selected year              |
| `ProfileID`  | String | Profile ID                 |
| `fk_zoneid`  | String | Zone ID (default: "0")     |

---

## 18. Subgroups

### 18.1 `Group/GetSubGroupList`
**Purpose:** Get list of subgroups.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Parent group ID            |

### 18.2 `Group/GetSubGroupDetail`
**Purpose:** Get subgroup details.
| Parameter   | Type   | Description                |
|-------------|--------|----------------------------|
| `groupId`   | String | Parent group ID            |
| `subgrpId`  | String | Subgroup ID                |

### 18.3 `Group/CreateSubGroup`
**Purpose:** Create a new subgroup.
| Parameter          | Type   | Description                |
|--------------------|--------|----------------------------|
| `subGroupTitle`    | String | Subgroup name              |
| `memberProfileId`  | String | Comma-separated member profile IDs |
| `groupId`          | String | Parent group ID            |
| `memberMainId`     | String | Creator's main ID          |
| `parentID`         | String | Parent ID                  |

---

## 19. Governing Council

### 19.1 `Member/GetGoverningCouncl`
**Purpose:** Get Governing Council members.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `searchText`  | String | Search keyword             |
| `YearFilter`  | String | Year filter                |

---

## 20. Sub Committee

### 20.1 `FindClub/GetCommitteelist` *(GET request)*
**Purpose:** Get committee list.
| Parameter | Type | Description |
|-----------|------|-------------|
| *(none)*  | —    | No parameters |

---

## 21. Branch / Chapter

### 21.1 `FindClub/GetClubList` *(reused)*
**Purpose:** Fetch branches/chapters list.
| Parameter            | Type   | Description                |
|----------------------|--------|----------------------------|
| `keyword`            | String | Search keyword (empty)     |
| `country`            | String | Country (empty)            |
| `meetingDay`         | String | Meeting day (empty)        |
| `district`           | String | District (empty)           |
| `stateProvinceCity`  | String | State/Province (empty)     |

### 21.2 `Member/GetMemberListSync` *(reused)*
**Purpose:** Fetch branch member details.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `grpID`       | String | Branch group ID            |
| `updatedOn`   | String | "1970-01-01 00:00:00"      |

### 21.3 `Gallery/Fillyearlist` *(reused)*
**Purpose:** Get year list for branch.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `grpID`   | String | Branch group ID            |

### 21.4 `Gallery/GetAlbumsList_New` *(reused)*
**Purpose:** Fetch past events for a branch.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `groupId`      | String | Group ID                   |
| `district_id`  | String | "2"                        |
| `category_id`  | String | "1"                        |
| `year`         | String | Selected year              |
| `clubid`       | String | Empty string               |
| `SharType`     | String | "0"                        |
| `moduleId`     | String | "8"                        |
| `searchText`   | String | Empty string               |

### 21.5 `Gallery/GetAlbumPhotoList_New` *(reused)*
**Purpose:** Fetch past event photos.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `albumId`      | String | Album ID                   |
| `Financeyear`  | String | Finance year               |
| `groupId`      | String | Group ID                   |

---

## 22. MER / iMélange

### 22.1 `Gallery/GetYear`
**Purpose:** Get available finance years.
| Parameter | Type   | Description                |
|-----------|--------|----------------------------|
| `Type`    | String | "1" for MER, "2" for iMélange |

### 22.2 `Gallery/GetMER_List`
**Purpose:** Fetch MER or iMélange list.
| Parameter      | Type   | Description                |
|----------------|--------|----------------------------|
| `FinanceYear`  | String | Selected finance year      |
| `TransType`    | String | "1" for MER, "2" for iMélange |

---

## 23. Web Links

### 23.1 `WebLink/GetWebLinksList`
**Purpose:** Fetch web links for a group.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `GroupId`     | String | Group ID                   |
| `searchText`  | String | Search keyword             |

---

## 24. Past Presidents

### 24.1 `PastPresidents/getPastPresidentsList`
**Purpose:** Fetch past presidents list.
| Parameter     | Type   | Description                |
|---------------|--------|----------------------------|
| `GroupId`     | String | Group ID                   |
| `SearchText`  | String | Search keyword (empty)     |
| `updateOn`    | String | "1970/01/01 00:00:00"      |

---

## 25. Upload

### 25.1 `upload/UploadImage` *(Multipart POST)*
**Purpose:** Upload an image (used across modules for event, announcement, album images).
| Parameter | Type | Sent As |
|-----------|------|---------|
| `userfile` | File | Multipart file |

---

## 26. Version Check

### 26.1 `versionList/GetVersionList`
**Purpose:** Check app version for force update.
*(Endpoint defined but not yet called from providers)*

---

## 27. District Committee

### 27.1 `DistrictCommittee/districtCommitteeDetails`
**Purpose:** Get district committee details.
*(Endpoint defined but not yet called from providers)*

---

## 28. Monthly Report

### 28.1 `ClubMonthlyReport/GetMonthlyReportList`
**Purpose:** Get monthly report list.
*(Endpoint defined but not yet called from providers)*

### 28.2 `ClubMonthlyReport/SendSMSAndMailToNonSubmitedReports`
**Purpose:** Send SMS/email reminders for non-submitted reports.
*(Endpoint defined but not yet called from providers)*

---

## API Response Format

All API responses follow this standard structure:

```json
{
  "isSuccess": true,
  "message": "Success",
  "data": { ... },
  "serverError": null,
  "errorName": null
}
```

Or the legacy format:

```json
{
  "TB{ResultName}Result": {
    "status": "0",
    "Result": { ... }
  }
}
```

**Status codes:**
- `"0"` — Success
- `"1"` — Error / No data
- HTTP `401` — Session expired → triggers re-login
