using Microsoft.EntityFrameworkCore;
using TouchBase.API.Data;
using TouchBase.API.Models.DTOs.Announcement;
using TouchBase.API.Models.DTOs.Attendance;
using TouchBase.API.Models.DTOs.Celebrations;
using TouchBase.API.Models.DTOs.Common;
using TouchBase.API.Models.DTOs.Dashboard;
using TouchBase.API.Models.DTOs.District;
using TouchBase.API.Models.DTOs.Document;
using TouchBase.API.Models.DTOs.Ebulletin;
using TouchBase.API.Models.DTOs.FindClub;
using TouchBase.API.Models.DTOs.FindRotarian;
using TouchBase.API.Models.DTOs.Gallery;
using TouchBase.API.Models.DTOs.Group;
using TouchBase.API.Models.DTOs.Leaderboard;
using TouchBase.API.Models.DTOs.Member;
using TouchBase.API.Models.DTOs.Mer;
using TouchBase.API.Models.DTOs.PastPresident;
using TouchBase.API.Models.DTOs.ServiceDirectory;
using TouchBase.API.Models.DTOs.Settings;
using TouchBase.API.Models.DTOs.Upload;
using TouchBase.API.Models.DTOs.WebLink;
using TouchBase.API.Models.Entities;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Services;

// ═══════════════════════════════════════════════════════════════
// DashboardService
// ═══════════════════════════════════════════════════════════════
public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;
    public DashboardService(AppDbContext db) => _db = db;

    public async Task<ModuleListResponse> GetGroupModules(ModuleListRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var modules = await _db.GroupModules.Where(m => m.GroupId == grpId && m.IsActive)
            .OrderBy(m => m.ModuleOrderNo)
            .Select(m => new GroupModuleDto { groupModuleId = m.Id.ToString(), groupId = m.GroupId.ToString(), moduleId = m.ModuleId, moduleName = m.ModuleName, moduleStaticRef = m.ModuleStaticRef, image = m.Image, masterProfileID = m.MasterProfileId, isCustomized = m.IsCustomized, moduleOrderNo = m.ModuleOrderNo, notificationCount = m.NotificationCount, modulePriceRs = m.ModulePriceRs, modulePriceUS = m.ModulePriceUS, moduleInfo = m.ModuleInfo })
            .ToListAsync();
        return new ModuleListResponse { status = "0", message = "success", GroupListResult = modules };
    }

    public async Task<DashboardResponse> GetDashboard(DashboardRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var banners = await _db.Banners.Where(b => b.GroupId == grpId && b.IsActive)
            .Select(b => new DashboardBannerDto { bannerId = b.Id.ToString(), bannerImage = b.BannerImage, bannerTitle = b.BannerTitle, bannerDescription = b.BannerDescription, bannerUrl = b.BannerUrl, bannerType = b.BannerType })
            .ToListAsync();
        return new DashboardResponse { status = "0", message = "success", BannerList = banners.Where(b => b.bannerType == "banner").ToList(), SliderList = banners.Where(b => b.bannerType == "slider").ToList() };
    }

    public async Task<NotificationCountResponse> GetNotificationCount(string groupId, string memberProfileId)
    {
        var count = await _db.Notifications.CountAsync(n => n.ReadStatus != "read");
        return new NotificationCountResponse { status = "0", message = "success", notificationCount = count.ToString() };
    }

    public async Task<AdminSubmodulesResponse> GetAdminSubModules(AdminSubmodulesRequest request)
    {
        var grpId = int.TryParse(request.Fk_groupID, out var gid) ? gid : 0;
        var modules = await _db.GroupModules.Where(m => m.GroupId == grpId && m.IsActive)
            .Select(m => new AdminSubmoduleDto { moduleId = m.ModuleId, moduleName = m.ModuleName, moduleStaticRef = m.ModuleStaticRef, image = m.Image, groupId = m.GroupId.ToString(), groupModuleId = m.Id.ToString(), moduleOrderNo = m.ModuleOrderNo, notificationCount = m.NotificationCount })
            .ToListAsync();
        return new AdminSubmodulesResponse { status = "0", message = "success", list = modules };
    }

    public async Task<object> UpdateModuleDashboard(UpdateModuleDashboardRequest request) => await Task.FromResult(new { status = "0", message = "success" });
    public async Task<object> GetAssistanceGov(AssistanceGovRequest request) => await Task.FromResult(new { status = "0", message = "success", ShowHideMonthlyReportModule = "0" });
}

// ═══════════════════════════════════════════════════════════════
// GroupService
// ═══════════════════════════════════════════════════════════════
public class GroupService : IGroupService
{
    private readonly AppDbContext _db;
    public GroupService(AppDbContext db) => _db = db;

    public async Task<CountryCategoryResponse> GetAllCountriesAndCategories()
    {
        var countries = await _db.Countries.Select(c => new CountryDto { countryId = c.Id.ToString(), countryCode = c.CountryCode, countryName = c.CountryName }).ToListAsync();
        var categories = await _db.Categories.Select(c => new CategoryDto { catId = c.Id.ToString(), catName = c.CatName }).ToListAsync();
        return new CountryCategoryResponse { status = "0", message = "success", CountryLists = countries, CategoryLists = categories };
    }

    public async Task<AllGroupsResponse> GetAllGroupsList(AllGroupsRequest request)
    {
        var userId = int.TryParse(request.masterUID, out var uid) ? uid : 0;
        var memberships = await _db.GroupMembers.Include(gm => gm.Group).Where(gm => gm.MemberProfile.UserId == userId && gm.IsActive)
            .Select(gm => new GroupResultDto { grpId = gm.GroupId.ToString(), grpName = gm.Group.GrpName, grpImg = gm.Group.GrpImg, grpProfileId = gm.GrpProfileId, grpProfileid = gm.GrpProfileId, myCategory = gm.MyCategory, isGrpAdmin = gm.IsGrpAdmin, moduleId = gm.ModuleId }).ToListAsync();
        return new AllGroupsResponse { status = "0", message = "success", AllGroupListResults = memberships, PersonalGroupListResults = memberships.Where(g => g.myCategory == "Personal").ToList(), SocialGroupListResults = memberships.Where(g => g.myCategory == "Social").ToList(), BusinessGroupListResults = memberships.Where(g => g.myCategory == "Business").ToList() };
    }

