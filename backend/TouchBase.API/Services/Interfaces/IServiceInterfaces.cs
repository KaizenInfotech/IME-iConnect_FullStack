using TouchBase.API.Models.DTOs.Announcement;
using TouchBase.API.Models.DTOs.Attendance;
using TouchBase.API.Models.DTOs.Auth;
using TouchBase.API.Models.DTOs.Celebrations;
using TouchBase.API.Models.DTOs.Common;
using TouchBase.API.Models.DTOs.Dashboard;
using TouchBase.API.Models.DTOs.District;
using TouchBase.API.Models.DTOs.Document;
using TouchBase.API.Models.DTOs.Ebulletin;
using TouchBase.API.Models.DTOs.Event;
using TouchBase.API.Models.DTOs.FindClub;
using TouchBase.API.Models.DTOs.FindRotarian;
using TouchBase.API.Models.DTOs.Gallery;
using TouchBase.API.Models.DTOs.Leaderboard;
using TouchBase.API.Models.DTOs.Member;
using TouchBase.API.Models.DTOs.Mer;
using TouchBase.API.Models.DTOs.PastPresident;
using TouchBase.API.Models.DTOs.ServiceDirectory;
using TouchBase.API.Models.DTOs.Settings;
using TouchBase.API.Models.DTOs.Upload;
using TouchBase.API.Models.DTOs.WebLink;
using TouchBase.API.Models.DTOs.Group;
using Microsoft.AspNetCore.Http;

namespace TouchBase.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> RequestOtp(LoginRequest request);
    Task<LoginResponse> VerifyOtp(OtpVerifyRequest request);
    Task<object> GetWelcomeGroups(WelcomeScreenRequest request);
    Task<object> GetMemberDetails(MemberDetailsRequest request);
    Task<object> Register(RegistrationRequest request);
}

public interface IDashboardService
{
    Task<ModuleListResponse> GetGroupModules(ModuleListRequest request);
    Task<DashboardResponse> GetDashboard(DashboardRequest request);
    Task<NotificationCountResponse> GetNotificationCount(string groupId, string memberProfileId);
    Task<AdminSubmodulesResponse> GetAdminSubModules(AdminSubmodulesRequest request);
    Task<object> UpdateModuleDashboard(UpdateModuleDashboardRequest request);
    Task<object> GetAssistanceGov(AssistanceGovRequest request);
}

public interface IGroupService
{
    Task<CountryCategoryResponse> GetAllCountriesAndCategories();
    Task<AllGroupsResponse> GetAllGroupsList(AllGroupsRequest request);
    Task<CreateGroupResponse> CreateGroup(CreateGroupRequest request);
    Task<object> CreateSubGroup(CreateSubGroupRequest request);
    Task<GroupDetailResponse> GetGroupDetail(GroupDetailRequest request);
    Task<SubGroupListResponse> GetSubGroupList(SubGroupRequest request);
    Task<SubGroupDetailResponse> GetSubGroupDetail(SubGroupDetailRequest request);
    Task<AddMemberGroupResponse> AddMemberToGroup(AddMemberRequest request);
    Task<object> AddMultipleMembersToGroup(AddMultipleMembersRequest request);
    Task<GlobalSearchResponse> GlobalSearchGroup(GlobalSearchRequest request);
    Task<object> DeleteByModuleName(DeleteByModuleRequest request);
    Task<object> DeleteImage(object request);
    Task<object> UpdateModuleDashboard(UpdateModuleDashboardRequest request);
    Task<object> RemoveGroupCategory(RemoveCategoryRequest request);
    Task<object> UpdateMemberGroupCategory(UpdateCategoryRequest request);
    Task<ModuleListResponse> GetGroupModulesList(ModuleListRequest request);
    Task<NotificationCountResponse> GetNotificationCount(object request);
    Task<object> GetEmail(GetEmailRequest request);
    Task<DashboardResponse> GetNewDashboard(DashboardRequest request);
    Task<object> GetRotaryLibraryData(object request);
    Task<AdminSubmodulesResponse> GetAdminSubModules(AdminSubmodulesRequest request);
    Task<EntityInfoResponse> GetEntityInfo(EntityInfoRequest request);
    Task<object> GetAllGroupListSync(GroupSyncRequest request);
    Task<object> GetClubDetails(object request);
    Task<object> GetClubHistory(object request);
    Task<object> SubmitFeedback(FeedbackRequest request);
    Task<object> GetZoneList(object request);
    Task<object> GetMobilePopup(PopupRequest request);
    Task<object> UpdateMobilePopupFlag(UpdatePopupRequest request);
    Task<object> UpdateDeviceTokenNumber(DeviceTokenRequest request);
    Task<object> GetAssistanceGov(AssistanceGovRequest request);
}

