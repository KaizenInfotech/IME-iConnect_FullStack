-- ============================================================
-- TouchBase Database Schema - MySQL 9.x
-- Generated from EF Core Entity Models & AppDbContext
-- Database: touchbase_db
-- ============================================================

CREATE DATABASE IF NOT EXISTS `touchbase_db`
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE `touchbase_db`;

-- ============================================================
-- DROP TABLES (reverse dependency order)
-- ============================================================

DROP TABLE IF EXISTS `popup_statuses`;
DROP TABLE IF EXISTS `popups`;
DROP TABLE IF EXISTS `feedbacks`;
DROP TABLE IF EXISTS `leaderboard_entries`;
DROP TABLE IF EXISTS `mer_items`;
DROP TABLE IF EXISTS `chapters`;
DROP TABLE IF EXISTS `zones`;
DROP TABLE IF EXISTS `banners`;
DROP TABLE IF EXISTS `web_links`;
DROP TABLE IF EXISTS `past_presidents`;
DROP TABLE IF EXISTS `group_settings`;
DROP TABLE IF EXISTS `touchbase_settings`;
DROP TABLE IF EXISTS `service_directory_entries`;
DROP TABLE IF EXISTS `service_categories`;
DROP TABLE IF EXISTS `attendance_visitors`;
DROP TABLE IF EXISTS `attendance_members`;
DROP TABLE IF EXISTS `attendance_records`;
DROP TABLE IF EXISTS `album_photos`;
DROP TABLE IF EXISTS `albums`;
DROP TABLE IF EXISTS `ebulletins`;
DROP TABLE IF EXISTS `document_read_statuses`;
DROP TABLE IF EXISTS `documents`;
DROP TABLE IF EXISTS `announcements`;
DROP TABLE IF EXISTS `event_responses`;
DROP TABLE IF EXISTS `events`;
DROP TABLE IF EXISTS `sub_group_members`;
DROP TABLE IF EXISTS `sub_groups`;
DROP TABLE IF EXISTS `group_modules`;
DROP TABLE IF EXISTS `group_members`;
DROP TABLE IF EXISTS `clubs`;
DROP TABLE IF EXISTS `groups`;
DROP TABLE IF EXISTS `districts`;
DROP TABLE IF EXISTS `notifications`;
DROP TABLE IF EXISTS `device_tokens`;
DROP TABLE IF EXISTS `address_details`;
DROP TABLE IF EXISTS `family_members`;
DROP TABLE IF EXISTS `member_profiles`;
DROP TABLE IF EXISTS `users`;
DROP TABLE IF EXISTS `countries`;
DROP TABLE IF EXISTS `categories`;

-- ============================================================
-- STANDALONE / REFERENCE TABLES
-- ============================================================

CREATE TABLE `users` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `MobileNo` VARCHAR(20) NULL,
    `CountryCode` VARCHAR(10) NULL,
    `DeviceToken` VARCHAR(512) NULL,
    `DeviceName` VARCHAR(255) NULL,
    `FirstName` VARCHAR(255) NULL,
    `MiddleName` VARCHAR(255) NULL,
    `LastName` VARCHAR(255) NULL,
    `Email` VARCHAR(255) NULL,
    `ProfileImage` VARCHAR(512) NULL,
    `ImeiMemId` VARCHAR(100) NULL,
    `ImeiNo` VARCHAR(100) NULL,
    `VersionNo` VARCHAR(50) NULL,
    `LoginType` VARCHAR(50) NULL,
    `Otp` VARCHAR(10) NULL,
    `OtpExpiry` DATETIME(6) NULL,
    `IsRegistered` TINYINT(1) NOT NULL DEFAULT 0,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    UNIQUE INDEX `IX_users_MobileNo` (`MobileNo`),
    INDEX `IX_users_Email` (`Email`)
) ENGINE=InnoDB;

CREATE TABLE `districts` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `DistrictName` VARCHAR(255) NULL,
    `DistrictNumber` VARCHAR(50) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

CREATE TABLE `countries` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `CountryCode` VARCHAR(10) NULL,
    `CountryName` VARCHAR(255) NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

