# IMEI Connect API - Complete Endpoint Reference

> **Base URL**: `https://api.imeiconnect.com/api/`
> **Auth**: `Authorization: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==`
> **Method**: All endpoints are POST
> **Total**: 31 Controllers, 200+ Endpoints

---

## LoginController (6 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Login/UserLogin` | Request OTP | `mobileNo`, `deviceToken`, `loginType` |
| `Login/PostOTP` | Verify OTP & register device | `mobileNo`, `deviceToken`, `imeiNo`, `deviceName`, `versionNo`, `countryCode`, `loginType` |
| `Login/GetWelcomeScreen` | Get user's groups after login | `masterUID` |
| `Login/GetMemberDetails` | Get member profile data | `masterUID` |
| `Login/Registration` | New user registration | `mobileNo`, `firstName`, `lastName`, `email` |

---

## MemberController (20 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Member/GetDirectoryList` | Member directory | `grpID` |
| `Member/GetMemberListSync` | Sync member list (incremental) | `grpID`, `updatedOn` |
| `Member/GetMemberListSyncIOS` | iOS sync (ZIP) | `grpID`, `updatedOn` |
| `Member/GetMember` | Member details | `profileId` |
| `Member/GetMemberWithDynamicFields` | Member with dynamic fields | `profileId`, `grpId` |
| `Member/UpdateProfile` | Update profile | varies |
| `Member/UpdateProfileDetails` | Update specific field | `key`, `profileID` |
| `Member/UpdateProfilePersonalDetails` | Update personal info | `profileId`, fields... |
| `Member/UpdatePersonalDynamicDtls` | Update dynamic fields | `profileId`, fields... |
| `Member/UpdateAddressDetails` | Update address | `profileId`, address fields |
| `Member/UpdateFamilyDetails` | Update family members | `profileId`, family data |
| `Member/UploadProfilePhoto` | Upload profile image | `ProfileID`, `GrpID`, `Type`, file |
| `Member/GetBdayAnnList` | Birthday/Anniversary list | `grpID` |
| `Member/GetBODList` | Board of Directors | `grpId`, `searchText`, `YearFilter` |
| `Member/DeleteFolder` | Delete folder | varies |
| `Member/ReadMemberFile` | Read member data file | varies |

---

## GroupController (20 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Group/CreateGroup` | Create group | group data |
| `Group/GetAllGroupsList` | List all groups | `masterUID` |
| `Group/GetAllGroupListSync` | Sync groups (session check) | `masterUID`, `imeiNo`, `mobileNo`, `countryCode`, `loginType`, `updatedOn` |
| `Group/GetGroupDetail` | Group details | `groupId` |
| `Group/GetGroupInfo` | Group info | `groupId` |
| `Group/GetClubDetails` | Club details | `groupId` |
| `Group/GetClubHistory` | Club history | `groupId` |
| `Group/GetGroupModulesList` | Available modules | `groupId` |
| `Group/AddSelectedModule` | Enable module | `groupId`, `moduleName` |
| `Group/DeleteByModuleName` | Disable module | `groupId`, `moduleName` |
| `Group/UpdateModuleDashboard` | Update module dashboard | varies |
| `Group/GetModulesList` | All modules | - |
| `Group/GetReplicaInfo` | Module sync info | `groupId` |
| `Group/GetEntityInfo` | Entity info | varies |
| `Group/GetAssistanceGov` | Assistance governance | varies |
| `Group/GetExternalLink` | External links | `groupId` |
| `Group/RemoveSelfFromGroup` | Leave group | `masterUID`, `groupId` |
| `Group/UpdateMemberGroupCategory` | Update category | varies |
| `Group/RemoveGroupCategory` | Remove category | varies |
| `Group/DeleteEntity` | Delete entity | varies |

---

## EventController (11 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Event/GetEventList` | Event listing | `groupId`, page params |
| `Event/GetEventListNew` | New format listing | `groupId` |
| `Event/GetEventLists` | Alternative listing | `groupId` |
| `Event/GetEventDetails` | Event details | `eventId` |
| `Event/GetEventDetails_New` | New format details | `eventId` |
| `Event/AddEvent` | Create/edit event | event data |
| `Event/AddEvent_New` | New format create | event data |
| `Event/AnsweringEvent` | RSVP | `eventId`, `profileId`, `response` |
| `Event/GetEventBySearchText` | Search | `groupId`, `searchText` |
| `Event/Getsmscountdetails` | SMS quota | `groupId` |
| `Event/RotaryIndiaGetEventList` | National events | varies |

---

## AnnouncementController (5 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Announcement/GetAnnouncementList` | List announcements | `groupId`, `memberProfileId`, `searchText`, `moduleId` |
| `Announcement/GetAnnouncementDetails` | Announcement details | `announcementId` |
| `Announcement/AddAnnouncement` | Create announcement | announcement data |
| `Announcement/GetAnnouncementBySearchText` | Search | `groupId`, `searchText` |
| `Announcement/RotaryIndiaAnnouncementlist` | National announcements | varies |

