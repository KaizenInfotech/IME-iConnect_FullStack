# IMEI Connect API - Use Cases

## 1. Authentication & Session Management

### UC-1.1: OTP Login
- User submits mobile number + country code
- System generates 4-digit OTP, sends via SMS/Email
- **Endpoint**: `POST /api/Login/UserLogin`
- **Params**: `mobileNo`, `deviceToken`, `deviceName`, `loginType` (0=Member, 1=Family)

### UC-1.2: OTP Verification & Device Registration
- User submits OTP with device info (IMEI, device token, version)
- System validates OTP, registers device, returns user session data
- Returns: `masterUID`, `grpid0` (branch group), `grpid1` (org group), `memberProfileId`
- **Endpoint**: `POST /api/Login/PostOTP`

### UC-1.3: Welcome Screen / Group Selection
- After login, fetch user's groups (clubs/organizations)
- User selects which group to interact with
- **Endpoint**: `POST /api/Login/GetWelcomeScreen`
- **Params**: `masterUID`

### UC-1.4: Session Expiry (Multi-Device)
- When same mobile logs in from another device, previous device gets `status: "2"` with "session expired"
- Tracked via `imeiNo` + `app_device_token_id` in `member_master_profile`

### UC-1.5: New User Registration
- Non-member inquiry/registration
- **Endpoint**: `POST /api/Login/Registration`

---

## 2. Member Management

### UC-2.1: Member Directory Sync
- Sync full member list for a group (supports incremental via `updatedOn` timestamp)
- Returns new, updated, and deleted records
- **Endpoint**: `POST /api/Member/GetMemberListSync`
- **Params**: `grpID`, `updatedOn`

### UC-2.2: iOS Offline Sync (ZIP)
- Compressed ZIP download of member directory for iOS offline use
- **Endpoint**: `POST /api/Member/GetMemberListSyncIOS`

### UC-2.3: View Member Profile
- Get complete member details including dynamic fields
- **Endpoint**: `POST /api/Member/GetMember`
- **Endpoint**: `POST /api/Member/GetMemberWithDynamicFields`

### UC-2.4: Update Profile Photo
- Multipart upload of profile image
- **Endpoint**: `POST /api/Member/UploadProfilePhoto`
- **Params**: `ProfileID`, `GrpID`, `Type=profile`, `profile_image` (file)

### UC-2.5: Update Personal Details
- Update name, DOB, anniversary, company, hide flags
- **Endpoint**: `POST /api/Member/UpdateProfilePersonalDetails`
- **Endpoint**: `POST /api/Member/UpdatePersonalDynamicDtls`

### UC-2.6: Update Address
- Update residential/business address
- **Endpoint**: `POST /api/Member/UpdateAddressDetails`

### UC-2.7: Update Family Members
- Add/edit/delete family member records
- **Endpoint**: `POST /api/Member/UpdateFamilyDetails`

### UC-2.8: Toggle Privacy Settings
- Hide/show mobile, email, WhatsApp number
- **Endpoint**: `POST /api/Member/UpdateProfileDetails`
- **Params**: key-value pairs like `hide_whatsNum`, `hide_email`, `hide_num`

### UC-2.9: Birthday/Anniversary List
- Get upcoming birthdays and anniversaries for a group
- **Endpoint**: `POST /api/Member/GetBdayAnnList`

### UC-2.10: Board of Directors List
- Get BOD members for a group
- **Endpoint**: `POST /api/Member/GetBODList` (inferred from new backend)

---

## 3. Group / Club Management

### UC-3.1: Create Group
- Create a new club/group
- **Endpoint**: `POST /api/Group/CreateGroup`

### UC-3.2: List All Groups
- Get all groups user belongs to
- **Endpoint**: `POST /api/Group/GetAllGroupsList`

### UC-3.3: Group Sync
- Sync group data with timestamp-based incremental updates
- Checks device registration (session expiry)
- **Endpoint**: `POST /api/Group/GetAllGroupListSync`
- **Params**: `masterUID`, `imeiNo`, `loginType`, `mobileNo`, `countryCode`, `updatedOn`