public interface IMemberService
{
    Task<DirectoryListResponse> GetDirectoryList(DirectoryListRequest request);
    Task<MemberDetailResponse> GetMember(MemberDetailRequest request);
    Task<UpdateResponse> UpdateProfile(UpdateProfileRequest request);
    Task<object> GetMemberListSync(MemberSyncRequest request);
    Task<UpdateResponse> UpdateProfileDetails(UpdatePersonalDetailsRequest request);
    Task<UpdateResponse> UpdateAddressDetails(UpdateAddressRequest request);
    Task<UpdateResponse> UpdateFamilyDetails(UpdateFamilyRequest request);
    Task<MemberDetailResponse> GetUpdatedProfileDetails(GetUpdatedProfileRequest request);
    Task<object> UploadProfilePhoto(IFormFile file, ProfilePhotoRequest request);
    Task<BodListResponse> GetBodList(BodListRequest request);
    Task<GoverningCouncilResponse> GetGoverningCouncil(GoverningCouncilRequest request);
    Task<UpdateResponse> UpdateMember(UpdateMemberRequest request);
    Task<UpdateResponse> UpdateProfilePersonalDetails(UpdatePersonalDetailsRequest request);
    Task<object> SaveProfile(SaveProfileRequest request);
}

public interface IEventService
{
    Task<EventListResponse> GetEventList(EventListRequest request);
    Task<EventDetailResponse> GetEventDetails(EventDetailRequest request);
    Task<object> AddEvent(AddEventRequest request);
    Task<AnswerEventResponse> AnswerEvent(AnswerEventRequest request);
    Task<SmsCountResponse> GetSmsCountDetails(SmsCountRequest request);
}

public interface IAnnouncementService
{
    Task<AnnouncementListResponse> GetAnnouncementList(AnnouncementListRequest request);
    Task<object> GetAnnouncementDetails(AnnouncementDetailRequest request);
    Task<object> AddAnnouncement(AddAnnouncementRequest request);
}

public interface IDocumentService
{
    Task<DocumentListResponse> GetDocumentList(DocumentListRequest request);
    Task<object> AddDocument(IFormFile file, string grpID, string profileID, string docTitle);
    Task<object> UpdateDocumentIsRead(UpdateDocReadRequest request);
}

public interface IEbulletinService
{
    Task<EbulletinListResponse> GetYearWiseList(EbulletinListRequest request);
    Task<object> AddEbulletin(AddEbulletinRequest request);
}

public interface IGalleryService
{
    Task<AlbumListResponse> GetAlbumsList(AlbumListRequest request);
    Task<AlbumListResponse> GetAlbumsListNew(ShowcaseAlbumsRequest request);
    Task<AlbumPhotoListResponse> GetAlbumPhotoList(AlbumPhotoListRequest request);
    Task<AlbumDetailResponse> GetAlbumDetails(AlbumDetailRequest request);
    Task<CreateAlbumResponse> AddUpdateAlbum(CreateAlbumRequest request);
    Task<object> AddUpdateAlbumPhoto(IFormFile file, string photoId, string desc, string albumId, string groupId, string createdBy);
    Task<DeletePhotoResponse> DeleteAlbumPhoto(DeletePhotoRequest request);
    Task<object> GetYear(TouchBase.API.Models.DTOs.Gallery.YearRequest request);
    Task<object> FillYearList(object request);
    Task<object> GetMerList(TouchBase.API.Models.DTOs.Gallery.MerListRequest request);
}