CREATE TABLE `categories` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `CatName` VARCHAR(255) NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

CREATE TABLE `zones` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `ZoneName` VARCHAR(255) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

CREATE TABLE `service_categories` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `CategoryName` VARCHAR(255) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB;

-- ============================================================
-- MEMBER PROFILES & RELATED
-- ============================================================

CREATE TABLE `member_profiles` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `MemberName` VARCHAR(255) NULL,
    `MemberEmail` VARCHAR(255) NULL,
    `MemberMobile` VARCHAR(20) NULL,
    `MemberCountry` VARCHAR(100) NULL,
    `ProfilePic` VARCHAR(512) NULL,
    `FamilyPic` VARCHAR(512) NULL,
    `Designation` VARCHAR(255) NULL,
    `CompanyName` VARCHAR(255) NULL,
    `BloodGroup` VARCHAR(10) NULL,
    `CountryCode` VARCHAR(10) NULL,
    `Dob` VARCHAR(50) NULL,
    `Doa` VARCHAR(50) NULL,
    `SecondaryMobileNo` VARCHAR(20) NULL,
    `WhatsappNum` VARCHAR(20) NULL,
    `MembershipGrade` VARCHAR(100) NULL,
    `ImeiMembershipId` VARCHAR(100) NULL,
    `Classification` VARCHAR(255) NULL,
    `Category` VARCHAR(255) NULL,
    `CategoryId` VARCHAR(50) NULL,
    `HideNum` VARCHAR(10) NULL,
    `HideMail` VARCHAR(10) NULL,
    `HideWhatsnum` VARCHAR(10) NULL,
    `IsPersonalDetVisible` VARCHAR(10) NULL,
    `IsBusinDetVisible` VARCHAR(10) NULL,
    `IsFamilDetailVisible` VARCHAR(10) NULL,
    `MobileNumHide` VARCHAR(10) NULL,
    `SecondaryNumHide` VARCHAR(10) NULL,
    `EmailHide` VARCHAR(10) NULL,
    `DobHide` VARCHAR(10) NULL,
    `DoaHide` VARCHAR(10) NULL,
    `CompanyNameHide` VARCHAR(10) NULL,
    `Keywords` TEXT NULL,
    `DonorRecognition` VARCHAR(255) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_member_profiles_UserId` (`UserId`),
    CONSTRAINT `FK_member_profiles_users_UserId` FOREIGN KEY (`UserId`)
        REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `family_members` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `MemberProfileId` INT NOT NULL,
    `MemberName` VARCHAR(255) NULL,
    `Relationship` VARCHAR(100) NULL,
    `Dob` VARCHAR(50) NULL,
    `Anniversary` VARCHAR(50) NULL,
    `ContactNo` VARCHAR(20) NULL,
    `Particulars` TEXT NULL,
    `EmailId` VARCHAR(255) NULL,
    `BloodGroup` VARCHAR(10) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_family_members_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_family_members_member_profiles_MemberProfileId` FOREIGN KEY (`MemberProfileId`)
        REFERENCES `member_profiles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `address_details` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `MemberProfileId` INT NOT NULL,
    `AddressType` VARCHAR(50) NULL,
    `Address` TEXT NULL,
    `City` VARCHAR(255) NULL,
    `State` VARCHAR(255) NULL,
    `Country` VARCHAR(255) NULL,
    `Pincode` VARCHAR(20) NULL,
    `PhoneNo` VARCHAR(20) NULL,
    `Fax` VARCHAR(50) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_address_details_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_address_details_member_profiles_MemberProfileId` FOREIGN KEY (`MemberProfileId`)
        REFERENCES `member_profiles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- DEVICE TOKENS & NOTIFICATIONS
-- ============================================================

CREATE TABLE `device_tokens` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `Token` VARCHAR(512) NULL,
    `Platform` VARCHAR(50) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_device_tokens_UserId` (`UserId`),
    CONSTRAINT `FK_device_tokens_users_UserId` FOREIGN KEY (`UserId`)
        REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `notifications` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `Title` VARCHAR(500) NULL,
    `Details` TEXT NULL,
    `Type` VARCHAR(50) NULL,
    `ClubDistrictType` VARCHAR(50) NULL,
    `NotifyDate` VARCHAR(50) NULL,
    `ExpiryDate` VARCHAR(50) NULL,
    `SortDate` VARCHAR(50) NULL,
    `ReadStatus` VARCHAR(10) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_notifications_UserId` (`UserId`),
    CONSTRAINT `FK_notifications_users_UserId` FOREIGN KEY (`UserId`)
        REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- GROUPS
-- ============================================================

CREATE TABLE `groups` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GrpName` VARCHAR(255) NULL,
    `GrpImg` VARCHAR(512) NULL,
    `GrpImageId` VARCHAR(255) NULL,
    `GrpType` VARCHAR(50) NULL,
    `GrpCategory` VARCHAR(100) NULL,
    `Address1` TEXT NULL,
    `Address2` TEXT NULL,
    `City` VARCHAR(255) NULL,
    `State` VARCHAR(255) NULL,
    `Pincode` VARCHAR(20) NULL,
    `Country` VARCHAR(255) NULL,
    `Email` VARCHAR(255) NULL,
    `Mobile` VARCHAR(20) NULL,
    `Website` VARCHAR(512) NULL,
    `Other` TEXT NULL,
    `TotalMembers` INT NOT NULL DEFAULT 0,
    `DistrictId` INT NULL,
    `ClubHistory` TEXT NULL,
    `ContactNo` VARCHAR(20) NULL,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_groups_GrpName` (`GrpName`),
    INDEX `IX_groups_DistrictId` (`DistrictId`),
    CONSTRAINT `FK_groups_districts_DistrictId` FOREIGN KEY (`DistrictId`)
        REFERENCES `districts` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ============================================================
-- GROUP MEMBERS & MODULES
-- ============================================================

CREATE TABLE `group_members` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `MyCategory` VARCHAR(255) NULL,
    `IsGrpAdmin` VARCHAR(10) NULL,
    `GrpProfileId` VARCHAR(100) NULL,
    `ModuleId` VARCHAR(100) NULL,
    `MemberMainId` VARCHAR(100) NULL,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    UNIQUE INDEX `IX_group_members_GroupId_MemberProfileId` (`GroupId`, `MemberProfileId`),
    INDEX `IX_group_members_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_group_members_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_group_members_member_profiles_MemberProfileId` FOREIGN KEY (`MemberProfileId`)
        REFERENCES `member_profiles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `group_modules` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `ModuleId` VARCHAR(100) NULL,
    `ModuleName` VARCHAR(255) NULL,
    `ModuleStaticRef` VARCHAR(255) NULL,
    `Image` VARCHAR(512) NULL,
    `MasterProfileId` VARCHAR(100) NULL,
    `IsCustomized` VARCHAR(10) NULL,
    `ModuleOrderNo` VARCHAR(10) NULL,
    `NotificationCount` VARCHAR(20) NULL,
    `ModulePriceRs` VARCHAR(20) NULL,
    `ModulePriceUS` VARCHAR(20) NULL,
    `ModuleInfo` TEXT NULL,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_group_modules_GroupId` (`GroupId`),
    CONSTRAINT `FK_group_modules_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- SUB-GROUPS