### UC-3.4: Group Details & Info
- Get detailed group/club information
- **Endpoint**: `POST /api/Group/GetGroupDetail`
- **Endpoint**: `POST /api/Group/GetGroupInfo`
- **Endpoint**: `POST /api/Group/GetClubDetails`

### UC-3.5: Club History
- View historical information about a club
- **Endpoint**: `POST /api/Group/GetClubHistory`

### UC-3.6: Module Management
- List, enable, disable modules for a group (Events, Gallery, Attendance, etc.)
- **Endpoint**: `POST /api/Group/GetGroupModulesList`
- **Endpoint**: `POST /api/Group/AddSelectedModule`
- **Endpoint**: `POST /api/Group/DeleteByModuleName`
- **Endpoint**: `POST /api/Group/UpdateModuleDashboard`

### UC-3.7: Replica Info
- Get module sync metadata
- **Endpoint**: `POST /api/Group/GetReplicaInfo`

### UC-3.8: Leave Group
- Member removes self from a group
- **Endpoint**: `POST /api/Group/RemoveSelfFromGroup`

### UC-3.9: Update Member Category
- Change member's category within a group
- **Endpoint**: `POST /api/Group/UpdateMemberGroupCategory`

### UC-3.10: External Links
- Get external links associated with a group
- **Endpoint**: `POST /api/Group/GetExternalLink`

### UC-3.11: Notification Count
- Get unread notification count
- **Endpoint**: `POST /api/Group/GetNotificationCount` (inferred from new backend)

---

## 4. Events

### UC-4.1: List Events
- Get paginated event list for a group
- **Endpoint**: `POST /api/Event/GetEventList`
- **Endpoint**: `POST /api/Event/GetEventListNew`
- **Endpoint**: `POST /api/Event/GetEventLists`

### UC-4.2: Event Details
- Get full event details including RSVP counts
- **Endpoint**: `POST /api/Event/GetEventDetails`
- **Endpoint**: `POST /api/Event/GetEventDetails_New`

### UC-4.3: Create/Edit Event
- Create or update an event with title, venue, date, description
- **Endpoint**: `POST /api/Event/AddEvent`
- **Endpoint**: `POST /api/Event/AddEvent_New`

### UC-4.4: RSVP to Event
- Respond to event (Going/Maybe/Not Going)
- **Endpoint**: `POST /api/Event/AnsweringEvent`

### UC-4.5: Search Events
- Full-text search on events
- **Endpoint**: `POST /api/Event/GetEventBySearchText`

### UC-4.6: SMS Quota
- Check remaining SMS count for event notifications
- **Endpoint**: `POST /api/Event/Getsmscountdetails`

### UC-4.7: Rotary India Events
- National-level event listing
- **Endpoint**: `POST /api/Event/RotaryIndiaGetEventList`

---

## 5. Announcements

### UC-5.1: List Announcements
- Get announcements for a group (requires valid `memberProfileId`)
- **Endpoint**: `POST /api/Announcement/GetAnnouncementList`
- **Params**: `groupId`, `memberProfileId`, `searchText`, `moduleId`

### UC-5.2: Announcement Details
- Get single announcement with full content
- **Endpoint**: `POST /api/Announcement/GetAnnouncementDetails`

### UC-5.3: Create Announcement
- Post new announcement to group
- **Endpoint**: `POST /api/Announcement/AddAnnouncement`

### UC-5.4: Search Announcements
- Full-text search
- **Endpoint**: `POST /api/Announcement/GetAnnouncementBySearchText`

### UC-5.5: Rotary India Announcements
- National-level announcements
- **Endpoint**: `POST /api/Announcement/RotaryIndiaAnnouncementlist`

---

## 6. Attendance

### UC-6.1: Attendance List
- Get attendance events for a group
- **Endpoint**: `POST /api/Attendance/GetAttendanceListNew`
- **Params**: `GroupId`