    public async Task<CreateGroupResponse> CreateGroup(CreateGroupRequest request)
    {
        var group = new Group { GrpName = request.grpName, GrpImageId = request.grpImageID, GrpType = request.grpType, GrpCategory = request.grpCategory, Address1 = request.addrss1, Address2 = request.addrss2, City = request.city, State = request.state, Pincode = request.pincode, Country = request.country, Email = request.emailid, Mobile = request.mobile, Website = request.website, Other = request.other };
        _db.Groups.Add(group);
        await _db.SaveChangesAsync();
        return new CreateGroupResponse { status = "0", message = "success", grdId = group.Id.ToString() };
    }

    public async Task<object> CreateSubGroup(CreateSubGroupRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var sg = new SubGroup { GroupId = grpId, SubgrpTitle = request.subGroupTitle, CreatedBy = request.memberProfileId };
        _db.SubGroups.Add(sg); await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<GroupDetailResponse> GetGroupDetail(GroupDetailRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var g = await _db.Groups.FindAsync(grpId);
        if (g == null) return new GroupDetailResponse { status = "1", message = "Not found" };
        return new GroupDetailResponse { status = "0", message = "success", getGroupDetailResult = new List<GroupDetailDto> { new() { grpId = g.Id.ToString(), grpName = g.GrpName, grpImg = g.GrpImg, grpType = g.GrpType, grpCategory = g.GrpCategory, addrss1 = g.Address1, addrss2 = g.Address2, city = g.City, state = g.State, pincode = g.Pincode, country = g.Country, emailid = g.Email, mobile = g.Mobile, website = g.Website, totalMembers = g.TotalMembers.ToString() } } };
    }

    public async Task<SubGroupListResponse> GetSubGroupList(SubGroupRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var sgs = await _db.SubGroups.Include(sg => sg.Members).Where(sg => sg.GroupId == grpId)
            .Select(sg => new SubGroupDto { subgrpId = sg.Id.ToString(), subgrpTitle = sg.SubgrpTitle, noOfmem = sg.Members.Count.ToString() }).ToListAsync();
        return new SubGroupListResponse { status = "0", message = "success", SubGroupResult = sgs };
    }

    public async Task<SubGroupDetailResponse> GetSubGroupDetail(SubGroupDetailRequest request)
    {
        var sgId = int.TryParse(request.subgrpId, out var sid) ? sid : 0;
        var members = await _db.SubGroupMembers.Include(sgm => sgm.MemberProfile).Where(sgm => sgm.SubGroupId == sgId)
            .Select(sgm => new SubGroupMemberDto { profileId = sgm.MemberProfileId.ToString(), memname = sgm.MemberProfile.MemberName, mobile = sgm.MemberProfile.MemberMobile }).ToListAsync();
        return new SubGroupDetailResponse { status = "0", message = "success", SubGroupResult = members };
    }

    public async Task<AddMemberGroupResponse> AddMemberToGroup(AddMemberRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var user = await _db.Users.Include(u => u.MemberProfiles).FirstOrDefaultAsync(u => u.MobileNo == request.mobile);
        if (user == null) return new AddMemberGroupResponse { status = "1", message = "User not found" };
        var profile = user.MemberProfiles.FirstOrDefault();
        if (profile == null) return new AddMemberGroupResponse { status = "1", message = "Profile not found" };

        var exists = await _db.GroupMembers.AnyAsync(gm => gm.GroupId == grpId && gm.MemberProfileId == profile.Id);
        if (!exists) { _db.GroupMembers.Add(new GroupMember { GroupId = grpId, MemberProfileId = profile.Id }); await _db.SaveChangesAsync(); }
        var total = await _db.GroupMembers.CountAsync(gm => gm.GroupId == grpId && gm.IsActive);
        return new AddMemberGroupResponse { status = "0", message = "success", totalMember = total.ToString() };
    }

    public async Task<object> AddMultipleMembersToGroup(AddMultipleMembersRequest request) => await Task.FromResult(new { status = "0", message = "success" });

    public async Task<GlobalSearchResponse> GlobalSearchGroup(GlobalSearchRequest request)
    {
        var userId = int.TryParse(request.otherMemId, out var uid) ? uid : 0;
        var user = await _db.Users.Include(u => u.MemberProfiles).ThenInclude(mp => mp.GroupMemberships).ThenInclude(gm => gm.Group).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return new GlobalSearchResponse { status = "1", message = "Not found" };
        var profile = user.MemberProfiles.FirstOrDefault();
        var groups = profile?.GroupMemberships.Select(gm => new GlobalGroupItemDto { grpId = gm.GroupId.ToString(), grpName = gm.Group.GrpName, grpImg = gm.Group.GrpImg, grpProfileId = gm.GrpProfileId, isMygrp = "1" }).ToList();
        return new GlobalSearchResponse { status = "0", message = "success", membername = user.FirstName, membermobile = user.MobileNo, profilepicpath = user.ProfileImage, AllGlobalGroupListResults = groups };
    }

    public async Task<object> DeleteByModuleName(DeleteByModuleRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> DeleteImage(object request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> UpdateModuleDashboard(UpdateModuleDashboardRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> RemoveGroupCategory(RemoveCategoryRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> UpdateMemberGroupCategory(UpdateCategoryRequest request)
    {
        var profileId = int.TryParse(request.memberProfileId, out var pid) ? pid : 0;
        var gm = await _db.GroupMembers.FirstOrDefaultAsync(g => g.MemberProfileId == profileId && g.MemberMainId == request.memberMainId);
        if (gm != null) { gm.MyCategory = request.mycategory; await _db.SaveChangesAsync(); }
        return new { status = "0", message = "success" };
    }
    public async Task<ModuleListResponse> GetGroupModulesList(ModuleListRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var modules = await _db.GroupModules.Where(m => m.GroupId == grpId && m.IsActive).OrderBy(m => m.ModuleOrderNo)
            .Select(m => new GroupModuleDto { groupModuleId = m.Id.ToString(), groupId = m.GroupId.ToString(), moduleId = m.ModuleId, moduleName = m.ModuleName, moduleStaticRef = m.ModuleStaticRef, image = m.Image, masterProfileID = m.MasterProfileId, isCustomized = m.IsCustomized, moduleOrderNo = m.ModuleOrderNo, notificationCount = m.NotificationCount, modulePriceRs = m.ModulePriceRs, modulePriceUS = m.ModulePriceUS, moduleInfo = m.ModuleInfo }).ToListAsync();
        return new ModuleListResponse { status = "0", message = "success", GroupListResult = modules };
    }
    public async Task<NotificationCountResponse> GetNotificationCount(object request) => new() { status = "0", message = "success", notificationCount = "0" };
    public async Task<object> GetEmail(GetEmailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success", email = "" }; }
    public async Task<DashboardResponse> GetNewDashboard(DashboardRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var banners = await _db.Banners.Where(b => b.GroupId == grpId && b.IsActive).Select(b => new DashboardBannerDto { bannerId = b.Id.ToString(), bannerImage = b.BannerImage, bannerTitle = b.BannerTitle, bannerDescription = b.BannerDescription, bannerUrl = b.BannerUrl, bannerType = b.BannerType }).ToListAsync();
        return new DashboardResponse { status = "0", message = "success", BannerList = banners, SliderList = banners };
    }
    public async Task<object> GetRotaryLibraryData(object request) { await Task.CompletedTask; return new { status = "0", message = "success", data = new List<object>() }; }
    public async Task<AdminSubmodulesResponse> GetAdminSubModules(AdminSubmodulesRequest request)
    {
        var grpId = int.TryParse(request.Fk_groupID, out var gid) ? gid : 0;
        var mods = await _db.GroupModules.Where(m => m.GroupId == grpId).Select(m => new AdminSubmoduleDto { moduleId = m.ModuleId, moduleName = m.ModuleName, moduleStaticRef = m.ModuleStaticRef, image = m.Image, groupId = m.GroupId.ToString(), groupModuleId = m.Id.ToString() }).ToListAsync();
        return new AdminSubmodulesResponse { status = "0", message = "success", list = mods };
    }
    public async Task<EntityInfoResponse> GetEntityInfo(EntityInfoRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var g = await _db.Groups.FindAsync(grpId);
        return new EntityInfoResponse { status = "0", message = "success", groupName = g?.GrpName, groupImg = g?.GrpImg, contactNo = g?.ContactNo, address = g?.Address1, email = g?.Email, EntityInfoResult = new List<EntityInfoItemDto>(), AdminInfoResult = new List<AdminInfoItemDto>() };
    }
    public async Task<object> GetAllGroupListSync(GroupSyncRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetClubDetails(object request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetClubHistory(object request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> SubmitFeedback(FeedbackRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var pid = int.TryParse(request.profileId, out var p) ? p : 0;
        _db.Feedbacks.Add(new Feedback { GroupId = grpId, MemberProfileId = pid, FeedbackText = request.feedback });
        await _db.SaveChangesAsync(); return new { status = "0", message = "success" };
    }
    public async Task<object> GetZoneList(object request)
    {
        var zones = await _db.Zones.Select(z => new TouchBase.API.Models.DTOs.Group.ZoneItemDto { PK_zoneID = z.Id.ToString(), zoneName = z.ZoneName }).ToListAsync();
        return new ZoneListResponse { status = "0", message = "success", zonelistResult = zones };
    }
    public async Task<object> GetMobilePopup(PopupRequest request) { await Task.CompletedTask; return new { status = "0", message = "success", data = new List<object>() }; }
    public async Task<object> UpdateMobilePopupFlag(UpdatePopupRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> UpdateDeviceTokenNumber(DeviceTokenRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetAssistanceGov(AssistanceGovRequest request) { await Task.CompletedTask; return new { status = "0", message = "success", ShowHideMonthlyReportModule = "0" }; }
}

// ═══════════════════════════════════════════════════════════════
// AnnouncementService
// ═══════════════════════════════════════════════════════════════
public class AnnouncementService : IAnnouncementService
{
    private readonly AppDbContext _db;
    public AnnouncementService(AppDbContext db) => _db = db;

    public async Task<AnnouncementListResponse> GetAnnouncementList(AnnouncementListRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var items = await _db.Announcements.Where(a => a.GroupId == grpId).OrderByDescending(a => a.CreatedAt)
            .Select(a => new AnnouncementItemDto { announID = a.Id.ToString(), announTitle = a.AnnounTitle, announceDEsc = a.AnnounDesc, announType = a.AnnounType, announImg = a.AnnounImg, publishDate = a.PublishDate, expiryDate = a.ExpiryDate, filterType = a.FilterType, createDateTime = a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), link = a.Link, grpID = a.GroupId.ToString() }).ToListAsync();
        return new AnnouncementListResponse { status = "0", message = "success", AnnounceList = items };
    }

    public async Task<object> GetAnnouncementDetails(AnnouncementDetailRequest request)
    {
        var id = int.TryParse(request.announID, out var aid) ? aid : 0;
        var a = await _db.Announcements.FindAsync(id);
        if (a == null) return new { status = "1", message = "Not found" };
        return new { status = "0", message = "success", data = new AnnouncementItemDto { announID = a.Id.ToString(), announTitle = a.AnnounTitle, announceDEsc = a.AnnounDesc, announType = a.AnnounType, announImg = a.AnnounImg, publishDate = a.PublishDate, expiryDate = a.ExpiryDate, filterType = a.FilterType, link = a.Link } };
    }

    public async Task<object> AddAnnouncement(AddAnnouncementRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var memId = int.TryParse(request.memID, out var mid) ? mid : 0;
        var ann = new Announcement { GroupId = grpId, AnnounTitle = request.announTitle, AnnounDesc = request.announceDEsc, AnnounType = request.annType, AnnounImg = request.announImg, PublishDate = request.publishDate, ExpiryDate = request.expiryDate, SendSMSNonSmartPh = request.sendSMSNonSmartPh, SendSMSAll = request.sendSMSAll, ModuleId = request.moduleId, RegLink = request.reglink, InputIds = request.inputIDs, RepeatDates = request.AnnouncementRepeatDates, IsSubGrpAdmin = request.isSubGrpAdmin, CreatedBy = memId };
        _db.Announcements.Add(ann); await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
}

// ═══════════════════════════════════════════════════════════════
// DocumentService
// ═══════════════════════════════════════════════════════════════
public class DocumentService : IDocumentService
{
    private readonly AppDbContext _db;
    public DocumentService(AppDbContext db) => _db = db;

    public async Task<DocumentListResponse> GetDocumentList(DocumentListRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var profileId = int.TryParse(request.memberProfileID, out var pid) ? pid : 0;
        var docs = await _db.Documents.Include(d => d.ReadStatuses).Where(d => d.GroupId == grpId).OrderByDescending(d => d.CreatedAt)
            .Select(d => new DocumentItemDto { docID = d.Id.ToString(), docTitle = d.DocTitle, docType = d.DocType, docURL = d.DocURL, createDateTime = d.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), docAccessType = d.DocAccessType, isRead = d.ReadStatuses.Any(rs => rs.MemberProfileId == profileId) ? "Yes" : "No" }).ToListAsync();
        return new DocumentListResponse { status = "0", message = "success", DocumentLsitResult = docs };
    }

    public async Task<object> AddDocument(IFormFile file, string grpID, string profileID, string docTitle)
    {
        var grpId = int.TryParse(grpID, out var gid) ? gid : 0;
        var pid = int.TryParse(profileID, out var p) ? p : 0;
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine("wwwroot", "uploads", "documents"); Directory.CreateDirectory(path);
        await using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
        var doc = new Document { GroupId = grpId, DocTitle = docTitle, DocType = Path.GetExtension(file.FileName), DocURL = $"/uploads/documents/{fileName}", CreatedBy = pid };
        _db.Documents.Add(doc); await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<object> UpdateDocumentIsRead(UpdateDocReadRequest request)
    {
        var docId = int.TryParse(request.DocID, out var did) ? did : 0;
        var pid = int.TryParse(request.memberProfileID, out var p) ? p : 0;
        var exists = await _db.DocumentReadStatuses.AnyAsync(drs => drs.DocumentId == docId && drs.MemberProfileId == pid);
        if (!exists) { _db.DocumentReadStatuses.Add(new DocumentReadStatus { DocumentId = docId, MemberProfileId = pid, IsRead = "Yes" }); await _db.SaveChangesAsync(); }
        return new { status = "0", message = "success" };
    }
}

// ═══════════════════════════════════════════════════════════════
// EbulletinService
// ═══════════════════════════════════════════════════════════════
public class EbulletinService : IEbulletinService
{
    private readonly AppDbContext _db;
    public EbulletinService(AppDbContext db) => _db = db;

    public async Task<EbulletinListResponse> GetYearWiseList(EbulletinListRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var items = await _db.Ebulletins.Where(e => e.GroupId == grpId).OrderByDescending(e => e.CreatedAt)
            .Select(e => new EbulletinItemDto { ebulletinID = e.Id.ToString(), ebulletinlink = e.EbulletinLink, ebulletinType = e.EbulletinType, ebulletinTitle = e.EbulletinTitle, createDateTime = e.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), publishDateTime = e.PublishDate }).ToListAsync();
        return new EbulletinListResponse { status = "0", message = "success", Result = new EbulletinResultWrapper { EbulletinListResult = items } };
    }

    public async Task<object> AddEbulletin(AddEbulletinRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var eb = new Ebulletin { GroupId = grpId, EbulletinTitle = request.ebulletinTitle, EbulletinLink = request.ebulletinlink, EbulletinType = request.ebulletinType, EbulletinFileId = request.ebulletinfileid, PublishDate = request.publishDate, ExpiryDate = request.expiryDate, SendSMSAll = request.sendSMSAll, CreatedBy = int.TryParse(request.memID, out var m) ? m : 0 };
        _db.Ebulletins.Add(eb); await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
}

// ═══════════════════════════════════════════════════════════════
// GalleryService
// ═══════════════════════════════════════════════════════════════
public class GalleryService : IGalleryService
{
    private readonly AppDbContext _db;
    public GalleryService(AppDbContext db) => _db = db;

    public async Task<AlbumListResponse> GetAlbumsList(AlbumListRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var albums = await _db.Albums.Include(a => a.Photos).Where(a => a.GroupId == grpId).OrderByDescending(a => a.CreatedAt)
            .Select(a => new AlbumItemDto { albumId = a.Id.ToString(), Title = a.Title, Description = a.Description, Image = a.Image, GroupID = a.GroupId.ToString(), ModuleID = a.ModuleId, Type = a.Type, beneficiary = a.Beneficiary, cost_of_project = a.CostOfProject, cost_of_project_type = a.CostOfProjectType, working_hour = a.WorkingHour, working_hour_type = a.WorkingHourType, project_date = a.ProjectDate, NumberOfRotarian = a.NumberOfRotarian, sharetype = a.ShareType, albumCategoryID = a.CategoryId, albumCategoryText = a.AlbumCategoryText }).ToListAsync();
        return new AlbumListResponse { status = "0", message = "success", Result = new AlbumListResult { newAlbums = albums, updatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") } };
    }

    public async Task<AlbumListResponse> GetAlbumsListNew(ShowcaseAlbumsRequest request) => await GetAlbumsList(new AlbumListRequest { groupId = request.groupId });

    public async Task<AlbumPhotoListResponse> GetAlbumPhotoList(AlbumPhotoListRequest request)
    {
        var albumId = int.TryParse(request.albumId, out var aid) ? aid : 0;
        var photos = await _db.AlbumPhotos.Where(p => p.AlbumId == albumId)
            .Select(p => new AlbumPhotoDto { photoId = p.Id.ToString(), Url = p.Url, Description = p.Description }).ToListAsync();
        return new AlbumPhotoListResponse { status = "0", message = "success", Result = new AlbumPhotoListResult { newPhotos = photos, updatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") } };
    }

    public async Task<AlbumDetailResponse> GetAlbumDetails(AlbumDetailRequest request)
    {
        var albumId = int.TryParse(request.albumId, out var aid) ? aid : 0;
        var a = await _db.Albums.FindAsync(albumId);
        if (a == null) return new AlbumDetailResponse { status = "1", message = "Not found" };
        return new AlbumDetailResponse { status = "0", message = "success", AlbumDetailResult = new AlbumDetailResultWrapper { AlbumDetail = new List<AlbumDetailDto> { new() { albumTitle = a.Title, albumDescription = a.Description, albumImage = a.Image, type = a.Type, groupId = a.GroupId.ToString(), albumId = a.Id.ToString(), memberIds = a.MemberIds, beneficiary = a.Beneficiary, NumberOfRotarian = a.NumberOfRotarian, projectCost = a.CostOfProject, projectDate = a.ProjectDate, workingHour = a.WorkingHour, albumCategoryID = a.CategoryId, albumCategoryText = a.AlbumCategoryText, otherCategoryText = a.OtherCategoryText, shareType = a.ShareType, costOfProjectType = a.CostOfProjectType, workingHourType = a.WorkingHourType } } } };
    }

    public async Task<CreateAlbumResponse> AddUpdateAlbum(CreateAlbumRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var albumId = int.TryParse(request.albumId, out var aid) ? aid : 0;
        Album album;
        if (albumId > 0) { album = await _db.Albums.FindAsync(albumId) ?? new Album(); } else { album = new Album { GroupId = grpId, CreatedBy = int.TryParse(request.createdBy, out var cb) ? cb : 0 }; _db.Albums.Add(album); }
        album.Title = request.albumTitle; album.Description = request.albumDescription; album.Image = request.albumImage; album.Type = request.type; album.ModuleId = request.moduleId; album.ShareType = request.shareType; album.CategoryId = request.categoryId; album.Beneficiary = request.beneficiary; album.CostOfProject = request.costofproject; album.CostOfProjectType = request.costofprojecttype; album.WorkingHour = request.manhourspent; album.WorkingHourType = request.Manhourspenttype; album.ProjectDate = request.dateofproject; album.NumberOfRotarian = request.NumberofRotarian; album.OtherCategoryText = request.OtherCategorytext; album.MemberIds = request.memberIds; album.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return new CreateAlbumResponse { status = "0", message = "success", galleryid = album.Id.ToString() };
    }

    public async Task<object> AddUpdateAlbumPhoto(IFormFile file, string photoId, string desc, string albumId, string groupId, string createdBy)
    {
        var aid = int.TryParse(albumId, out var a) ? a : 0;
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine("wwwroot", "uploads", "gallery"); Directory.CreateDirectory(path);
        await using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
        _db.AlbumPhotos.Add(new AlbumPhoto { AlbumId = aid, Url = $"/uploads/gallery/{fileName}", Description = desc, CreatedBy = int.TryParse(createdBy, out var cb) ? cb : 0 });
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<DeletePhotoResponse> DeleteAlbumPhoto(DeletePhotoRequest request)
    {
        var photoId = int.TryParse(request.photoId, out var pid) ? pid : 0;
        var photo = await _db.AlbumPhotos.FindAsync(photoId);
        if (photo != null) { _db.AlbumPhotos.Remove(photo); await _db.SaveChangesAsync(); }
        return new DeletePhotoResponse { status = "0", message = "success" };
    }

    public async Task<object> GetYear(TouchBase.API.Models.DTOs.Gallery.YearRequest request) { await Task.CompletedTask; return new { status = "0", message = "success", str = new { Table = new List<object>() } }; }
    public async Task<object> FillYearList(object request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetMerList(TouchBase.API.Models.DTOs.Gallery.MerListRequest request) { await Task.CompletedTask; return new { status = "0", message = "success", Result = new { Table = new List<object>() } }; }
}

// ═══════════════════════════════════════════════════════════════
// AttendanceService
// ═══════════════════════════════════════════════════════════════
public class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _db;
    public AttendanceService(AppDbContext db) => _db = db;

    public async Task<AttendanceListResponse> GetAttendanceListNew(AttendanceListRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        var items = await _db.AttendanceRecords.Include(ar => ar.AttendanceMembers).Include(ar => ar.AttendanceVisitors).Where(ar => ar.GroupId == grpId).OrderByDescending(ar => ar.CreatedAt)
            .Select(ar => new AttendanceItemDto { AttendanceID = ar.Id.ToString(), AttendanceName = ar.AttendanceName, AttendanceDate = ar.AttendanceDate, Attendancetime = ar.AttendanceTime, member_count = ar.AttendanceMembers.Count(m => m.Type == "1").ToString(), visitor_count = ar.AttendanceVisitors.Count.ToString(), Description = ar.AttendanceDesc }).ToListAsync();
        return new AttendanceListResponse { status = "0", message = "success", Result = new AttendanceListResultWrapper { Table = items } };
    }

    public async Task<AttendanceDetailResponse> GetAttendanceDetails(AttendanceDetailRequest request)
    {
        var id = int.TryParse(request.AttendanceID, out var aid) ? aid : 0;
        var ar = await _db.AttendanceRecords.Include(a => a.AttendanceMembers).Include(a => a.AttendanceVisitors).FirstOrDefaultAsync(a => a.Id == id);
        if (ar == null) return new AttendanceDetailResponse { status = "1", message = "Not found" };
        return new AttendanceDetailResponse { status = "0", message = "success", AttendanceDetailsResult = new List<AttendanceDetailDto> { new() { AttendanceID = ar.Id.ToString(), AttendanceName = ar.AttendanceName, AttendanceDate = ar.AttendanceDate, Attendancetime = ar.AttendanceTime, AttendanceDesc = ar.AttendanceDesc, MemberCount = ar.AttendanceMembers.Count(m => m.Type == "1"), VisitorsCount = ar.AttendanceVisitors.Count } } };
    }

    public async Task<object> AttendanceDelete(AttendanceDeleteRequest request)
    {
        var id = int.TryParse(request.AttendanceID, out var aid) ? aid : 0;
        var ar = await _db.AttendanceRecords.FindAsync(id);
        if (ar != null) { _db.AttendanceRecords.Remove(ar); await _db.SaveChangesAsync(); }
        return new { status = "0", message = "success" };
    }

    public async Task<AttendanceMemberResponse> GetAttendanceMemberDetails(AttendanceMemberDetailRequest request)
    {
        var id = int.TryParse(request.AttendanceID, out var aid) ? aid : 0;
        var members = await _db.AttendanceMembers.Include(am => am.MemberProfile).Where(am => am.AttendanceRecordId == id && am.Type == request.type)
            .Select(am => new AttendanceMemberDto { FK_MemberID = am.MemberProfileId.ToString(), MemberName = am.MemberProfile.MemberName, Designation = am.MemberProfile.Designation, image = am.MemberProfile.ProfilePic }).ToListAsync();
        return new AttendanceMemberResponse { status = "0", message = "success", Result = members };
    }

    public async Task<AttendanceVisitorResponse> GetAttendanceVisitorsDetails(AttendanceMemberDetailRequest request)
    {
        var id = int.TryParse(request.AttendanceID, out var aid) ? aid : 0;
        var visitors = await _db.AttendanceVisitors.Where(av => av.AttendanceRecordId == id)
            .Select(av => new AttendanceVisitorDto { PK_AttendanceVisitorID = av.Id.ToString(), FK_AttendanceID = av.AttendanceRecordId.ToString(), VisitorsName = av.VisitorName }).ToListAsync();
        return new AttendanceVisitorResponse { status = "0", message = "success", Result = visitors };
    }

}

// ═══════════════════════════════════════════════════════════════
// CelebrationsService
// ═══════════════════════════════════════════════════════════════
public class CelebrationsService : ICelebrationsService
{
    private readonly AppDbContext _db;
    public CelebrationsService(AppDbContext db) => _db = db;

    public async Task<MonthEventListResponse> GetMonthEventList(MonthEventListRequest request)
    {
        var events = await _db.MemberProfiles.Where(mp => mp.Dob != null || mp.Doa != null)
            .Select(mp => new CelebrationEventDto { eventID = mp.Id.ToString(), title = mp.MemberName, eventDate = mp.Dob, type = mp.Dob != null ? "B" : "A", memberID = mp.Id.ToString() }).Take(100).ToListAsync();
        return new MonthEventListResponse { status = "0", message = "success", Result = new MonthEventResult { Events = events } };
    }
    public async Task<object> GetEventMinDetails(EventMinDetailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<TodaysBirthdayResponse> GetTodaysBirthday(TodaysBirthdayRequest request) { await Task.CompletedTask; return new TodaysBirthdayResponse { status = "0", message = "success", Result = new List<BirthdayItemDto>() }; }
    public async Task<TypeWiseResponse> GetMonthEventListTypeWiseNational(TypeWiseRequest request) { var r = await GetMonthEventList(new MonthEventListRequest { }); return new TypeWiseResponse { status = "0", message = "success", Result = new TypeWiseResult { Events = r.Result?.Events } }; }
    public async Task<MonthEventListResponse> GetMonthEventListDetailsNational(DateWiseRequest request) => await GetMonthEventList(new MonthEventListRequest { });
}

// ═══════════════════════════════════════════════════════════════
// Remaining Simple Services
// ═══════════════════════════════════════════════════════════════
public class ServiceDirectoryService : IServiceDirectoryService
{
    private readonly AppDbContext _db;
    public ServiceDirectoryService(AppDbContext db) => _db = db;
    public async Task<ServiceCategoriesResponse> GetServiceCategoriesData(ServiceCategoriesRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var cats = await _db.ServiceCategories.Include(sc => sc.Entries).Select(sc => new ServiceCategoryDto { CategoryName = sc.CategoryName, ID = sc.Id, TotalCount = sc.Entries.Count(e => e.GroupId == grpId) }).ToListAsync();
        var items = await _db.ServiceDirectoryEntries.Where(s => s.GroupId == grpId).Select(s => new ServiceDirectoryItemDto { serviceDirId = s.Id.ToString(), memberName = s.MemberName, image = s.Image, contactNo = s.ContactNo, description = s.Description, email = s.Email, address = s.Address, city = s.City, state = s.State, latitude = s.Latitude, longitude = s.Longitude, categoryId = s.ServiceCategoryId.ToString(), website = s.Website }).ToListAsync();
        return new ServiceCategoriesResponse { status = "0", message = "success", Result = new ServiceCategoriesResult { Category = cats, DirectoryData = items } };
    }
    public async Task<object> GetServiceDirectoryCategories(object request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetServiceDirectoryDetails(ServiceDetailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> AddServiceDirectory(AddServiceRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetServiceDirectoryListSync(object request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
}

public class SettingsService : ISettingsService
{
    private readonly AppDbContext _db;
    public SettingsService(AppDbContext db) => _db = db;
    public async Task<TouchbaseSettingResponse> GetTouchbaseSetting(TouchbaseSettingRequest request)
    {
        var uid = int.TryParse(request.mainMasterId, out var u) ? u : 0;
        var settings = await _db.TouchbaseSettings.Where(ts => ts.UserId == uid).Select(ts => new SettingItemDto { grpId = ts.GrpId, grpVal = ts.GrpVal, grpName = ts.GrpName }).ToListAsync();
        return new TouchbaseSettingResponse { status = "0", message = "success", TBSettingResult = new TouchbaseSettingResultWrapper { AllTBSettingResults = new TouchbaseSettingResults { TBSettingResults = settings } } };
    }
    public async Task<object> UpdateTouchbaseSetting(UpdateTouchbaseSettingRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<GroupSettingResponse> GetGroupSetting(GroupSettingRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        var settings = await _db.GroupSettings.Where(gs => gs.GroupId == grpId).Select(gs => new GroupSettingItemDto { moduleId = gs.ModuleId, modVal = gs.ModVal, modName = gs.ModName }).ToListAsync();
        return new GroupSettingResponse { status = "0", message = "success", TBGroupSettingResult = new GroupSettingResultWrapper { GRpSettingResult = new GroupSettingResult { GRpSettingDetails = settings } } };
    }
    public async Task<object> UpdateGroupSetting(UpdateGroupSettingRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
}

public class FindClubService : IFindClubService
{
    private readonly AppDbContext _db;
    public FindClubService(AppDbContext db) => _db = db;
    public async Task<ClubSearchResponse> GetClubList(ClubSearchRequest request)
    {
        var query = _db.Clubs.AsQueryable();
        if (!string.IsNullOrEmpty(request.keyword)) query = query.Where(c => c.ClubName!.Contains(request.keyword));
        var clubs = await query.Take(50).Select(c => new TouchBase.API.Models.DTOs.FindClub.ClubItemDto { GroupId = c.GroupId.ToString(), ClubName = c.ClubName, ClubType = c.ClubType, ClubId = c.ClubId, District = c.District, CharterDate = c.CharterDate, MeetingDay = c.MeetingDay, MeetingTime = c.MeetingTime, Website = c.Website }).ToListAsync();
        return new ClubSearchResponse { status = "0", message = "success", ClubResult = clubs };
    }
    public async Task<ClubSearchResponse> GetClubsNearMe(ClubNearMeRequest request) => await GetClubList(new ClubSearchRequest());
    public async Task<ClubDetailResponse> GetClubDetails(ClubDetailRequest request)
    {
        var grpId = int.TryParse(request.grpId, out var gid) ? gid : 0;
        var c = await _db.Clubs.FirstOrDefaultAsync(cl => cl.GroupId == grpId);
        if (c == null) return new ClubDetailResponse { status = "1", message = "Not found" };
        return new ClubDetailResponse { status = "0", message = "success", ClubDetailResult = new ClubDetailDto { clubId = c.ClubId, clubName = c.ClubName, address = c.Address, city = c.City, state = c.State, country = c.Country, meetingDay = c.MeetingDay, meetingTime = c.MeetingTime, clubWebsite = c.Website, lat = c.Lat, longi = c.Longi, presidentName = c.PresidentName, presidentMobile = c.PresidentMobile, presidentEmail = c.PresidentEmail, secretaryName = c.SecretaryName, secretaryMobile = c.SecretaryMobile, secretaryEmail = c.SecretaryEmail, governorName = c.GovernorName, governorMobile = c.GovernorMobile, governorEmail = c.GovernorEmail } };
    }
    public async Task<ClubMembersResponse> GetClubMembers(ClubDetailRequest request) { await Task.CompletedTask; return new ClubMembersResponse { status = "0", message = "success", Result = new List<ClubMemberDto>() }; }
    public async Task<object> GetPublicAlbumsList(ClubDetailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetPublicEventsList(ClubDetailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<object> GetPublicNewsletterList(ClubDetailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<CommitteeListResponse> GetCommitteeList(ClubDetailRequest request) { await Task.CompletedTask; return new CommitteeListResponse { status = "0", message = "success" }; }
}

public class FindRotarianService : IFindRotarianService
{
    private readonly AppDbContext _db;
    public FindRotarianService(AppDbContext db) => _db = db;
    public async Task<ZoneChapterResponse> GetZoneChapterList()
    {
        var zones = await _db.Zones.Select(z => new TouchBase.API.Models.DTOs.FindRotarian.ZoneItemDto { ZoneID = z.Id.ToString(), ZoneName = z.ZoneName }).ToListAsync();
        var chapters = await _db.Chapters.Select(c => new ChapterItemDto { ChapterID = c.Id.ToString(), ChapterName = c.ChapterName, ZoneID = c.ZoneId.ToString() }).ToListAsync();
        return new ZoneChapterResponse { status = "0", message = "success", ZoneChapterResult = new ZoneChapterResult { Table = zones, Table1 = chapters } };
    }
    public async Task<RotarianSearchResponse> GetRotarianList(RotarianSearchRequest request)
    {
        var query = _db.MemberProfiles.AsQueryable();
        if (!string.IsNullOrEmpty(request.name)) query = query.Where(mp => mp.MemberName!.Contains(request.name));
        var items = await query.Take(50).Select(mp => new RotarianItemDto { masterUID = mp.UserId.ToString(), profileID = mp.Id.ToString(), memberMobile = mp.MemberMobile, member_Name = mp.MemberName, mem_Category = mp.Category, Grade = mp.MembershipGrade, pic = mp.ProfilePic }).ToListAsync();
        return new RotarianSearchResponse { status = "0", message = "success", RotarianResult = items };
    }
    public async Task<RotarianDetailResponse> GetRotarianDetails(RotarianDetailRequest request)
    {
        var pid = int.TryParse(request.memberProfileId, out var p) ? p : 0;
        var mp = await _db.MemberProfiles.Include(m => m.Addresses).Include(m => m.User).FirstOrDefaultAsync(m => m.Id == pid);
        if (mp == null) return new RotarianDetailResponse { status = "1", message = "Not found" };
        var addr = mp.Addresses.FirstOrDefault();
        return new RotarianDetailResponse { status = "0", message = "success", Result = new RotarianDetailResult { Table = new List<RotarianDetailDto> { new() { memberName = mp.MemberName, pic = mp.ProfilePic, masterUID = mp.UserId.ToString(), memberMobile = mp.MemberMobile, memberEmail = mp.MemberEmail, bloodGroup = mp.BloodGroup, membershipGrade = mp.MembershipGrade, dob = mp.Dob, doa = mp.Doa, whatsappNum = mp.WhatsappNum, hideWhatsnum = mp.HideWhatsnum, hideMail = mp.HideMail, hideNum = mp.HideNum, companyName = mp.CompanyName, designation = mp.Designation, classification = mp.Classification, address = addr?.Address, city = addr?.City, state = addr?.State, country = addr?.Country, pincode = addr?.Pincode } } } };
    }
    public async Task<CategoryListResponse> GetCategoryList() { await Task.CompletedTask; return new CategoryListResponse { status = "0", message = "success", Result = new List<TouchBase.API.Models.DTOs.FindRotarian.CategoryItemDto>() }; }
    public async Task<GradeListResponse> GetMemberGradeList() { await Task.CompletedTask; return new GradeListResponse { status = "0", message = "success", Result = new List<GradeItemDto>() }; }
    public async Task<ClubListResponse> GetClubList() { await Task.CompletedTask; return new ClubListResponse { status = "0", message = "success", Result = new List<TouchBase.API.Models.DTOs.FindRotarian.ClubItemDto>() }; }
}

public class DistrictService : IDistrictService
{
    private readonly AppDbContext _db;
    public DistrictService(AppDbContext db) => _db = db;
    public async Task<DistrictMemberListResponse> GetDistrictMemberList(DistrictMemberListRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var page = int.TryParse(request.pageNo, out var p) ? p : 1;
        var size = int.TryParse(request.recordCount, out var s) ? s : 25;
        var query = _db.GroupMembers.Include(gm => gm.MemberProfile).Include(gm => gm.Group).Where(gm => gm.GroupId == grpId);
        if (!string.IsNullOrEmpty(request.searchText)) query = query.Where(gm => gm.MemberProfile.MemberName!.Contains(request.searchText));
        var total = await query.CountAsync();
        var members = await query.Skip((page - 1) * size).Take(size)
            .Select(gm => new DistrictMemberDto { profileId = gm.MemberProfileId.ToString(), memberName = gm.MemberProfile.MemberName, memberMobile = gm.MemberProfile.MemberMobile, masterUID = gm.MemberProfile.UserId.ToString(), pic = gm.MemberProfile.ProfilePic, grpID = gm.GroupId.ToString(), club_name = gm.Group.GrpName }).ToListAsync();
        return new DistrictMemberListResponse { status = "0", message = "success", resultCount = total.ToString(), totalPages = ((int)Math.Ceiling(total / (double)size)).ToString(), currentPage = page.ToString(), Result = members };
    }
    public async Task<ClassificationListResponse> GetClassificationList(ClassificationListRequest request) { await Task.CompletedTask; return new ClassificationListResponse { status = "0", message = "success", Result = new List<ClassificationItemDto>() }; }
    public async Task<object> GetMemberByClassification(MemberByClassificationRequest request) { await Task.CompletedTask; return new { status = "0", message = "success", Result = new List<object>() }; }
    public async Task<DistrictClubListResponse> GetClubs(DistrictClubsRequest request) { await Task.CompletedTask; return new DistrictClubListResponse { status = "0", message = "success", Clubs = new List<DistrictClubDto>() }; }
    public async Task<object> GetMemberWithDynamicFields(DistrictMemberDetailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }
    public async Task<DistrictCommitteeResponse> GetDistrictCommittee(DistrictCommitteeRequest request) { await Task.CompletedTask; return new DistrictCommitteeResponse { status = "0", message = "success", Result = new List<DistrictCommitteeMemberDto>() }; }
}

public class LeaderboardService : ILeaderboardService
{
    private readonly AppDbContext _db;
    public LeaderboardService(AppDbContext db) => _db = db;
    public async Task<object> GetZoneList(TouchBase.API.Models.DTOs.Leaderboard.ZoneListRequest request)
    {
        var zones = await _db.Zones.Select(z => new { PK_zoneID = z.Id.ToString(), z.ZoneName }).ToListAsync();
        return new { status = "0", message = "success", zonelistResult = zones };
    }
    public async Task<LeaderboardResponse> GetLeaderBoardDetails(LeaderboardRequest request)
    {
        var entries = await _db.LeaderboardEntries.Where(le => le.Year == request.RowYear).Select(le => new LeaderBoardEntryDto { clubName = le.ClubName, Points = le.Points }).ToListAsync();
        return new LeaderboardResponse { status = "0", message = "success", leaderBoardResult = entries };
    }
}

public class WebLinkService : IWebLinkService
{
    private readonly AppDbContext _db;
    public WebLinkService(AppDbContext db) => _db = db;
    public async Task<WebLinkListResponse> GetWebLinksList(WebLinkListRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        var links = await _db.WebLinks.Where(wl => wl.GroupId == grpId).Select(wl => new WebLinkItemDto { weblinkId = wl.Id.ToString(), groupId = wl.GroupId.ToString(), title = wl.Title, fullDesc = wl.FullDesc, linkUrl = wl.LinkUrl }).ToListAsync();
        return new WebLinkListResponse { status = "0", message = "success", TBGetWebLinkListResult = new WebLinkResultWrapper { WebLinkListResult = links } };
    }
}

public class PastPresidentService : IPastPresidentService
{
    private readonly AppDbContext _db;
    public PastPresidentService(AppDbContext db) => _db = db;
    public async Task<PastPresidentResponse> GetPastPresidentsList(PastPresidentRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        var presidents = await _db.PastPresidents.Where(pp => pp.GroupId == grpId).Select(pp => new PastPresidentDto { PastPresidentId = pp.Id.ToString(), MemberName = pp.MemberName, PhotoPath = pp.PhotoPath, TenureYear = pp.TenureYear, designation = pp.Designation }).ToListAsync();
        return new PastPresidentResponse { status = "0", message = "success", TBPastPresidentList = new PastPresidentListWrapper { newRecords = presidents } };
    }
}

public class MerService : IMerService
{
    private readonly AppDbContext _db;
    public MerService(AppDbContext db) => _db = db;
    public async Task<Models.DTOs.Mer.YearResponse> GetYear(Models.DTOs.Mer.YearRequest request)
    {
        var years = await _db.MerItems.Select(m => m.FinanceYear).Distinct().Select(y => new YearItemDto { FinanceYear = y }).ToListAsync();
        return new Models.DTOs.Mer.YearResponse { status = "0", message = "success", str = new YearResultWrapper { Table = years } };
    }
    public async Task<Models.DTOs.Mer.MerListResponse> GetMerList(Models.DTOs.Mer.MerListRequest request)
    {
        var items = await _db.MerItems.Where(m => m.FinanceYear == request.FinanceYear && m.TransType == request.TransType)
            .Select(m => new MerItemDto { GrpID = m.GroupId.ToString(), MER_ID = m.Id.ToString(), Title = m.Title, Link = m.Link, File_Path = m.FilePath, publish_date = m.PublishDate, expiry_date = m.ExpiryDate, FinanceYear = m.FinanceYear }).ToListAsync();
        return new Models.DTOs.Mer.MerListResponse { status = "0", message = "success", Result = new MerResultWrapper { Table = items } };
    }
}