public interface IAttendanceService
{
    Task<AttendanceListResponse> GetAttendanceListNew(AttendanceListRequest request);
    Task<AttendanceDetailResponse> GetAttendanceDetails(AttendanceDetailRequest request);
    Task<object> AttendanceDelete(AttendanceDeleteRequest request);
    Task<AttendanceMemberResponse> GetAttendanceMemberDetails(AttendanceMemberDetailRequest request);
    Task<AttendanceVisitorResponse> GetAttendanceVisitorsDetails(AttendanceMemberDetailRequest request);
}

public interface ICelebrationsService
{
    Task<MonthEventListResponse> GetMonthEventList(MonthEventListRequest request);
    Task<object> GetEventMinDetails(EventMinDetailRequest request);
    Task<TodaysBirthdayResponse> GetTodaysBirthday(TodaysBirthdayRequest request);
    Task<TypeWiseResponse> GetMonthEventListTypeWiseNational(TypeWiseRequest request);
    Task<MonthEventListResponse> GetMonthEventListDetailsNational(DateWiseRequest request);
}

public interface IServiceDirectoryService
{
    Task<ServiceCategoriesResponse> GetServiceCategoriesData(ServiceCategoriesRequest request);
    Task<object> GetServiceDirectoryCategories(object request);
    Task<object> GetServiceDirectoryDetails(ServiceDetailRequest request);
    Task<object> AddServiceDirectory(AddServiceRequest request);
    Task<object> GetServiceDirectoryListSync(object request);
}

public interface ISettingsService
{
    Task<TouchbaseSettingResponse> GetTouchbaseSetting(TouchbaseSettingRequest request);
    Task<object> UpdateTouchbaseSetting(UpdateTouchbaseSettingRequest request);
    Task<GroupSettingResponse> GetGroupSetting(GroupSettingRequest request);
    Task<object> UpdateGroupSetting(UpdateGroupSettingRequest request);
}

public interface IFindClubService
{
    Task<ClubSearchResponse> GetClubList(ClubSearchRequest request);
    Task<ClubSearchResponse> GetClubsNearMe(ClubNearMeRequest request);
    Task<ClubDetailResponse> GetClubDetails(ClubDetailRequest request);
    Task<ClubMembersResponse> GetClubMembers(ClubDetailRequest request);
    Task<object> GetPublicAlbumsList(ClubDetailRequest request);
    Task<object> GetPublicEventsList(ClubDetailRequest request);
    Task<object> GetPublicNewsletterList(ClubDetailRequest request);
    Task<CommitteeListResponse> GetCommitteeList(ClubDetailRequest request);
}

public interface IFindRotarianService
{
    Task<ZoneChapterResponse> GetZoneChapterList();
    Task<RotarianSearchResponse> GetRotarianList(RotarianSearchRequest request);
    Task<RotarianDetailResponse> GetRotarianDetails(RotarianDetailRequest request);
    Task<CategoryListResponse> GetCategoryList();
    Task<GradeListResponse> GetMemberGradeList();
    Task<ClubListResponse> GetClubList();
}

public interface IDistrictService
{
    Task<DistrictMemberListResponse> GetDistrictMemberList(DistrictMemberListRequest request);
    Task<ClassificationListResponse> GetClassificationList(ClassificationListRequest request);
    Task<object> GetMemberByClassification(MemberByClassificationRequest request);
    Task<DistrictClubListResponse> GetClubs(DistrictClubsRequest request);
    Task<object> GetMemberWithDynamicFields(DistrictMemberDetailRequest request);
    Task<DistrictCommitteeResponse> GetDistrictCommittee(DistrictCommitteeRequest request);
}

public interface ILeaderboardService
{
    Task<object> GetZoneList(TouchBase.API.Models.DTOs.Leaderboard.ZoneListRequest request);
    Task<LeaderboardResponse> GetLeaderBoardDetails(LeaderboardRequest request);
}

public interface IWebLinkService
{
    Task<WebLinkListResponse> GetWebLinksList(WebLinkListRequest request);
}

public interface IPastPresidentService
{
    Task<PastPresidentResponse> GetPastPresidentsList(PastPresidentRequest request);
}

public interface IMerService
{
    Task<Models.DTOs.Mer.YearResponse> GetYear(Models.DTOs.Mer.YearRequest request);
    Task<Models.DTOs.Mer.MerListResponse> GetMerList(Models.DTOs.Mer.MerListRequest request);
}