### UC-6.2: Member Attendance Details
- View which members attended a specific event
- **Endpoint**: `POST /api/Attendance/getAttendanceMemberDetails`
- **Params**: `AttendanceID`, `type=1`

### UC-6.3: Visitor Attendance
- View visitor details for an attendance event
- **Endpoint**: `POST /api/Attendance/getAttendanceVisitorsDetails`
- **Params**: `AttendanceID`, `type=2`

### UC-6.4: Rotarian Attendance
- View Rotarian attendance from other clubs
- **Endpoint**: `POST /api/Attendance/getAttendanceRotariansDetails`

### UC-6.5: District Delegate Attendance
- Track district delegate visits
- **Endpoint**: `POST /api/Attendance/getAttendanceDistrinctDelegateDetails`

### UC-6.6: Add/Edit Attendance
- Record attendance for an event
- **Endpoint**: `POST /api/Attendance/AttendanceAddEdit`

### UC-6.7: Delete Attendance
- Remove attendance record
- **Endpoint**: `POST /api/Attendance/AttendanceDelete`

---

## 7. Gallery / Photo Management

### UC-7.1: Get Year List
- Get available years for gallery filtering
- **Endpoint**: `POST /api/Gallery/GetYear` (inferred)

### UC-7.2: List Albums
- Get photo albums for a group, filtered by year/category
- **Endpoint**: `POST /api/Gallery/GetAlbumsList`
- **Endpoint**: `POST /api/Gallery/GetAlbumsList_New` (JSON content type required)

### UC-7.3: Album Photos
- Get photos within an album
- **Endpoint**: `POST /api/Gallery/GetAlbumPhotoList`
- **Endpoint**: `POST /api/Gallery/GetAlbumPhotoList_New` (JSON content type required)

### UC-7.4: Album Details
- Get album metadata
- **Endpoint**: `POST /api/Gallery/GetAlbumDetails`
- **Endpoint**: `POST /api/Gallery/GetAlbumDetails_New`

### UC-7.5: Create/Edit Album
- Create or update a photo album
- **Endpoint**: `POST /api/Gallery/AddUpdateAlbum`
- **Endpoint**: `POST /api/Gallery/AddUpdateAlbum_New`

### UC-7.6: Upload Photo
- Add photo to album
- **Endpoint**: `POST /api/Gallery/AddUpdateAlbumPhoto`

### UC-7.7: Delete Photo
- Remove photo from album
- **Endpoint**: `POST /api/Gallery/DeleteAlbumPhoto`

### UC-7.8: Showcase
- Get featured/showcase gallery items
- **Endpoint**: `POST /api/Gallery/GetShowcaseDetails`

### UC-7.9: MER List
- Monitoring & Evaluation Report listing
- **Endpoint**: `POST /api/Gallery/GetMER_List`
- **Params**: `FinanceYear`, `TransType`

### UC-7.10: Fill Year List
- Get financial year list for gallery filtering
- **Endpoint**: `POST /api/Gallery/Fillyearlist`

---

## 8. Celebrations (Birthdays / Anniversaries / Events)

### UC-8.1: Today's Birthdays
- Get today's birthday list for a group
- **Endpoint**: `POST /api/Celebrations/GetTodaysBirthday`
- **Params**: `groupID`

### UC-8.2: Monthly Calendar Events
- Get all celebration events for a month
- **Endpoint**: `POST /api/Celebrations/GetMonthEventList`

### UC-8.3: National Celebrations
- National-level celebration events
- **Endpoint**: `POST /api/Celebrations/GetMonthEventList_National`

### UC-8.4: Events by Type
- Filter celebrations by type (Birthday/Anniversary/Event)
- **Endpoint**: `POST /api/Celebrations/GetMonthEventListTypeWise`

---

## 9. Documents

### UC-9.1: List Documents
- Get documents shared within a group
- **Endpoint**: `POST /api/DocumentSafe/GetDocumentList`