-- ============================================================

CREATE TABLE `sub_groups` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `SubgrpTitle` VARCHAR(255) NULL,
    `ParentId` INT NULL,
    `CreatedBy` VARCHAR(100) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_sub_groups_GroupId` (`GroupId`),
    CONSTRAINT `FK_sub_groups_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `sub_group_members` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `SubGroupId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_sub_group_members_SubGroupId` (`SubGroupId`),
    INDEX `IX_sub_group_members_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_sub_group_members_sub_groups_SubGroupId` FOREIGN KEY (`SubGroupId`)
        REFERENCES `sub_groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- CLUBS
-- ============================================================

CREATE TABLE `clubs` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NULL,
    `ClubName` VARCHAR(255) NULL,
    `ClubType` VARCHAR(50) NULL,
    `ClubId` VARCHAR(100) NULL,
    `District` VARCHAR(255) NULL,
    `DistrictId` INT NULL,
    `CharterDate` VARCHAR(50) NULL,
    `MeetingDay` VARCHAR(50) NULL,
    `MeetingTime` VARCHAR(50) NULL,
    `Website` VARCHAR(512) NULL,
    `Lat` VARCHAR(50) NULL,
    `Longi` VARCHAR(50) NULL,
    `Address` TEXT NULL,
    `City` VARCHAR(255) NULL,
    `State` VARCHAR(255) NULL,
    `Country` VARCHAR(255) NULL,
    `PresidentName` VARCHAR(255) NULL,
    `PresidentMobile` VARCHAR(20) NULL,
    `PresidentEmail` VARCHAR(255) NULL,
    `SecretaryName` VARCHAR(255) NULL,
    `SecretaryMobile` VARCHAR(20) NULL,
    `SecretaryEmail` VARCHAR(255) NULL,
    `GovernorName` VARCHAR(255) NULL,
    `GovernorMobile` VARCHAR(20) NULL,
    `GovernorEmail` VARCHAR(255) NULL,
    `Distance` VARCHAR(50) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_clubs_ClubName` (`ClubName`),
    INDEX `IX_clubs_GroupId` (`GroupId`),
    CONSTRAINT `FK_clubs_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ============================================================
-- EVENTS
-- ============================================================

CREATE TABLE `events` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `EventTitle` VARCHAR(500) NULL,
    `EventDesc` TEXT NULL,
    `EventType` VARCHAR(50) NULL,
    `EventImageId` VARCHAR(255) NULL,
    `EventVenue` VARCHAR(500) NULL,
    `VenueLat` VARCHAR(50) NULL,
    `VenueLon` VARCHAR(50) NULL,
    `EventDate` VARCHAR(50) NULL,
    `PublishDate` VARCHAR(50) NULL,
    `ExpiryDate` VARCHAR(50) NULL,
    `NotifyDate` VARCHAR(50) NULL,
    `RepeatDateTime` VARCHAR(100) NULL,
    `RsvpEnable` VARCHAR(10) NULL,
    `QuestionEnable` VARCHAR(10) NULL,
    `QuestionType` VARCHAR(50) NULL,
    `QuestionText` TEXT NULL,
    `Option1` VARCHAR(255) NULL,
    `Option2` VARCHAR(255) NULL,
    `DisplayOnBanner` VARCHAR(10) NULL,
    `RegLink` VARCHAR(512) NULL,
    `SendSMSNonSmartPh` VARCHAR(10) NULL,
    `SendSMSAll` VARCHAR(10) NULL,
    `InputIds` TEXT NULL,
    `MembersIds` TEXT NULL,
    `Link` VARCHAR(512) NULL,
    `ProjectName` VARCHAR(255) NULL,
    `FilterType` VARCHAR(50) NULL,
    `IsSubGrpAdmin` VARCHAR(10) NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_events_GroupId` (`GroupId`),
    INDEX `IX_events_EventDate` (`EventDate`),
    CONSTRAINT `FK_events_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `event_responses` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `EventId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `JoiningStatus` VARCHAR(50) NULL,
    `AnswerByMe` VARCHAR(255) NULL,
    `QuestionId` VARCHAR(100) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    UNIQUE INDEX `IX_event_responses_EventId_MemberProfileId` (`EventId`, `MemberProfileId`),
    INDEX `IX_event_responses_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_event_responses_events_EventId` FOREIGN KEY (`EventId`)
        REFERENCES `events` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_event_responses_member_profiles_MemberProfileId` FOREIGN KEY (`MemberProfileId`)
        REFERENCES `member_profiles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- ANNOUNCEMENTS