---

## AttendanceController (12 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Attendance/GetAttendanceList` | Attendance list | `GroupId` |
| `Attendance/GetAttendanceListNew` | New format list | `GroupId` |
| `Attendance/GetAttendanceEventsListNew` | Events with attendance | `GroupId` |
| `Attendance/getAttendanceDetails` | Attendance details | `AttendanceID` |
| `Attendance/getAttendanceMemberDetails` | Member attendance | `AttendanceID`, `type=1` |
| `Attendance/getAttendanceVisitorsDetails` | Visitor attendance | `AttendanceID`, `type=2` |
| `Attendance/getAttendanceRotariansDetails` | Rotarian attendance | `AttendanceID` |
| `Attendance/getAttendanceDistrinctDelegateDetails` | District delegates | `AttendanceID` |
| `Attendance/GetrotarianDetailsbyRotarianID` | Rotarian by ID | `rotarianId` |
| `Attendance/GetAttendanceDistrinctDelegateDetailsByRotarianName` | By name | `name` |
| `Attendance/AttendanceAddEdit` | Add/Edit | attendance data |
| `Attendance/AttendanceDelete` | Delete | `AttendanceID` |

---

## GalleryController (19 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Gallery/GetAlbumsList` | Album listing | `groupId` |
| `Gallery/GetAlbumsList_New` | New format (JSON body) | `groupId`, `district_id`, `category_id`, `year`, `clubid`, `SharType`, `moduleId`, `searchText`, `profileId`, `updatedOn` |
| `Gallery/GetAlbumPhotoList` | Album photos | `albumId` |
| `Gallery/GetAlbumPhotoList_New` | New format (JSON body) | `albumId`, `updatedOn` |
| `Gallery/GetAlbumDetails` | Album details | `albumId` |
| `Gallery/GetAlbumDetails_New` | New format details | `albumId` |
| `Gallery/AddUpdateAlbum` | Create/edit album | album data |
| `Gallery/AddUpdateAlbum_New` | New format create | album data |
| `Gallery/AddUpdateAlbumPhoto` | Upload photo | file + metadata |
| `Gallery/DeleteAlbumPhoto` | Delete photo | `photoId` |
| `Gallery/GetShowcaseDetails` | Showcase | `groupId` |
| `Gallery/GetNotificationlist` | Notifications | varies |
| `Gallery/GetMER_List` | MER listing | `FinanceYear`, `TransType` |
| `Gallery/GetYear` | Year list | `Type` |
| `Gallery/Fillyearlist` | Financial year list | varies |
| `Gallery/validateDirectBeneficiaries` | Validate beneficiaries | varies |
| `Gallery/LoadExistingProject` | Load projects | varies |
| `Gallery/GetSubProjectOfOngoing` | Sub-projects | varies |
| `Gallery/GEtBODMember_Cnt` | BOD member count | varies |

---

## CelebrationsController (8 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Celebrations/GetTodaysBirthday` | Today's birthdays | `groupID` |
| `Celebrations/GetMonthEventList` | Monthly celebrations | `groupId`, `month`, `year` |
| `Celebrations/GetMonthEventList_National` | National celebrations | varies |
| `Celebrations/GetEventMinDetails` | Minimal event info | `eventId` |
| `Celebrations/GetMonthEventListTypeWise` | By type | `groupId`, `type` |
| `Celebrations/GetMonthEventListTypeWise_National` | National by type | varies |
| `Celebrations/GetMonthEventListDetails` | Detailed list | `groupId` |
| `Celebrations/GetMonthEventListDetails_National` | National detailed | varies |

---

## FindRotarianController (5 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `FindRotarian/GetRotarianList` | Search Rotarians | search criteria |
| `FindRotarian/GetrotarianDetails` | Rotarian profile | `memberProfileId` |
| `FindRotarian/GetMemberGradeList` | Grade dropdown | - |
| `FindRotarian/GetCategoryList` | Category dropdown | - |
| `FindRotarian/GetClubList` | Club dropdown | - |

---

## FindClubController (9 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `FindClub/GetClubList` | Club directory | search criteria |
| `FindClub/GetClubsNearMe` | Nearby clubs | `latitude`, `longitude` |
| `FindClub/GetClubDetails` | Club info | `clubId` |
| `FindClub/GetClubMembers` | Club members | `clubId` |
| `FindClub/GetPublicAlbumsList` | Public albums | `clubId` |
| `FindClub/GetPublicEventsList` | Public events | `clubId` |
| `FindClub/GetPublicNewsletterList` | Public newsletters | `clubId` |
| `FindClub/GetCommitteelist` | Committee | `clubId` |
| `FindClub/GetCommunicationCount` | Comm stats | `clubId` |

---

## SettingController (4 endpoints)