### UC-9.2: Upload Document
- Share a document with group
- **Endpoint**: `POST /api/DocumentSafe/AddDocument`

### UC-9.3: Mark as Read
- Track document read status per member
- **Endpoint**: `POST /api/DocumentSafe/UpdateDocumentIsRead`

### UC-9.4: Rotary India Documents
- National-level shared documents
- **Endpoint**: `POST /api/DocumentSafe/RotaryIndiaGetDocumentList`

---

## 10. E-Bulletins / Newsletters

### UC-10.1: List E-Bulletins
- Get newsletters for a group
- **Endpoint**: `POST /api/Ebulletin/GetEbulletinList`
- **Endpoint**: `POST /api/Ebulletin/GetYearWiseEbulletinList`

### UC-10.2: E-Bulletin Details
- View full newsletter content
- **Endpoint**: `POST /api/Ebulletin/GetEbulletinDetails`

### UC-10.3: Create E-Bulletin
- Publish newsletter
- **Endpoint**: `POST /api/Ebulletin/AddEbulletin`

### UC-10.4: Search E-Bulletins
- Full-text search
- **Endpoint**: `POST /api/Ebulletin/GetEbulletinBySearchText`

---

## 11. Find Rotarian / Find Club

### UC-11.1: Search Rotarians
- Search members across all clubs by name, grade, category
- **Endpoint**: `POST /api/FindRotarian/GetRotarianList`

### UC-11.2: Rotarian Profile
- View detailed Rotarian profile
- **Endpoint**: `POST /api/FindRotarian/GetrotarianDetails`

### UC-11.3: Filter Dropdowns
- Get member grades, categories, club lists for search filters
- **Endpoint**: `POST /api/FindRotarian/GetMemberGradeList`
- **Endpoint**: `POST /api/FindRotarian/GetCategoryList`
- **Endpoint**: `POST /api/FindRotarian/GetClubList`

### UC-11.4: Search Clubs
- Find clubs by name or location
- **Endpoint**: `POST /api/FindClub/GetClubList`

### UC-11.5: Clubs Near Me
- Geo-location based club search
- **Endpoint**: `POST /api/FindClub/GetClubsNearMe`

### UC-11.6: Club Details & Members
- View club info, members, albums, events, newsletters
- **Endpoint**: `POST /api/FindClub/GetClubDetails`
- **Endpoint**: `POST /api/FindClub/GetClubMembers`
- **Endpoint**: `POST /api/FindClub/GetPublicAlbumsList`
- **Endpoint**: `POST /api/FindClub/GetPublicEventsList`
- **Endpoint**: `POST /api/FindClub/GetPublicNewsletterList`

---

## 12. Service Directory

### UC-12.1: List Service Providers
- Business/service directory for a group
- **Endpoint**: `POST /api/ServiceDirectory/GetServiceDirectoryList`

### UC-12.2: Service Details
- View service provider details
- **Endpoint**: `POST /api/ServiceDirectory/GetServiceDirectoryDetails`

### UC-12.3: Add Service Listing
- Register a service/business
- **Endpoint**: `POST /api/ServiceDirectory/AddServiceDirectory`

### UC-12.4: Service Categories
- Get available service categories
- **Endpoint**: `POST /api/ServiceDirectory/GetServiceDirectoryCategories`

---

## 13. Settings

### UC-13.1: App Settings (TouchBase)
- Get/update user-level app settings
- **Endpoint**: `POST /api/Setting/GetTouchbaseSetting`
- **Endpoint**: `POST /api/Setting/TouchbaseSetting`

### UC-13.2: Group Settings
- Get/update group-level settings
- **Endpoint**: `POST /api/Setting/GetGroupSetting`
- **Endpoint**: `POST /api/Setting/GroupSetting`

---

## 14. Past Presidents

### UC-14.1: List Past Presidents / Chairmen
- Get historical president/chairman list for a group
- Same API, different GroupId returns different data:
  - `grpid1` (org group) → Past Presidents (national level)
  - `grpid0` (branch group) → Past Chairmen (club level)