-- ============================================================

CREATE TABLE `announcements` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `AnnounTitle` VARCHAR(500) NULL,
    `AnnounDesc` TEXT NULL,
    `AnnounType` VARCHAR(50) NULL,
    `AnnounImg` VARCHAR(512) NULL,
    `PublishDate` VARCHAR(50) NULL,
    `ExpiryDate` VARCHAR(50) NULL,
    `SendSMSNonSmartPh` VARCHAR(10) NULL,
    `SendSMSAll` VARCHAR(10) NULL,
    `ModuleId` VARCHAR(100) NULL,
    `RegLink` VARCHAR(512) NULL,
    `InputIds` TEXT NULL,
    `RepeatDates` TEXT NULL,
    `FilterType` VARCHAR(50) NULL,
    `IsSubGrpAdmin` VARCHAR(10) NULL,
    `Link` VARCHAR(512) NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_announcements_GroupId` (`GroupId`),
    CONSTRAINT `FK_announcements_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- DOCUMENTS
-- ============================================================

CREATE TABLE `documents` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `DocTitle` VARCHAR(500) NULL,
    `DocType` VARCHAR(50) NULL,
    `DocURL` VARCHAR(512) NULL,
    `DocAccessType` VARCHAR(50) NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_documents_GroupId` (`GroupId`),
    CONSTRAINT `FK_documents_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `document_read_statuses` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `DocumentId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `IsRead` VARCHAR(10) NULL,
    `ReadAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    UNIQUE INDEX `IX_document_read_statuses_DocumentId_MemberProfileId` (`DocumentId`, `MemberProfileId`),
    INDEX `IX_document_read_statuses_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_document_read_statuses_documents_DocumentId` FOREIGN KEY (`DocumentId`)
        REFERENCES `documents` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_document_read_statuses_member_profiles_MemberProfileId` FOREIGN KEY (`MemberProfileId`)
        REFERENCES `member_profiles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- E-BULLETINS