| Endpoint | Description | Key Params |
|----------|-------------|------------|
| `Setting/GetTouchbaseSetting` | Get app settings | `masterUID` |
| `Setting/TouchbaseSetting` | Update app settings | settings data |
| `Setting/GetGroupSetting` | Get group settings | `groupId`, `groupProfileId` |
| `Setting/GroupSetting` | Update group settings | settings data |

---

## Other Controllers

### PastPresidentsController (1)
| `PastPresidents/getPastPresidentsList` | Past presidents/chairmen | `GroupId`, `SearchText`, `updateOn` |

### DocumentSafeController (4)
| `DocumentSafe/GetDocumentList` | List documents | `groupId` |
| `DocumentSafe/AddDocument` | Upload document | file + metadata |
| `DocumentSafe/UpdateDocumentIsRead` | Mark read | `docId`, `profileId` |
| `DocumentSafe/RotaryIndiaGetDocumentList` | National docs | varies |

### EbulletinController (6)
| `Ebulletin/GetEbulletinList` | List newsletters | `groupId` |
| `Ebulletin/GetEbulletinDetails` | Newsletter details | `bulletinId` |
| `Ebulletin/AddEbulletin` | Create newsletter | data |
| `Ebulletin/GetEbulletinBySearchText` | Search | `searchText` |
| `Ebulletin/GetYearWiseEbulletinList` | By year | `groupId`, `year` |
| `Ebulletin/RotaryIndiaNewsletterlist` | National newsletters | varies |

### ServiceDirectoryController (5)
| `ServiceDirectory/GetServiceDirectoryList` | List services | `groupId` |
| `ServiceDirectory/GetServiceDirectoryDetails` | Service details | `serviceId` |
| `ServiceDirectory/AddServiceDirectory` | Add service | data |
| `ServiceDirectory/GetServiceDirectoryCategories` | Categories | - |
| `ServiceDirectory/GetServiceCategoriesData` | Category data | `categoryId` |

### WebLinkController (2)
| `WebLink/GetWebLinksList` | List web links | `groupId` |
| `WebLink/AddUpdateWebLinks` | Manage links | link data |

### DistrictController (13)
| `District/GetDistrictMemberList` | District members | `districtId` |
| `District/GetDistrictMemberListSync` | Sync members | `districtId`, `updatedOn` |
| `District/GetClubs` | District clubs | `districtId` |
| `District/GetDistrictCommittee` | Committee | `districtId` |
| `District/GetDistrictSubGroupList` | Sub-groups | `districtId` |
| `District/GetClassificationList` | Classifications | `districtId` |
| `District/GetClassificationList_New` | New format | `districtId` |
| `District/GetMemberByClassification` | By classification | `classificationId` |
| `District/GetMemberWithDynamicFields` | Dynamic fields | varies |
| `District/GetPublicAlbumsList` | Public albums | `districtId` |
| `District/GetClubDetails` | Club info | `clubId` |
| `District/GetClubMembers` | Club members | `clubId` |
| `District/GetPublicEventsList` | Public events | `districtId` |

### DistrictCommitteeController (3)
| `DistrictCommittee/districtCommitteeList` | Committee list | `districtId` |
| `DistrictCommittee/districtCommitteeDetails` | Details | `committeeId` |
| `DistrictCommittee/districtCommitteeSearchList` | Search | `searchText` |

### SubGroupDirectoryController (2)
| `SubGroupDirectory/GetSubGrpDirectoryList` | Sub-groups | `groupId` |
| `SubGroupDirectory/GetSubGroupMemberList` | Members | `subGroupId` |

### LeaderBoardController (1)
| `LeaderBoard/GetLeaderBoardDetails` | Leaderboard | `groupId` |

### ClubMonthlyReportController (2)
| `ClubMonthlyReport/GetMonthlyReportList` | Reports | `groupId` |
| `ClubMonthlyReport/SendSMSAndMailToNonSubmitedReports` | Reminders | varies |

### SurveyController (4)
| `Survey/Survey_List` | List surveys | `groupId` |
| `Survey/SurveyDetails` | Details | `surveyId` |
| `Survey/AddEditSevey` | Create/edit | data |
| `Survey/DeleteSevey` | Delete | `surveyId` |

### TicketingController (2)
| `Ticketing/GetTicketList` | List tickets | `masterUID` |
| `Ticketing/AddTicket` | Create ticket | data |

### ImprovementController (3)
| `Improvement/GetImprovementList` | List | `groupId` |
| `Improvement/GetImprovementDetails` | Details | `improvementId` |
| `Improvement/AddImprovement` | Create | data |

### UploadController (2)
| `Upload/UploadImage` | Upload image | file |
| `Upload/UploadAllDocs` | Upload document | file |

### OfflineDataController (2)
| `OfflineData/GetDirectoryListSync` | Offline directory | `groupId` |
| `OfflineData/GetServiceDirectoryListSync` | Offline services | `groupId` |

### versionListController (1)
| `versionList/GetVersionList` | App version | - |