- **Endpoint**: `POST /api/PastPresidents/getPastPresidentsList`
- **Params**: `GroupId`, `SearchText`, `updateOn`

---

## 15. District Management

### UC-15.1: District Member List
- List members at district level
- **Endpoint**: `POST /api/District/GetDistrictMemberList`
- **Endpoint**: `POST /api/District/GetDistrictMemberListSync`

### UC-15.2: District Clubs
- Get clubs within a district
- **Endpoint**: `POST /api/District/GetClubs`

### UC-15.3: District Committee
- View district committee members
- **Endpoint**: `POST /api/District/GetDistrictCommittee`
- **Endpoint**: `POST /api/DistrictCommittee/districtCommitteeList`
- **Endpoint**: `POST /api/DistrictCommittee/districtCommitteeDetails`

### UC-15.4: Classification
- Member classification within district
- **Endpoint**: `POST /api/District/GetClassificationList`
- **Endpoint**: `POST /api/District/GetMemberByClassification`

### UC-15.5: Sub-Groups
- District sub-group management
- **Endpoint**: `POST /api/District/GetDistrictSubGroupList`
- **Endpoint**: `POST /api/SubGroupDirectory/GetSubGrpDirectoryList`
- **Endpoint**: `POST /api/SubGroupDirectory/GetSubGroupMemberList`

---

## 16. Web Links

### UC-16.1: Manage Web Links
- Add/update external web links for a group
- **Endpoint**: `POST /api/WebLink/AddUpdateWebLinks`
- **Endpoint**: `POST /api/WebLink/GetWebLinksList`

---

## 17. Leaderboard

### UC-17.1: View Leaderboard
- Club/member performance rankings
- **Endpoint**: `POST /api/LeaderBoard/GetLeaderBoardDetails`

---

## 18. Club Monthly Reports

### UC-18.1: Monthly Reports
- View/submit club monthly activity reports
- **Endpoint**: `POST /api/ClubMonthlyReport/GetMonthlyReportList`

### UC-18.2: Report Reminders
- Send SMS/Email to clubs that haven't submitted reports
- **Endpoint**: `POST /api/ClubMonthlyReport/SendSMSAndMailToNonSubmitedReports`

---

## 19. Surveys

### UC-19.1: Create/Edit Survey
- **Endpoint**: `POST /api/Survey/AddEditSevey`

### UC-19.2: List Surveys
- **Endpoint**: `POST /api/Survey/Survey_List`

### UC-19.3: Survey Details
- **Endpoint**: `POST /api/Survey/SurveyDetails`

### UC-19.4: Delete Survey
- **Endpoint**: `POST /api/Survey/DeleteSevey`

---

## 20. Support / Feedback

### UC-20.1: Create Support Ticket
- **Endpoint**: `POST /api/Ticketing/AddTicket`

### UC-20.2: List Tickets
- **Endpoint**: `POST /api/Ticketing/GetTicketList`

### UC-20.3: Improvement Suggestions
- **Endpoint**: `POST /api/Improvement/AddImprovement`
- **Endpoint**: `POST /api/Improvement/GetImprovementList`

---

## 21. File Upload

### UC-21.1: Upload Image
- Generic image upload
- **Endpoint**: `POST /api/Upload/UploadImage`

### UC-21.2: Upload Documents
- Generic document upload (PDF, DOC, etc.)
- **Endpoint**: `POST /api/Upload/UploadAllDocs`

---

## 22. Offline Data Sync

### UC-22.1: Directory Offline Sync
- Download full directory for offline use
- **Endpoint**: `POST /api/OfflineData/GetDirectoryListSync`

### UC-22.2: Service Directory Offline Sync
- **Endpoint**: `POST /api/OfflineData/GetServiceDirectoryListSync`

---

## 23. Version Management

### UC-23.1: App Version Check
- Check for app updates
- **Endpoint**: `POST /api/versionList/GetVersionList`