-- ============================================================

CREATE TABLE `ebulletins` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `EbulletinTitle` VARCHAR(500) NULL,
    `EbulletinLink` VARCHAR(512) NULL,
    `EbulletinType` VARCHAR(50) NULL,
    `EbulletinFileId` VARCHAR(255) NULL,
    `PublishDate` VARCHAR(50) NULL,
    `ExpiryDate` VARCHAR(50) NULL,
    `SendSMSAll` VARCHAR(10) NULL,
    `InputIds` TEXT NULL,
    `FilterType` VARCHAR(50) NULL,
    `IsSubGrpAdmin` VARCHAR(10) NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_ebulletins_GroupId` (`GroupId`),
    CONSTRAINT `FK_ebulletins_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- GALLERY (ALBUMS & PHOTOS)
-- ============================================================

CREATE TABLE `albums` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `Title` VARCHAR(500) NULL,
    `Description` TEXT NULL,
    `Image` VARCHAR(512) NULL,
    `Type` VARCHAR(50) NULL,
    `ModuleId` VARCHAR(100) NULL,
    `ShareType` VARCHAR(50) NULL,
    `CategoryId` VARCHAR(50) NULL,
    `AlbumCategoryText` VARCHAR(255) NULL,
    `OtherCategoryText` VARCHAR(255) NULL,
    `Beneficiary` VARCHAR(255) NULL,
    `CostOfProject` VARCHAR(50) NULL,
    `CostOfProjectType` VARCHAR(50) NULL,
    `WorkingHour` VARCHAR(50) NULL,
    `WorkingHourType` VARCHAR(50) NULL,
    `ProjectDate` VARCHAR(50) NULL,
    `NumberOfRotarian` VARCHAR(50) NULL,
    `MemberIds` TEXT NULL,
    `SubgrpIds` TEXT NULL,
    `IsSubGrpAdmin` VARCHAR(10) NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_albums_GroupId` (`GroupId`),
    CONSTRAINT `FK_albums_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `album_photos` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `AlbumId` INT NOT NULL,
    `Url` VARCHAR(512) NULL,
    `Description` TEXT NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_album_photos_AlbumId` (`AlbumId`),
    CONSTRAINT `FK_album_photos_albums_AlbumId` FOREIGN KEY (`AlbumId`)
        REFERENCES `albums` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- ATTENDANCE
-- ============================================================

CREATE TABLE `attendance_records` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `AttendanceName` VARCHAR(500) NULL,
    `AttendanceDate` VARCHAR(50) NULL,
    `AttendanceTime` VARCHAR(50) NULL,
    `AttendanceDesc` TEXT NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_attendance_records_GroupId` (`GroupId`),
    CONSTRAINT `FK_attendance_records_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `attendance_members` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `AttendanceRecordId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `Type` VARCHAR(10) NULL COMMENT '1=Member, 2=Visitor, 3=Anns, 4=Annets, 5=Rotarian, 6=DistrictDelegate',
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_attendance_members_AttendanceRecordId` (`AttendanceRecordId`),
    INDEX `IX_attendance_members_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_attendance_members_attendance_records_AttendanceRecordId` FOREIGN KEY (`AttendanceRecordId`)
        REFERENCES `attendance_records` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `attendance_visitors` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `AttendanceRecordId` INT NOT NULL,
    `VisitorName` VARCHAR(255) NULL,
    `Type` VARCHAR(10) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_attendance_visitors_AttendanceRecordId` (`AttendanceRecordId`),
    CONSTRAINT `FK_attendance_visitors_attendance_records_AttendanceRecordId` FOREIGN KEY (`AttendanceRecordId`)
        REFERENCES `attendance_records` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- SERVICE DIRECTORY
-- ============================================================

CREATE TABLE `service_directory_entries` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `ServiceCategoryId` INT NULL,
    `MemberName` VARCHAR(255) NULL,
    `Image` VARCHAR(512) NULL,
    `Description` TEXT NULL,
    `ContactNo` VARCHAR(20) NULL,
    `ContactNo2` VARCHAR(20) NULL,
    `CountryCode1` VARCHAR(10) NULL,
    `CountryCode2` VARCHAR(10) NULL,
    `PaxNo` VARCHAR(20) NULL,
    `Email` VARCHAR(255) NULL,
    `Keywords` TEXT NULL,
    `Address` TEXT NULL,
    `City` VARCHAR(255) NULL,
    `State` VARCHAR(255) NULL,
    `Country` VARCHAR(255) NULL,
    `Zip` VARCHAR(20) NULL,
    `Latitude` VARCHAR(50) NULL,
    `Longitude` VARCHAR(50) NULL,
    `Website` VARCHAR(512) NULL,
    `ModuleId` VARCHAR(100) NULL,
    `CreatedBy` INT NOT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_service_directory_entries_GroupId` (`GroupId`),
    INDEX `IX_service_directory_entries_ServiceCategoryId` (`ServiceCategoryId`),
    CONSTRAINT `FK_service_directory_entries_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_service_directory_entries_service_categories_ServiceCategoryId` FOREIGN KEY (`ServiceCategoryId`)
        REFERENCES `service_categories` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ============================================================
-- SETTINGS
-- ============================================================

CREATE TABLE `touchbase_settings` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `UserId` INT NOT NULL,
    `GrpId` VARCHAR(100) NULL,
    `GrpVal` VARCHAR(255) NULL,
    `GrpName` VARCHAR(255) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_touchbase_settings_UserId` (`UserId`),
    CONSTRAINT `FK_touchbase_settings_users_UserId` FOREIGN KEY (`UserId`)
        REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `group_settings` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `ModuleId` VARCHAR(100) NULL,
    `ModVal` VARCHAR(255) NULL,
    `ModName` VARCHAR(255) NULL,
    `IsMob` VARCHAR(10) NULL,
    `IsEmail` VARCHAR(10) NULL,
    `IsPersonal` VARCHAR(10) NULL,
    `IsFamily` VARCHAR(10) NULL,
    `IsBusiness` VARCHAR(10) NULL,
    `ShowMobileSelfClub` VARCHAR(10) NULL,
    `ShowMobileOutsideClub` VARCHAR(10) NULL,
    `ShowEmailSelfClub` VARCHAR(10) NULL,
    `ShowEmailOutsideClub` VARCHAR(10) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_group_settings_GroupId` (`GroupId`),
    INDEX `IX_group_settings_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_group_settings_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- WEB LINKS
-- ============================================================

CREATE TABLE `web_links` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `Title` VARCHAR(500) NULL,
    `FullDesc` TEXT NULL,
    `LinkUrl` VARCHAR(512) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_web_links_GroupId` (`GroupId`),
    CONSTRAINT `FK_web_links_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- PAST PRESIDENTS
-- ============================================================

CREATE TABLE `past_presidents` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `MemberProfileId` INT NULL,
    `MemberName` VARCHAR(255) NULL,
    `PhotoPath` VARCHAR(512) NULL,
    `TenureYear` VARCHAR(50) NULL,
    `Designation` VARCHAR(255) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_past_presidents_GroupId` (`GroupId`),
    INDEX `IX_past_presidents_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_past_presidents_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- BANNERS
-- ============================================================

CREATE TABLE `banners` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `BannerImage` VARCHAR(512) NULL,
    `BannerTitle` VARCHAR(500) NULL,
    `BannerDescription` TEXT NULL,
    `BannerUrl` VARCHAR(512) NULL,
    `BannerType` VARCHAR(50) NULL COMMENT 'banner or slider',
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_banners_GroupId` (`GroupId`),
    CONSTRAINT `FK_banners_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- ZONES & CHAPTERS
-- ============================================================

CREATE TABLE `chapters` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `ZoneId` INT NOT NULL,
    `ChapterName` VARCHAR(255) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_chapters_ZoneId` (`ZoneId`),
    CONSTRAINT `FK_chapters_zones_ZoneId` FOREIGN KEY (`ZoneId`)
        REFERENCES `zones` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- LEADERBOARD
-- ============================================================

CREATE TABLE `leaderboard_entries` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `ZoneId` INT NULL,
    `ClubName` VARCHAR(255) NULL,
    `Points` VARCHAR(50) NULL,
    `Year` VARCHAR(10) NULL,
    `MembersCount` VARCHAR(50) NULL,
    `TrfCount` VARCHAR(50) NULL,
    `TotalProjects` VARCHAR(50) NULL,
    `ProjectCost` VARCHAR(50) NULL,
    `ManHoursCount` VARCHAR(50) NULL,
    `BeneficiaryCount` VARCHAR(50) NULL,
    `RotariansCount` VARCHAR(50) NULL,
    `RotaractorsCount` VARCHAR(50) NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_leaderboard_entries_GroupId` (`GroupId`),
    INDEX `IX_leaderboard_entries_ZoneId` (`ZoneId`),
    CONSTRAINT `FK_leaderboard_entries_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_leaderboard_entries_zones_ZoneId` FOREIGN KEY (`ZoneId`)
        REFERENCES `zones` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ============================================================
-- MER (Monthly Expense Reports / iMélange)
-- ============================================================

CREATE TABLE `mer_items` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `Title` VARCHAR(500) NULL,
    `Link` VARCHAR(512) NULL,
    `FilePath` VARCHAR(512) NULL,
    `PublishDate` VARCHAR(50) NULL,
    `ExpiryDate` VARCHAR(50) NULL,
    `FinanceYear` VARCHAR(20) NULL,
    `TransType` VARCHAR(10) NULL COMMENT '1=MER, 2=iMélange',
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_mer_items_GroupId` (`GroupId`),
    CONSTRAINT `FK_mer_items_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- POPUPS
-- ============================================================

CREATE TABLE `popups` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `Content` TEXT NULL,
    `ImageUrl` VARCHAR(512) NULL,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_popups_GroupId` (`GroupId`),
    CONSTRAINT `FK_popups_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `popup_statuses` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `PopupId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `IsSeen` TINYINT(1) NOT NULL DEFAULT 0,
    `SeenAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_popup_statuses_PopupId` (`PopupId`),
    INDEX `IX_popup_statuses_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_popup_statuses_popups_PopupId` FOREIGN KEY (`PopupId`)
        REFERENCES `popups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- FEEDBACK
-- ============================================================

CREATE TABLE `feedbacks` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `GroupId` INT NOT NULL,
    `MemberProfileId` INT NOT NULL,
    `FeedbackText` TEXT NULL,
    `CreatedAt` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`Id`),
    INDEX `IX_feedbacks_GroupId` (`GroupId`),
    INDEX `IX_feedbacks_MemberProfileId` (`MemberProfileId`),
    CONSTRAINT `FK_feedbacks_groups_GroupId` FOREIGN KEY (`GroupId`)
        REFERENCES `groups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ============================================================
-- END OF SCHEMA
-- ============================================================