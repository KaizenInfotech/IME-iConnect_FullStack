using Microsoft.EntityFrameworkCore;
using TouchBase.API.Controllers;
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
// ══════════════════════════════════════════════════════════════���
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
        var profileId = int.TryParse(memberProfileId, out var pid) ? pid : 0;
        var profile = await _db.MemberProfiles.FirstOrDefaultAsync(mp => mp.Id == profileId);
        var userId = profile?.UserId ?? 0;

        var count = userId > 0
            ? await _db.Notifications.CountAsync(n => n.UserId == userId && n.ReadStatus != "Read")
            : 0;
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
    public GroupService(AppDbContext db, IHttpClientFactory httpClientFactory) { _db = db; }

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
        var existingId = int.TryParse(request.grpId, out var eid) ? eid : 0;
        Group group;
        if (existingId > 0)
        {
            group = await _db.Groups.FindAsync(existingId) ?? new Group();
            if (group.Id == 0) { _db.Groups.Add(group); }
        }
        else
        {
            group = new Group();
            _db.Groups.Add(group);
        }
        if (request.grpName != null) group.GrpName = request.grpName;
        if (request.grpImageID != null) group.GrpImageId = request.grpImageID;
        if (request.grpType != null) group.GrpType = request.grpType;
        if (request.grpCategory != null) group.GrpCategory = request.grpCategory;
        if (request.addrss1 != null) group.Address1 = request.addrss1;
        if (request.addrss2 != null) group.Address2 = request.addrss2;
        if (request.city != null) group.City = request.city;
        if (request.state != null) group.State = request.state;
        if (request.pincode != null) group.Pincode = request.pincode;
        if (request.country != null) group.Country = request.country;
        if (request.emailid != null) group.Email = request.emailid;
        if (request.mobile != null) group.Mobile = request.mobile;
        if (request.website != null) group.Website = request.website;
        if (request.other != null) group.Other = request.other;
        group.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // Also sync the Clubs table so the list reflects the updated name
        var club = await _db.Clubs.FirstOrDefaultAsync(c => c.GroupId == group.Id);
        if (club == null)
        {
            club = new Club { GroupId = group.Id };
            _db.Clubs.Add(club);
        }
        if (request.grpName != null) club.ClubName = request.grpName;
        if (request.addrss1 != null) club.Address = request.addrss1;
        if (request.city != null) club.City = request.city;
        if (request.state != null) club.State = request.state;
        if (request.country != null) club.Country = request.country;
        if (request.other != null) club.MeetingDay = request.other;
        if (request.website != null) club.Website = request.website;
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
    public async Task<object> GetNotificationCount(object request)
    {
        string groupId = "", memberProfileId = "";
        if (request is System.Text.Json.JsonElement je && je.ValueKind == System.Text.Json.JsonValueKind.Object)
        {
            foreach (var prop in je.EnumerateObject())
            {
                if (prop.Name == "groupId") groupId = prop.Value.ToString();
                if (prop.Name == "memberProfileId") memberProfileId = prop.Value.ToString();
            }
        }
        var pid = int.TryParse(memberProfileId, out var p) ? p : 0;
        var profile = await _db.MemberProfiles.FirstOrDefaultAsync(mp => mp.Id == pid);
        var userId = profile?.UserId ?? 0;
        var count = userId > 0 ? await _db.Notifications.CountAsync(n => n.UserId == userId && n.ReadStatus != "Read") : 0;
        return new { status = "0", message = "success", notificationCount = count.ToString() };
    }
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
    public async Task<object> GetAllGroupListSync(GroupSyncRequest request)
    {
        var uid = int.TryParse(request.masterUID, out var u) ? u : 0;

        // Session expiry check: compare request imeiNo with stored ImeiNo.
        // When another device logs in (OTP verify), it overwrites User.ImeiNo.
        // If the requesting device's imeiNo doesn't match, the session is expired.
        if (!string.IsNullOrEmpty(request.imeiNo))
        {
            var user = await _db.Users.FirstOrDefaultAsync(usr => usr.Id == uid);
            if (user != null && !string.IsNullOrEmpty(user.ImeiNo)
                && user.ImeiNo != request.imeiNo)
            {
                return new
                {
                    status = "2", version = "2.3",
                    message = "Your session has been expired. Please login again."
                };
            }
        }

        var groups = await _db.GroupMembers.Include(gm => gm.Group)
            .Where(gm => gm.MemberMainId == u.ToString())
            .Select(gm => new { grpId = gm.GroupId, grpName = gm.Group.GrpName, grpImg = gm.Group.GrpImg, grpType = gm.Group.GrpType, grpProfileId = gm.MemberProfileId, myCategory = gm.MyCategory, isGrpAdmin = gm.IsGrpAdmin })
            .ToListAsync();
        var modules = await _db.GroupModules.Where(m => m.IsActive)
            .Select(m => new { m.Id, m.GroupId, m.ModuleId, m.ModuleName, m.ModuleStaticRef, m.Image, m.ModuleOrderNo })
            .ToListAsync();
        return new
        {
            status = "0", version = "2.3", message = "Success",
            updatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Result = new
            {
                GroupList = new { NewGroupList = groups, UpdatedGroupList = new List<object>(), DeletedGroupList = new List<object>() },
                ModuleList = new { NewModuleList = modules, UpdatedModuleList = new List<object>(), DeletedModuleList = new List<object>() }
            }
        };
    }
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
    public async Task<object> UpdateDeviceTokenNumber(DeviceTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.MobileNumber) || string.IsNullOrEmpty(request.DeviceToken))
            return new { status = "1", message = "Missing mobile number or device token" };

        var user = await _db.Users.FirstOrDefaultAsync(u => u.MobileNo == request.MobileNumber);
        if (user == null)
            return new { status = "1", message = "User not found" };

        // Update or insert device token
        var existing = await _db.DeviceTokens.FirstOrDefaultAsync(dt => dt.UserId == user.Id && dt.Token == request.DeviceToken);
        if (existing != null)
        {
            existing.Platform = request.Platform ?? existing.Platform;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _db.DeviceTokens.Add(new Models.Entities.DeviceToken
            {
                UserId = user.Id,
                Token = request.DeviceToken,
                Platform = request.Platform ?? "android",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Also update user's device token field
        user.DeviceToken = request.DeviceToken;
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // Enforce single-device: any other tokens for this user belong to a
        // previously logged-in phone whose session was already invalidated via
        // ImeiNo check. Remove them so pushes only fire on the current device.
        var stale = await _db.DeviceTokens
            .Where(dt => dt.UserId == user.Id && dt.Token != request.DeviceToken)
            .ToListAsync();
        if (stale.Count > 0)
        {
            _db.DeviceTokens.RemoveRange(stale);
            await _db.SaveChangesAsync();
        }

        return new { status = "0", message = "success" };
    }
    public async Task<object> GetAssistanceGov(AssistanceGovRequest request) { await Task.CompletedTask; return new { status = "0", message = "success", ShowHideMonthlyReportModule = "0" }; }

    public async Task<object> DeleteGroup(string groupId)
    {
        var gid = int.TryParse(groupId, out var g) ? g : 0;
        var deleted = false;

        // Soft delete from group_master (production table)
        try
        {
            using var conn = new MySqlConnector.MySqlConnection("server=101.53.148.126;database=imei_new;user=admin_mysql_db;password=o27AvGxQQGTBEfrlpD7G1;AllowZeroDateTime=True;ConvertZeroDateTime=True");
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE group_master SET isdeleted=1 WHERE pk_group_master_id=@id";
            cmd.Parameters.AddWithValue("@id", gid);
            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows > 0) deleted = true;
        }
        catch { }

        // Delete from Clubs table (listing table)
        var club = await _db.Clubs.FirstOrDefaultAsync(c => c.GroupId == gid);
        if (club != null)
        {
            _db.Clubs.Remove(club);
            deleted = true;
        }

        // Cascade delete all data belonging to this group
        await CascadeDeleteGroupData(gid);

        // Delete the group record itself
        var group = await _db.Groups.FindAsync(gid);
        if (group != null)
        {
            _db.Groups.Remove(group);
            deleted = true;
        }

        await _db.SaveChangesAsync();

        return deleted
            ? new { status = "0", message = "success" }
            : new { status = "1", message = "Group not found" };
    }

    /// Cascade delete all data belonging to a group.
    private async Task CascadeDeleteGroupData(int groupId)
    {
        // Child tables first (foreign key dependencies)
        // album_photos → albums
        var albumIds = await _db.Albums.Where(a => a.GroupId == groupId).Select(a => a.Id).ToListAsync();
        if (albumIds.Count > 0)
        {
            var albumPhotos = await _db.AlbumPhotos.Where(p => albumIds.Contains(p.AlbumId)).ToListAsync();
            _db.AlbumPhotos.RemoveRange(albumPhotos);
        }

        // event child tables → events
        var eventIds = await _db.Events.Where(e => e.GroupId == groupId).Select(e => e.Id).ToListAsync();
        if (eventIds.Count > 0)
        {
            var eventResponses = await _db.EventResponses.Where(r => eventIds.Contains(r.EventId)).ToListAsync();
            _db.EventResponses.RemoveRange(eventResponses);
        }

        // sub_group_members → sub_groups
        var subGroupIds = await _db.SubGroups.Where(sg => sg.GroupId == groupId).Select(sg => sg.Id).ToListAsync();
        if (subGroupIds.Count > 0)
        {
            var sgMembers = await _db.SubGroupMembers.Where(m => subGroupIds.Contains(m.SubGroupId)).ToListAsync();
            _db.SubGroupMembers.RemoveRange(sgMembers);
        }

        // attendance child tables → attendance_records
        var attendanceIds = await _db.AttendanceRecords.Where(a => a.GroupId == groupId).Select(a => a.Id).ToListAsync();
        if (attendanceIds.Count > 0)
        {
            var attMembers = await _db.AttendanceMembers.Where(m => attendanceIds.Contains(m.AttendanceRecordId)).ToListAsync();
            _db.AttendanceMembers.RemoveRange(attMembers);
            var attVisitors = await _db.AttendanceVisitors.Where(v => attendanceIds.Contains(v.AttendanceRecordId)).ToListAsync();
            _db.AttendanceVisitors.RemoveRange(attVisitors);
        }

        // document_read_statuses → documents
        var docIds = await _db.Documents.Where(d => d.GroupId == groupId).Select(d => d.Id).ToListAsync();
        if (docIds.Count > 0)
        {
            var docStatuses = await _db.DocumentReadStatuses.Where(s => docIds.Contains(s.DocumentId)).ToListAsync();
            _db.DocumentReadStatuses.RemoveRange(docStatuses);
        }

        // Collect member profile IDs belonging to this group before removing GroupMembers
        var groupMemberRecords = await _db.GroupMembers.Where(gm => gm.GroupId == groupId).ToListAsync();
        var memberProfileIds = groupMemberRecords.Select(gm => gm.MemberProfileId).Distinct().ToList();

        // Now delete parent tables
        _db.Albums.RemoveRange(await _db.Albums.Where(a => a.GroupId == groupId).ToListAsync());
        _db.Announcements.RemoveRange(await _db.Announcements.Where(a => a.GroupId == groupId).ToListAsync());
        _db.AttendanceRecords.RemoveRange(await _db.AttendanceRecords.Where(a => a.GroupId == groupId).ToListAsync());
        _db.Banners.RemoveRange(await _db.Banners.Where(b => b.GroupId == groupId).ToListAsync());
        _db.Documents.RemoveRange(await _db.Documents.Where(d => d.GroupId == groupId).ToListAsync());
        _db.Ebulletins.RemoveRange(await _db.Ebulletins.Where(e => e.GroupId == groupId).ToListAsync());
        _db.Events.RemoveRange(await _db.Events.Where(e => e.GroupId == groupId).ToListAsync());
        _db.Feedbacks.RemoveRange(await _db.Feedbacks.Where(f => f.GroupId == groupId).ToListAsync());
        _db.GroupMembers.RemoveRange(groupMemberRecords);
        _db.GroupModules.RemoveRange(await _db.GroupModules.Where(gm => gm.GroupId == groupId).ToListAsync());
        _db.GroupSettings.RemoveRange(await _db.GroupSettings.Where(gs => gs.GroupId == groupId).ToListAsync());
        _db.LeaderboardEntries.RemoveRange(await _db.LeaderboardEntries.Where(l => l.GroupId == groupId).ToListAsync());
        _db.MerItems.RemoveRange(await _db.MerItems.Where(m => m.GroupId == groupId).ToListAsync());
        _db.PastPresidents.RemoveRange(await _db.PastPresidents.Where(p => p.GroupId == groupId).ToListAsync());
        _db.Popups.RemoveRange(await _db.Popups.Where(p => p.GroupId == groupId).ToListAsync());
        _db.ServiceDirectoryEntries.RemoveRange(await _db.ServiceDirectoryEntries.Where(s => s.GroupId == groupId).ToListAsync());
        _db.SubGroups.RemoveRange(await _db.SubGroups.Where(sg => sg.GroupId == groupId).ToListAsync());
        _db.WebLinks.RemoveRange(await _db.WebLinks.Where(w => w.GroupId == groupId).ToListAsync());

        // Save first so GroupMembers are removed before checking for orphaned profiles
        await _db.SaveChangesAsync();

        // Delete member profiles and users who no longer belong to any group
        foreach (var profileId in memberProfileIds)
        {
            // Check if this member still belongs to any other group
            var stillInOtherGroup = await _db.GroupMembers.AnyAsync(gm => gm.MemberProfileId == profileId);
            if (stillInOtherGroup) continue;

            var profile = await _db.MemberProfiles.FindAsync(profileId);
            if (profile == null) continue;

            var userId = profile.UserId;

            // Remove family members
            var family = await _db.FamilyMembers.Where(fm => fm.MemberProfileId == profileId).ToListAsync();
            _db.FamilyMembers.RemoveRange(family);

            // Remove addresses
            var addresses = await _db.AddressDetails.Where(a => a.MemberProfileId == profileId).ToListAsync();
            _db.AddressDetails.RemoveRange(addresses);

            // Remove profile
            _db.MemberProfiles.Remove(profile);
            await _db.SaveChangesAsync();

            // If the user has no remaining profiles, delete the user record entirely
            var remainingProfiles = await _db.MemberProfiles.AnyAsync(mp => mp.UserId == userId);
            if (!remainingProfiles)
            {
                var user = await _db.Users.FindAsync(userId);
                if (user != null)
                    _db.Users.Remove(user);
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task<object> DeleteSubGroup(string subGroupId)
    {
        var sgid = int.TryParse(subGroupId, out var s) ? s : 0;
        var sg = await _db.SubGroups.FindAsync(sgid);
        if (sg == null) return new { status = "1", message = "Sub group not found" };
        var members = await _db.SubGroupMembers.Where(m => m.SubGroupId == sgid).ToListAsync();
        _db.SubGroupMembers.RemoveRange(members);
        _db.SubGroups.Remove(sg);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
}

// ═══════════════════════════════════════════════════════════════
// AnnouncementService
// ═════════════════════════��═════════════════════════════════════
public class AnnouncementService : IAnnouncementService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notificationService;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AnnouncementService> _logger;
    public AnnouncementService(AppDbContext db, IHttpClientFactory httpClientFactory, INotificationService notificationService, IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<AnnouncementService> logger) { _db = db; _notificationService = notificationService; _configuration = configuration; _scopeFactory = scopeFactory; _logger = logger; }

    public async Task<object> GetAnnouncementList(AnnouncementListRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var announcements = await _db.Announcements.Where(a => a.GroupId == grpId)
            .OrderByDescending(a => a.Id)
            .Select(a => new
            {
                AnnounceList = new
                {
                    announID = a.Id.ToString(),
                    announTitle = a.AnnounTitle,
                    announceDEsc = a.AnnounDesc,
                    announImg = a.AnnounImg,
                    link = a.Link,
                    filterType = a.FilterType,
                    createDateTime = a.CreatedAt.ToString("dd MMM yyyy hh:mm tt"),
                    publishDateTime = a.PublishDate,
                    expiryDateTime = a.ExpiryDate,
                    createDate = (string?)null,
                    publishDate = (string?)null,
                    expiryDate = (string?)null,
                    isAdmin = "No",
                    isRead = "No",
                    type = (string?)null,
                    reglink = a.RegLink,
                    clubName = "",
                    SMSCount = "0"
                }
            }).ToListAsync();
        return new { TBAnnounceListResult = new { status = "0", message = "success", AnnounListResult = announcements } };
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
        var annId = int.TryParse(request.announID, out var aid) ? aid : 0;

        Announcement ann;
        if (annId > 0)
        {
            ann = await _db.Announcements.FindAsync(annId) ?? new Announcement { GroupId = grpId, CreatedBy = memId };
            if (ann.Id == 0) _db.Announcements.Add(ann);
        }
        else
        {
            ann = new Announcement { GroupId = grpId, CreatedBy = memId };
            _db.Announcements.Add(ann);
        }

        ann.AnnounTitle = request.announTitle; ann.AnnounDesc = request.announceDEsc;
        ann.AnnounType = request.annType;
        // Convert base64 image to file if needed
        var imgValue = request.announImg;
        if (!string.IsNullOrEmpty(imgValue) && imgValue.StartsWith("data:image"))
        {
            var base64Data = imgValue.Substring(imgValue.IndexOf(",") + 1);
            var ext = imgValue.Contains("png") ? ".png" : ".jpg";
            var fileName = $"{Guid.NewGuid()}{ext}";
            var dir = Path.Combine("wwwroot", "uploads", "announcement"); Directory.CreateDirectory(dir);
            await File.WriteAllBytesAsync(Path.Combine(dir, fileName), Convert.FromBase64String(base64Data));
            var appBaseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? "";
            imgValue = $"{appBaseUrl}/uploads/announcement/{fileName}";
        }
        ann.AnnounImg = imgValue;
        ann.PublishDate = request.publishDate; ann.ExpiryDate = request.expiryDate;
        ann.SendSMSNonSmartPh = request.sendSMSNonSmartPh; ann.SendSMSAll = request.sendSMSAll;
        ann.ModuleId = request.moduleId; ann.RegLink = request.reglink;
        ann.InputIds = request.inputIDs; ann.RepeatDates = request.AnnouncementRepeatDates;
        ann.IsSubGrpAdmin = request.isSubGrpAdmin;

        // Determine if notification should be sent now or scheduled for publishDate
        var publishTime = ScheduledNotificationHelper.ParseDate(request.publishDate);
        var sendNow = publishTime == null || publishTime <= DateTime.Now;
        ann.NotificationSent = sendNow; // false = background service will send at publishDate
        await _db.SaveChangesAsync();

        // Send push notification immediately only if publishDate is now or in the past
        // Otherwise, ScheduledNotificationService will send it at the publish time
        if (annId == 0 && grpId > 0 && sendNow)
        {
            var isChapter = await _db.Clubs.AnyAsync(c => c.GroupId == grpId);
            var scopeFactory = _scopeFactory;
            var logger = _logger;
            var newId = ann.Id;
            var imgUrl = ann.AnnounImg;
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var notif = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    var extra = new Dictionary<string, string>
                    {
                        ["announId"] = newId.ToString(),
                        ["ann_title"] = request.announTitle ?? "",
                        ["Ann_date"] = request.publishDate ?? "",
                        ["ann_desc"] = request.announceDEsc ?? "",
                        ["ann_lnk"] = request.reglink ?? "",
                        ["ann_img"] = imgUrl ?? "",
                        ["grpID"] = grpId.ToString()
                    };
                    if (!isChapter)
                        await notif.SendAllUsersNotification(
                            "ann", request.announTitle ?? "New Announcement",
                            request.announceDEsc ?? "A new announcement has been posted", extra);
                    else
                        await notif.SendGroupNotification(
                            grpId, "ann", request.announTitle ?? "New Announcement",
                            request.announceDEsc ?? "A new announcement has been posted", extra);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send announcement notification, announId={Id}, grpId={GrpId}", newId, grpId);
                }
            });
        }

        return new { status = "0", message = "success" };
    }

    public async Task<object> DeleteAnnouncement(string announId)
    {
        var id = int.TryParse(announId, out var aid) ? aid : 0;
        var ann = await _db.Announcements.FindAsync(id);
        if (ann == null) return new { status = "1", message = "Not found" };
        _db.Announcements.Remove(ann);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
}

// ═══════════════════════════════════════════════════════════════
// DocumentService
// ═══════════════════════════════════════════════════════════════
public class DocumentService : IDocumentService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notificationService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DocumentService> _logger;
    public DocumentService(AppDbContext db, INotificationService notificationService, IServiceScopeFactory scopeFactory, ILogger<DocumentService> logger) { _db = db; _notificationService = notificationService; _scopeFactory = scopeFactory; _logger = logger; }

    public async Task<DocumentListResponse> GetDocumentList(DocumentListRequest request)
    {
        var grpId = int.TryParse(request.grpID, out var gid) ? gid : 0;
        var profileId = int.TryParse(request.memberProfileID, out var pid) ? pid : 0;
        var docs = await _db.Documents.Include(d => d.ReadStatuses).Where(d => d.GroupId == grpId)
            .OrderBy(d => d.DisplayOrder).ThenByDescending(d => d.CreatedAt)
            .Select(d => new DocumentItemDto { docID = d.Id.ToString(), docTitle = d.DocTitle, docType = d.DocType, docURL = d.DocURL, createDateTime = d.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), docAccessType = d.DocAccessType, isRead = d.ReadStatuses.Any(rs => rs.MemberProfileId == profileId) ? "Yes" : "No", displayOrder = d.DisplayOrder }).ToListAsync();
        return new DocumentListResponse { status = "0", message = "success", DocumentLsitResult = docs };
    }

    public async Task<object> ReorderDocuments(List<ReorderDocumentItem> items)
    {
        if (items == null || items.Count == 0) return new { status = "1", message = "No items" };
        var ids = items.Select(i => i.DocID).ToList();
        var docs = await _db.Documents.Where(d => ids.Contains(d.Id)).ToListAsync();
        var orderById = items.ToDictionary(i => i.DocID, i => i.DisplayOrder);
        foreach (var d in docs)
        {
            if (orderById.TryGetValue(d.Id, out var order))
            {
                d.DisplayOrder = order;
                d.UpdatedAt = DateTime.UtcNow;
            }
        }
        await _db.SaveChangesAsync();
        return new { status = "0", message = "Reorder saved successfully" };
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

        // Send push notification for new document
        if (grpId > 0)
        {
            var scopeFactory = _scopeFactory;
            var logger = _logger;
            var docId = doc.Id;
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var notif = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    await notif.SendGroupNotification(
                        grpId, "Document",
                        docTitle ?? "New Document",
                        $"A new document '{docTitle}' has been shared",
                        new Dictionary<string, string> { ["docId"] = docId.ToString() });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send document notification, docId={DocId}, grpId={GrpId}", docId, grpId);
                }
            });
        }

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
    private readonly INotificationService _notificationService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EbulletinService> _logger;
    public EbulletinService(AppDbContext db, INotificationService notificationService, IServiceScopeFactory scopeFactory, ILogger<EbulletinService> logger) { _db = db; _notificationService = notificationService; _scopeFactory = scopeFactory; _logger = logger; }

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

        // Send push notification for new newsletter
        if (grpId > 0)
        {
            var scopeFactory = _scopeFactory;
            var logger = _logger;
            var ebId = eb.Id;
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var notif = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    await notif.SendGroupNotification(
                        grpId, "Ebulletin",
                        request.ebulletinTitle ?? "New Newsletter",
                        $"A new newsletter '{request.ebulletinTitle}' has been published",
                        new Dictionary<string, string> { ["ebulletinId"] = ebId.ToString() });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send ebulletin notification, ebulletinId={Id}, grpId={GrpId}", ebId, grpId);
                }
            });
        }

        return new { status = "0", message = "success" };
    }
}

// ═══════════════════════════════════════════════════════════════
// GalleryService
// ═══════════════════════════════════════════════════════════════
public class GalleryService : IGalleryService
{
    private readonly AppDbContext _db;
    public GalleryService(AppDbContext db, IHttpClientFactory httpClientFactory) { _db = db; }

    public async Task<AlbumListResponse> GetAlbumsList(AlbumListRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var albums = await _db.Albums.Include(a => a.Photos).Where(a => a.GroupId == grpId).OrderByDescending(a => a.CreatedAt)
            .Select(a => new AlbumItemDto { albumId = a.Id.ToString(), Title = a.Title, Description = a.Description, Image = a.Image, GroupID = a.GroupId.ToString(), ModuleID = a.ModuleId, Type = a.Type, beneficiary = a.Beneficiary, cost_of_project = a.CostOfProject, cost_of_project_type = a.CostOfProjectType, working_hour = a.WorkingHour, working_hour_type = a.WorkingHourType, project_date = a.ProjectDate, NumberOfRotarian = a.NumberOfRotarian, sharetype = a.ShareType, albumCategoryID = a.CategoryId, albumCategoryText = a.AlbumCategoryText }).ToListAsync();
        return new AlbumListResponse { status = "0", message = "success", Result = new AlbumListResult { newAlbums = albums, updatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") } };
    }

    public async Task<object> GetAlbumsListNew(ShowcaseAlbumsRequest request)
    {
        var grpId = int.TryParse(request.groupId, out var gid) ? gid : 0;
        var query = _db.Albums.Where(a => a.GroupId == grpId);
        if (!string.IsNullOrEmpty(request.year)) query = query.Where(a => a.ProjectDate != null && a.ProjectDate.Contains(request.year));
        if (!string.IsNullOrEmpty(request.searchText)) query = query.Where(a => a.Title != null && a.Title.Contains(request.searchText));
        var memberCount = await _db.GroupMembers.CountAsync(gm => gm.GroupId == grpId);
        var albums = await query.OrderByDescending(a => a.Id)
            .Select(a => new { albumId = a.Id.ToString(), title = a.Title, description = a.Description, image = a.Image, groupId = a.GroupId.ToString(), moduleId = a.ModuleId ?? "8", clubname = "", project_date = a.ProjectDate, sharetype = a.ShareType ?? "0", Attendance = "0", AttendancePer = "0", MeetingType = "0", AgendaDocID = "", MOMDocID = "" }).ToListAsync();
        return new { TBAlbumsListResult = new { status = "0", message = "success", updatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), MemberCount = memberCount.ToString(), Result = new { MemberCount = memberCount.ToString(), newAlbums = albums } } };
    }

    public async Task<object> GetAlbumPhotoList(AlbumPhotoListRequest request)
    {
        var albumId = int.TryParse(request.albumId, out var aid) ? aid : 0;
        var photos = await _db.AlbumPhotos.Where(p => p.AlbumId == albumId)
            .Select(p => new { photoID = p.Id.ToString(), url = p.Url, description = p.Description, albumId = p.AlbumId.ToString() }).ToListAsync();
        return new { TBAlbumPhotoResult = new { status = "0", message = "success", Result = new { newPhotos = photos } } };
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

    public async Task<object> GetYear(TouchBase.API.Models.DTOs.Gallery.YearRequest request)
    {
        var transType = request.Type?.ToString() ?? "1";
        var years = await _db.MerItems.Where(m => m.TransType == transType)
            .Select(m => m.FinanceYear).Distinct().OrderByDescending(y => y)
            .Select(y => new { FinanceYear = y }).ToListAsync();
        return new { status = "0", message = "success", str = new { Table = years } };
    }
    public async Task<object> FillYearList(object request)
    {
        string grpId = "";
        if (request is System.Text.Json.JsonElement je && je.ValueKind == System.Text.Json.JsonValueKind.Object)
        {
            foreach (var prop in je.EnumerateObject())
                if (prop.Name == "grpID" || prop.Name == "groupId") grpId = prop.Value.ToString();
        }
        var gid = int.TryParse(grpId, out var g) ? g : 0;
        var years = await _db.Albums.Where(a => a.GroupId == gid && a.ProjectDate != null)
            .Select(a => a.ProjectDate!.Substring(a.ProjectDate.Length - 4))
            .Distinct().OrderByDescending(y => y)
            .Select(y => new { Yearlist = y }).ToListAsync();
        return new { status = "0", message = "success", Result = new { Table = years } };
    }
    public async Task<object> GetMerList(TouchBase.API.Models.DTOs.Gallery.MerListRequest request)
    {
        var items = await _db.MerItems.Where(m => m.FinanceYear == request.FinanceYear && m.TransType == (request.TransType ?? "1"))
            .Select(m => new { GrpID = m.GroupId, MER_ID = m.Id, Title = m.Title, Link = m.Link, File_Path = m.FilePath, publish_date = m.PublishDate, expiry_date = m.ExpiryDate, FinanceYear = m.FinanceYear }).ToListAsync();
        return new { TBMERListResult = new { status = "0", message = "success", MERListResult = items } };
    }

    public async Task<object> AddMer(AddMerRequest request)
    {
        var gid = int.TryParse(request.GroupId, out var g) ? g : 31185;
        var mer = new MerItem { GroupId = gid, Title = request.Title, Link = request.Link, FilePath = request.FilePath, PublishDate = request.PublishDate, ExpiryDate = request.ExpiryDate, FinanceYear = request.FinanceYear, TransType = request.TransType ?? "1" };
        _db.MerItems.Add(mer);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<object> UpdateMer(UpdateMerRequest request)
    {
        var id = int.TryParse(request.MER_ID, out var mid) ? mid : 0;
        var mer = await _db.MerItems.FindAsync(id);
        if (mer == null) return new { status = "1", message = "Not found" };
        if (request.Title != null) mer.Title = request.Title;
        if (request.Link != null) mer.Link = request.Link;
        if (request.FilePath != null) mer.FilePath = request.FilePath;
        if (request.PublishDate != null) mer.PublishDate = request.PublishDate;
        if (request.ExpiryDate != null) mer.ExpiryDate = request.ExpiryDate;
        if (request.FinanceYear != null) mer.FinanceYear = request.FinanceYear;
        if (request.TransType != null) mer.TransType = request.TransType;
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<object> DeleteMer(string merId)
    {
        var id = int.TryParse(merId, out var mid) ? mid : 0;
        var mer = await _db.MerItems.FindAsync(id);
        if (mer == null) return new { status = "1", message = "Not found" };
        _db.MerItems.Remove(mer);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
}

// ═══════════════════════════════════════════════════════════════
// AttendanceService
// ═══════════════════════════════════════════════════════════════
public class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _db;
    public AttendanceService(AppDbContext db, IHttpClientFactory httpClientFactory) { _db = db; }

    public async Task<object> GetAttendanceListNew(AttendanceListRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        try
        {
            var records = new List<object>();
            using var conn = new MySqlConnector.MySqlConnection(ProdAttConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            // Exclude rows auto-created by the Past Events admin page —
            // those carry FK_eventID and are stored only so Event/GetEventList
            // / Event/GetEventDetails can pull the typed Attendance number
            // via JOIN. They don't represent real "Events Attendance"
            // records (no member/visitor detail rows behind them), so the
            // Flutter Events Attendance listing must skip them.
            // Pull every count column the legacy table tracks so the
            // Meeting Wise Excel report can show all the breakdowns.
            cmd.CommandText = @"SELECT attendance_id as AttendanceID, name as AttendanceName, AttendanceDate,
                                       '' as Attendancetime,
                                       COALESCE(MembersCount,0) as member_count,
                                       COALESCE(VisitorsCount,0) as visitor_count,
                                       COALESCE(AnnsCount,0) as anns_count,
                                       COALESCE(AnnetsCount,0) as annets_count,
                                       COALESCE(RotariansCount,0) as rotarians_count,
                                       COALESCE(DistrictDelegatesCount,0) as district_delegates_count,
                                       COALESCE(AttendanceDesc,'') as Description
                                FROM attentance_master
                                WHERE fk_group_id=@grp AND (isdeleted=0 OR isdeleted IS NULL)
                                  AND (FK_eventID IS NULL OR FK_eventID = 0)
                                ORDER BY attendance_id DESC";
            cmd.Parameters.AddWithValue("@grp", grpId);
            using var reader = await cmd.ExecuteReaderAsync();
            int dateOrdinal = reader.GetOrdinal("AttendanceDate");
            while (await reader.ReadAsync())
            {
                // Format datetime as stable ISO 8601 — see note in GetAttendanceDetails.
                string? attDate = reader.IsDBNull(dateOrdinal)
                    ? null
                    : reader.GetDateTime(dateOrdinal).ToString("yyyy-MM-ddTHH:mm:ss");
                records.Add(new {
                    AttendanceID = reader["AttendanceID"],
                    AttendanceName = reader["AttendanceName"]?.ToString(),
                    AttendanceDate = attDate,
                    Attendancetime = reader["Attendancetime"]?.ToString(),
                    member_count = reader["member_count"],
                    visitor_count = reader["visitor_count"],
                    anns_count = reader["anns_count"],
                    annets_count = reader["annets_count"],
                    rotarians_count = reader["rotarians_count"],
                    district_delegates_count = reader["district_delegates_count"],
                    Description = reader["Description"]?.ToString()
                });
            }
            if (records.Count > 0)
                return new { TBAttendanceListResult = new { status = "0", message = "success", Result = new { Table = records } } };
        }
        catch { }

        // Fallback to EF
        var localRecords = await _db.AttendanceRecords.Where(a => a.GroupId == grpId)
            .OrderByDescending(a => a.Id)
            .Select(a => new { AttendanceID = a.Id, AttendanceName = a.AttendanceName, AttendanceDate = a.AttendanceDate, Attendancetime = a.AttendanceTime, member_count = a.MemberCount ?? 0, visitor_count = a.VisitorCount ?? 0, Description = a.AttendanceDesc }).ToListAsync();
        return new { TBAttendanceListResult = new { status = "0", message = "success", Result = new { Table = localRecords } } };
    }

    public async Task<AttendanceDetailResponse> GetAttendanceDetails(AttendanceDetailRequest request)
    {
        var id = int.TryParse(request.AttendanceID, out var aid) ? aid : 0;
        // Read from production DB first
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdAttConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT attendance_id, name, AttendanceDate, AttendanceDesc, MembersCount, VisitorsCount FROM attentance_master WHERE attendance_id = @id AND (isdeleted=0 OR isdeleted IS NULL)";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // AttendanceDate column is a MySQL `datetime`. We must format
                // it as a stable ISO 8601 string here — calling `.ToString()`
                // on a DateTime uses CurrentCulture, which produces formats
                // like "4/15/2026 4:46:00 PM" that the frontend can't parse,
                // leaving the date input blank.
                int dateOrdinal = reader.GetOrdinal("AttendanceDate");
                string? attendanceDate = null;
                if (!reader.IsDBNull(dateOrdinal))
                    attendanceDate = reader.GetDateTime(dateOrdinal).ToString("yyyy-MM-ddTHH:mm:ss");

                return new AttendanceDetailResponse
                {
                    status = "0", message = "success",
                    AttendanceDetailsResult = new List<AttendanceDetailDto> { new() {
                        AttendanceID = reader["attendance_id"]?.ToString(),
                        AttendanceName = reader["name"]?.ToString(),
                        AttendanceDate = attendanceDate,
                        AttendanceDesc = reader["AttendanceDesc"]?.ToString(),
                        MemberCount = reader["MembersCount"] as int? ?? 0,
                        VisitorsCount = reader["VisitorsCount"] as int? ?? 0,
                    }}
                };
            }
        }
        catch { }

        // Fallback to local DB
        var ar = await _db.AttendanceRecords.FirstOrDefaultAsync(a => a.Id == id);
        if (ar == null) return new AttendanceDetailResponse { status = "1", message = "Not found" };
        var memberCount = await _db.AttendanceMembers.CountAsync(m => m.AttendanceRecordId == id);
        var visitorCount = await _db.AttendanceVisitors.CountAsync(v => v.AttendanceRecordId == id);
        return new AttendanceDetailResponse { status = "0", message = "success", AttendanceDetailsResult = new List<AttendanceDetailDto> { new() { AttendanceID = ar.Id.ToString(), AttendanceName = ar.AttendanceName, AttendanceDate = ar.AttendanceDate, Attendancetime = ar.AttendanceTime, AttendanceDesc = ar.AttendanceDesc, MemberCount = memberCount, VisitorsCount = visitorCount } } };
    }

    public async Task<object> AttendanceDelete(AttendanceDeleteRequest request)
    {
        var id = int.TryParse(request.AttendanceID, out var aid) ? aid : 0;
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdAttConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE attentance_master SET isdeleted=1, deletion_date=NOW() WHERE attendance_id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();
            return new { status = "0", message = "success" };
        }
        catch
        {
            var ar = await _db.AttendanceRecords.FindAsync(id);
            if (ar != null) { _db.AttendanceRecords.Remove(ar); await _db.SaveChangesAsync(); }
            return new { status = "0", message = "success" };
        }
    }

    public async Task<object> AttendanceAddEdit(AttendanceAddEditRequest request)
    {
        var attId = int.TryParse(request.AttendanceID, out var aid) ? aid : 0;
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        var memberCount = request.Members?.Count ?? 0;
        var visitorCount = request.Visitors?.Count(v => !string.IsNullOrEmpty(v.VisitorName)) ?? 0;

        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdAttConnStr);
            await conn.OpenAsync();

            // Insert or update attentance_master
            if (attId > 0)
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE attentance_master SET name=@name, AttendanceDesc=@desc, AttendanceDate=@date, MembersCount=@mc, VisitorsCount=@vc, modification_date=NOW() WHERE attendance_id=@id";
                cmd.Parameters.AddWithValue("@id", attId);
                cmd.Parameters.AddWithValue("@name", request.AttendanceName ?? "");
                cmd.Parameters.AddWithValue("@desc", request.AttendanceDesc ?? "");
                cmd.Parameters.AddWithValue("@date", string.IsNullOrEmpty(request.AttendanceDate) ? DBNull.Value : (object)DateTime.Parse(request.AttendanceDate));
                cmd.Parameters.AddWithValue("@mc", memberCount);
                cmd.Parameters.AddWithValue("@vc", visitorCount);
                await cmd.ExecuteNonQueryAsync();
            }
            else
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO attentance_master (fk_group_id, name, AttendanceDesc, AttendanceDate, MembersCount, VisitorsCount, creation_date, isdeleted) VALUES (@grp, @name, @desc, @date, @mc, @vc, NOW(), 0)";
                cmd.Parameters.AddWithValue("@grp", grpId);
                cmd.Parameters.AddWithValue("@name", request.AttendanceName ?? "");
                cmd.Parameters.AddWithValue("@desc", request.AttendanceDesc ?? "");
                cmd.Parameters.AddWithValue("@date", string.IsNullOrEmpty(request.AttendanceDate) ? DBNull.Value : (object)DateTime.Parse(request.AttendanceDate));
                cmd.Parameters.AddWithValue("@mc", memberCount);
                cmd.Parameters.AddWithValue("@vc", visitorCount);
                await cmd.ExecuteNonQueryAsync();
                using var idCmd = conn.CreateCommand();
                idCmd.CommandText = "SELECT LAST_INSERT_ID()";
                attId = Convert.ToInt32(await idCmd.ExecuteScalarAsync());
            }

            // Update members
            if (request.Members != null)
            {
                using var delCmd = conn.CreateCommand();
                delCmd.CommandText = "UPDATE tbl_attendancememberdetails SET Isdeleted=1, deleted_date=NOW() WHERE FK_AttendanceID=@aid AND (Isdeleted=0 OR Isdeleted IS NULL)";
                delCmd.Parameters.AddWithValue("@aid", attId);
                await delCmd.ExecuteNonQueryAsync();
                foreach (var m in request.Members)
                {
                    var mpId = int.TryParse(m.MemberProfileId ?? m.Id, out var mp) ? mp : 0;
                    if (mpId > 0)
                    {
                        using var insCmd = conn.CreateCommand();
                        insCmd.CommandText = "INSERT INTO tbl_attendancememberdetails (FK_AttendanceID, FK_MemberID) VALUES (@aid, @mid)";
                        insCmd.Parameters.AddWithValue("@aid", attId);
                        insCmd.Parameters.AddWithValue("@mid", mpId);
                        await insCmd.ExecuteNonQueryAsync();
                    }
                }
            }

            // Update visitors
            if (request.Visitors != null)
            {
                using var delCmd = conn.CreateCommand();
                delCmd.CommandText = "UPDATE tbl_attendancevisitorsdetails SET isdeleted=1, deleted_date=NOW() WHERE FK_AttendanceID=@aid AND (isdeleted=0 OR isdeleted IS NULL)";
                delCmd.Parameters.AddWithValue("@aid", attId);
                await delCmd.ExecuteNonQueryAsync();
                foreach (var v in request.Visitors)
                {
                    if (!string.IsNullOrEmpty(v.VisitorName))
                    {
                        using var insCmd = conn.CreateCommand();
                        insCmd.CommandText = "INSERT INTO tbl_attendancevisitorsdetails (FK_AttendanceID, VisitorsName, Rotarian_whohas_Brought, created_date) VALUES (@aid, @name, @inv, NOW())";
                        insCmd.Parameters.AddWithValue("@aid", attId);
                        insCmd.Parameters.AddWithValue("@name", v.VisitorName);
                        insCmd.Parameters.AddWithValue("@inv", v.InvitedBy ?? "");
                        await insCmd.ExecuteNonQueryAsync();
                    }
                }
            }

            return new { status = "0", message = "success" };
        }
        catch (Exception ex) { return new { status = "1", message = ex.Message }; }
    }

    public async Task<object> GetAttendanceMemberDetails(AttendanceMemberDetailRequest request)
    {
        var aid = int.TryParse(request.AttendanceID, out var id) ? id : 0;
        // Read from production-style tables in imei_new
        try
        {
            var members = new List<object>();
            using var conn = new MySqlConnector.MySqlConnection(ProdAttConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT a.FK_MemberID, TRIM(CONCAT(COALESCE(m.Member_name,''),' ',COALESCE(m.last_name,''))) as MemberName,
                COALESCE(m.member_profile_photo_path,'') as image
                FROM tbl_attendancememberdetails a
                LEFT JOIN member_master_profile m ON a.FK_MemberID = m.pk_member_master_profile_id
                WHERE a.FK_AttendanceID = @aid AND (a.isdeleted=0 OR a.isdeleted IS NULL)";
            cmd.Parameters.AddWithValue("@aid", aid);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                members.Add(new { FK_MemberID = reader["FK_MemberID"], MemberName = reader["MemberName"]?.ToString()?.Trim(), Designation = "", image = reader["image"]?.ToString() });
            }
            if (members.Count > 0)
                return new { TBAttendanceMemberDetailsResult = new { status = "0", message = "success", AttendanceMemberResult = members } };
        }
        catch { }

        // Fallback to EF
        var localMembers = await _db.AttendanceMembers.Where(am => am.AttendanceRecordId == aid)
            .Join(_db.MemberProfiles, am => am.MemberProfileId, mp => mp.Id, (am, mp) => new { FK_MemberID = mp.Id, MemberName = mp.MemberName, Designation = mp.Designation ?? "", image = mp.ProfilePic })
            .ToListAsync();
        return new { TBAttendanceMemberDetailsResult = new { status = "0", message = "success", AttendanceMemberResult = localMembers } };
    }

    private const string ProdAttConnStr = "server=101.53.148.126;database=imei_new;user=admin_mysql_db;password=o27AvGxQQGTBEfrlpD7G1;AllowZeroDateTime=True;ConvertZeroDateTime=True;Allow User Variables=true";

    public async Task<object> GetAttendanceVisitorsDetails(AttendanceMemberDetailRequest request)
    {
        var aid = int.TryParse(request.AttendanceID, out var id) ? id : 0;
        // Read from production DB (tbl_attendancevisitorsdetails) - same as production web app
        var visitors = new List<object>();
        try
        {
            using var conn = new MySqlConnector.MySqlConnection(ProdAttConnStr);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT PK_AttendanceVisitorID, FK_AttendanceID, VisitorsName, COALESCE(Rotarian_whohas_Brought,'') as Rotarian_whohas_Brought FROM tbl_attendancevisitorsdetails WHERE FK_AttendanceID = @aid AND (isdeleted=0 OR isdeleted IS NULL)";
            cmd.Parameters.AddWithValue("@aid", aid);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                visitors.Add(new
                {
                    PK_AttendanceVisitorID = reader["PK_AttendanceVisitorID"]?.ToString(),
                    FK_AttendanceID = reader["FK_AttendanceID"]?.ToString(),
                    VisitorsName = reader["VisitorsName"]?.ToString(),
                    Member_whohas_Brought = reader["Rotarian_whohas_Brought"]?.ToString() ?? ""
                });
            }
            if (visitors.Count > 0)
                return new { TBAttendanceVisitorsDetailsResult = new { status = "0", message = "success", AttendanceVisitorsResult = visitors } };
        }
        catch { }

        // Fallback to local DB
        var localVisitors = await _db.AttendanceVisitors.Where(av => av.AttendanceRecordId == aid)
            .Select(av => new { PK_AttendanceVisitorID = av.Id.ToString(), FK_AttendanceID = av.AttendanceRecordId.ToString(), VisitorsName = av.VisitorName, Member_whohas_Brought = "" }).ToListAsync();
        return new { TBAttendanceVisitorsDetailsResult = new { status = "0", message = "success", AttendanceVisitorsResult = localVisitors } };
    }
}

// ═══════════════════════════════════════════════════════════════
// CelebrationsService
// ═══════════════════════════════════════════════════════════════
public class CelebrationsService : ICelebrationsService
{
    private readonly AppDbContext _db;

    public CelebrationsService(AppDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
    }

    public async Task<MonthEventListResponse> GetMonthEventList(MonthEventListRequest request)
    {
        var events = await _db.MemberProfiles.Where(mp => mp.Dob != null || mp.Doa != null)
            .Select(mp => new CelebrationEventDto { eventID = mp.Id.ToString(), title = mp.MemberName, eventDate = mp.Dob, type = mp.Dob != null ? "B" : "A", memberID = mp.Id.ToString() }).Take(100).ToListAsync();
        return new MonthEventListResponse { status = "0", message = "success", Result = new MonthEventResult { Events = events } };
    }
    public async Task<object> GetEventMinDetails(EventMinDetailRequest request) { await Task.CompletedTask; return new { status = "0", message = "success" }; }

    public async Task<object> GetTodaysBirthday(TodaysBirthdayRequest request)
    {
        var grpId = int.TryParse(request.groupID, out var gid) ? gid : 0;
        var today = DateTime.Now;
        // Dob stored as "yyyy-MM-dd" string. Match by suffix "-MM-dd"
        var dateSuffix = today.ToString("-MM-dd");

        var birthdays = await _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Join(_db.Users, x => x.mp.UserId, u => u.Id, (x, u) => new { x.mp, x.gm, u })
            .Where(x => x.gm.GroupId == grpId && x.gm.IsActive && x.mp.Dob != null && x.mp.Dob.Contains(dateSuffix))
            .Select(x => new { profileId = x.mp.Id.ToString(), groupID = x.gm.GroupId.ToString(), memberName = x.u.FirstName ?? x.mp.MemberName, memberMobile = (x.mp.CountryCode != null ? "+" + x.mp.CountryCode + " " : "") + x.mp.MemberMobile, memberEmail = x.mp.MemberEmail ?? "", relation = "", msg = "BirthDay", hide_whatsnum = x.mp.HideWhatsnum ?? "0", hide_num = x.mp.HideNum ?? "0", hide_mail = x.mp.HideMail ?? "0" }).ToListAsync();

        var anniversaries = await _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Join(_db.Users, x => x.mp.UserId, u => u.Id, (x, u) => new { x.mp, x.gm, u })
            .Where(x => x.gm.GroupId == grpId && x.gm.IsActive && x.mp.Doa != null && x.mp.Doa.Contains(dateSuffix))
            .Select(x => new { profileId = x.mp.Id.ToString(), groupID = x.gm.GroupId.ToString(), memberName = x.u.FirstName ?? x.mp.MemberName, memberMobile = (x.mp.CountryCode != null ? "+" + x.mp.CountryCode + " " : "") + x.mp.MemberMobile, memberEmail = x.mp.MemberEmail ?? "", relation = "", msg = "Anniversary", hide_whatsnum = x.mp.HideWhatsnum ?? "0", hide_num = x.mp.HideNum ?? "0", hide_mail = x.mp.HideMail ?? "0" }).ToListAsync();

        // Family birthdays
        var familyBdays = await _db.FamilyMembers
            .Join(_db.GroupMembers, fm => fm.MemberProfileId, gm => gm.MemberProfileId, (fm, gm) => new { fm, gm })
            .Where(x => x.gm.GroupId == grpId && x.gm.IsActive && x.fm.Dob != null && x.fm.Dob.Contains(dateSuffix))
            .Join(_db.MemberProfiles, x => x.fm.MemberProfileId, mp => mp.Id, (x, mp) => new { x.fm, x.gm, mp })
            .Select(x => new { profileId = x.mp.Id.ToString(), groupID = x.gm.GroupId.ToString(), memberName = x.fm.MemberName, memberMobile = "", memberEmail = "", relation = x.fm.Relationship + " of " + x.mp.MemberName, msg = "BirthDay", hide_whatsnum = "0", hide_num = "0", hide_mail = "0" }).ToListAsync();

        var result = new List<object>();
        result.AddRange(birthdays);
        result.AddRange(anniversaries);
        result.AddRange(familyBdays);

        return new { TBMemberListResult = new { status = "0", message = "success", Result = result } };
    }

    public async Task<object> GetMonthEventListTypeWiseNational(TypeWiseRequest request)
    {
        var type = request.Type ?? "B";
        if (!DateTime.TryParse(request.SelectedDate, out var selectedDate))
            selectedDate = DateTime.Now;
        // Dob stored as "yyyy-MM-dd" string. Match by suffix "-MM-dd"
        var dateSuffix = selectedDate.ToString("-MM-dd");

        var events = new List<object>();

        if (type == "B")
        {
            var bdays = await _db.MemberProfiles
                .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
                .Join(_db.Groups, x => x.gm.GroupId, g => g.Id, (x, g) => new { x.mp, x.gm, g })
                .Where(x => x.gm.GroupId != 31185 && x.mp.Dob != null && x.mp.Dob.Contains(dateSuffix))
                .Select(x => new
                {
                    MemberID = "M" + x.mp.ImeiMembershipId,
                    eventDate = x.mp.Dob + " 00:00:00",
                    title = x.mp.MemberName + "   ( " + x.g.GrpName + " )",
                    eventImg = (string?)null,
                    GroupId = x.gm.GroupId.ToString(),
                    EmailId = x.mp.MemberEmail ?? "",
                    ContactNumber = (x.mp.CountryCode != null ? "+" + x.mp.CountryCode + " " : "+91 ") + (x.mp.MemberMobile ?? ""),
                    Description = (string?)null,
                    EventTime = (string?)null,
                    type = "Birthday",
                    MemberProfileId = (string?)null,
                    EventVenue = (string?)null,
                    RegLink = (string?)null,
                    hide_whatsnum = x.mp.HideWhatsnum ?? "0",
                    hide_num = x.mp.HideNum ?? "0",
                    hide_mail = x.mp.HideMail ?? "0"
                }).ToListAsync();
            events.AddRange(bdays);
        }
        else if (type == "A")
        {
            var annis = await _db.MemberProfiles
                .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
                .Join(_db.Groups, x => x.gm.GroupId, g => g.Id, (x, g) => new { x.mp, x.gm, g })
                .Where(x => x.gm.GroupId != 31185 && x.mp.Doa != null && x.mp.Doa.Contains(dateSuffix))
                .Select(x => new
                {
                    MemberID = "M" + x.mp.ImeiMembershipId,
                    eventDate = x.mp.Doa + " 00:00:00",
                    title = x.mp.MemberName + "   ( " + x.g.GrpName + " )",
                    eventImg = (string?)null,
                    GroupId = x.gm.GroupId.ToString(),
                    EmailId = x.mp.MemberEmail ?? "",
                    ContactNumber = (x.mp.CountryCode != null ? "+" + x.mp.CountryCode + " " : "+91 ") + (x.mp.MemberMobile ?? ""),
                    Description = (string?)null,
                    EventTime = (string?)null,
                    type = "Anniversary",
                    MemberProfileId = (string?)null,
                    EventVenue = (string?)null,
                    RegLink = (string?)null,
                    hide_whatsnum = x.mp.HideWhatsnum ?? "0",
                    hide_num = x.mp.HideNum ?? "0",
                    hide_mail = x.mp.HideMail ?? "0"
                }).ToListAsync();
            events.AddRange(annis);
        }
        else if (type == "E")
        {
            var evts = await _db.Events.Where(e => e.EventDate != null && e.EventDate.Contains(selectedDate.ToString("yyyy-MM")))
                .Select(e => new
                {
                    MemberID = "E" + e.Id,
                    eventDate = e.EventDate,
                    title = e.EventTitle,
                    // Strip inline base64 data URLs — they bloat the list payload
                    // (each one is ~hundreds of KB). Historical rows still have
                    // base64 stored in EventImageId from before the upload fix.
                    eventImg = e.EventImageId != null && e.EventImageId.StartsWith("data:") ? null : e.EventImageId,
                    GroupId = e.GroupId.ToString(),
                    EmailId = "",
                    ContactNumber = "",
                    Description = e.EventDesc,
                    EventTime = (string?)null,
                    type = "Event",
                    MemberProfileId = (string?)null,
                    EventVenue = e.EventVenue,
                    RegLink = e.RegLink
                }).ToListAsync();
            events.AddRange(evts);
        }

        return new { TBEventListTypeResult = new { status = "0", message = "success", updatedOn = DateTime.Now.ToString("yyyy-MM-dd HHmm:ss"), Result = new { Events = events } } };
    }

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
    public SettingsService(AppDbContext db, IHttpClientFactory httpClientFactory) { _db = db; }

    public async Task<object> GetTouchbaseSetting(TouchbaseSettingRequest request)
    {
        var uid = int.TryParse(request.mainMasterId, out var u) ? u : 0;
        var settings = await _db.TouchbaseSettings.Where(ts => ts.UserId == uid)
            .Select(ts => new { SettingResult = new { grpId = ts.GrpId, grpVal = ts.GrpVal, grpName = ts.GrpName } }).ToListAsync();
        return new { TBSettingResult = new { status = "0", message = "success", AllTBSettingResults = settings } };
    }
    public async Task<object> UpdateTouchbaseSetting(UpdateTouchbaseSettingRequest request)
    {
        var uid = int.TryParse(request.mainMasterId, out var u) ? u : 0;
        var grpId = int.TryParse(request.GroupId, out var g) ? g : 0;
        var grpIdStr = request.GroupId ?? "";
        var existing = await _db.TouchbaseSettings.FirstOrDefaultAsync(ts => ts.UserId == uid && ts.GrpId == grpIdStr);
        if (existing != null) { existing.GrpVal = request.UpdatedValue; }
        else { _db.TouchbaseSettings.Add(new TouchbaseSetting { UserId = uid, GrpId = grpIdStr, GrpVal = request.UpdatedValue }); }
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
    public async Task<object> GetGroupSetting(GroupSettingRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var g) ? g : 0;
        var settings = await _db.GroupSettings.Where(gs => gs.GroupId == grpId)
            .Select(gs => new { gs.ModuleId, gs.ModVal }).ToListAsync();
        return new { TBGroupSettingResult = new { status = "0", message = "success", GroupSettingResult = settings } };
    }
    public async Task<object> UpdateGroupSetting(UpdateGroupSettingRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var g) ? g : 0;
        var existing = await _db.GroupSettings.FirstOrDefaultAsync(gs => gs.GroupId == grpId && gs.ModuleId == request.ModuleId);
        if (existing != null) { existing.ModVal = request.UpdatedValue; }
        else { _db.GroupSettings.Add(new GroupSetting { GroupId = grpId, ModuleId = request.ModuleId, ModVal = request.UpdatedValue }); }
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }
}

public class FindClubService : IFindClubService
{
    private readonly AppDbContext _db;
    public FindClubService(AppDbContext db, IHttpClientFactory httpClientFactory) { _db = db; }
    public async Task<object> GetClubList(ClubSearchRequest request)
    {
        var query = _db.Clubs.AsQueryable();
        if (!string.IsNullOrEmpty(request.keyword)) query = query.Where(c => c.ClubName != null && c.ClubName.Contains(request.keyword));
        if (!string.IsNullOrEmpty(request.country)) query = query.Where(c => c.Country != null && c.Country.Contains(request.country));
        var clubs = await query.Select(c => new { GroupId = c.GroupId, group_name = c.ClubName, address = c.Address, city = c.City, State = c.State, country_name = c.Country, Meeting_Day = c.MeetingDay, meeting_from_time = c.MeetingTime, member_name = c.PresidentName, member_name0 = c.SecretaryName }).ToListAsync();
        // Table1: committee members per branch (only active members in active groups)
        var committees = await _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Join(_db.Groups, x => x.gm.GroupId, g => g.Id, (x, g) => new { x.mp, x.gm, g })
            .Where(x => x.g.IsActive && x.gm.IsActive && x.mp.Designation != null && x.mp.Designation != "")
            .Select(x => new { GroupId = x.gm.GroupId, member_name = x.mp.MemberName, Designation = x.mp.Designation, mobile_no = x.mp.MemberMobile, hide_num = x.mp.HideNum, email_id = x.mp.MemberEmail, hide_mail = x.mp.HideMail, member_name0 = x.mp.MemberName, member_name1 = x.mp.MemberName, Designation0 = x.mp.Designation, Designation1 = x.mp.Designation, mobile_no0 = x.mp.MemberMobile, mobile_no1 = x.mp.MemberMobile, hide_num0 = x.mp.HideNum, hide_num1 = x.mp.HideNum, email_id0 = x.mp.MemberEmail, email_id1 = x.mp.MemberEmail, hide_mail0 = x.mp.HideMail, hide_mail1 = x.mp.HideMail }).ToListAsync();
        return new { TBGetClubResult = new { status = "0", message = "success", ClubResult = new { Table = clubs, Table1 = committees } } };
    }
    public async Task<object> GetClubsNearMe(ClubNearMeRequest request) => await GetClubList(new ClubSearchRequest());
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
    public async Task<object> GetCommitteeList()
    {
        var committees = await _db.SubGroups.Where(sg => sg.GroupId == 31185)
            .Select(sg => new { Pk_subcomittee_id = sg.Id, committeName = sg.SubgrpTitle }).ToListAsync();
        var members = await _db.SubGroupMembers
            .Join(_db.SubGroups, sgm => sgm.SubGroupId, sg => sg.Id, (sgm, sg) => new { sgm, sg })
            .Select(x => new { Pk_subcomittee_id = x.sg.Id, committeName = x.sg.SubgrpTitle, membername = x.sgm.MemberName, mobilenumber = x.sgm.MobileNumber, EmailId = x.sgm.EmailId, designation = x.sgm.Designation, branch = x.sgm.Branch, othermobilenumber = "", otheremailid = "" }).ToListAsync();
        return new { TBGetClubResult = new { status = "0", message = "success", ClubResult = new { Table = committees, Table1 = members } } };
    }
}

public class FindRotarianService : IFindRotarianService
{
    private readonly AppDbContext _db;
    public FindRotarianService(AppDbContext db, IHttpClientFactory httpClientFactory) { _db = db; }

    public async Task<ZoneChapterResponse> GetZoneChapterList()
    {
        var zones = await _db.Zones.Select(z => new TouchBase.API.Models.DTOs.FindRotarian.ZoneItemDto { ZoneID = z.Id.ToString(), ZoneName = z.ZoneName }).ToListAsync();
        var chapters = await _db.Chapters.Select(c => new ChapterItemDto { ChapterID = c.Id.ToString(), ChapterName = c.ChapterName, ZoneID = c.ZoneId.ToString() }).ToListAsync();
        return new ZoneChapterResponse { status = "0", message = "success", ZoneChapterResult = new ZoneChapterResult { Table = zones, Table1 = chapters } };
    }
    public async Task<object> GetRotarianList(RotarianSearchRequest request)
    {
        var query = _db.MemberProfiles
            .Join(_db.GroupMembers, mp => mp.Id, gm => gm.MemberProfileId, (mp, gm) => new { mp, gm })
            .Join(_db.Groups, x => x.gm.GroupId, g => g.Id, (x, g) => new { x.mp, x.gm, g })
            .Where(x => x.g.IsActive && x.gm.IsActive && x.gm.GroupId != 31185);

        if (!string.IsNullOrEmpty(request.name)) query = query.Where(x => x.mp.MemberName != null && x.mp.MemberName.Contains(request.name));
        if (!string.IsNullOrEmpty(request.Grade)) query = query.Where(x => x.mp.MembershipGrade == request.Grade);
        if (!string.IsNullOrEmpty(request.Category)) query = query.Where(x => x.mp.Category != null && x.mp.Category.Contains(request.Category));
        if (!string.IsNullOrEmpty(request.memberMobile)) query = query.Where(x => x.mp.MemberMobile != null && x.mp.MemberMobile.Contains(request.memberMobile));
        if (!string.IsNullOrEmpty(request.club))
        {
            var clubId = int.TryParse(request.club, out var cid) ? cid : 0;
            if (clubId > 0) query = query.Where(x => x.gm.GroupId == clubId);
        }

        var members = await query.Take(200).Select(x => new { masterUID = x.mp.UserId.ToString(), groupID = x.gm.GroupId.ToString(), member_Name = x.mp.MemberName, memberMobile = x.mp.MemberMobile, mem_Category = x.mp.Category, Grade = x.mp.MembershipGrade, clubName = x.g.GrpName, pic = x.mp.ProfilePic }).ToListAsync();
        return new { TBFindRotarianResult = new { status = "0", message = "success", FindRotarianResult = members } };
    }
    public async Task<object> GetRotarianDetails(RotarianDetailRequest request)
    {
        var pid = int.TryParse(request.memberProfileId, out var p) ? p : 0;
        var mp = await _db.MemberProfiles.FirstOrDefaultAsync(m => m.Id == pid || m.UserId == pid);
        if (mp == null) return new { TBRotarianDetailsResult = new { status = "1", message = "Not found" } };
        var grp = await _db.GroupMembers.Include(gm => gm.Group).FirstOrDefaultAsync(gm => gm.MemberProfileId == mp.Id && gm.GroupId != 31185);
        var addr = await _db.AddressDetails.FirstOrDefaultAsync(a => a.MemberProfileId == mp.Id);
        return new { TBRotarianDetailsResult = new { status = "0", message = "success", RotarianDetailsResult = new {
            masterUID = mp.UserId.ToString(), profileID = mp.Id.ToString(),
            memberName = mp.MemberName, memberMobile = mp.MemberMobile,
            Whatsapp_num = mp.MemberMobile, Secondry_num = mp.SecondaryMobileNo,
            memberEmail = mp.MemberEmail, Email = mp.MemberEmail,
            designation = mp.Designation, clubName = grp?.Group?.GrpName,
            Chaptr_Brnch_Name = grp?.Group?.GrpName, Club_Name = grp?.Group?.GrpName,
            pic = mp.ProfilePic, member_profile_photo_path = mp.ProfilePic,
            Company_name = mp.CompanyName, blood_Group = mp.BloodGroup,
            IMEI_Membership_Id = mp.ImeiMembershipId, Membership_Grade = mp.MembershipGrade,
            CategoryName = mp.Category, mem_Category = mp.Category,
            DOB = mp.Dob, DOA = mp.Doa,
            member_date_of_birth = mp.Dob, member_date_of_wedding = mp.Doa,
            Keywords = mp.Keywords, country = addr?.Country ?? mp.MemberCountry,
            Address = addr?.Address, City = addr?.City, State = addr?.State, pincode = addr?.Pincode
        } } };
    }
    public async Task<object> GetCategoryList()
    {
        var cats = await _db.Categories.OrderBy(c => c.Id).Select(c => new { id = c.Id, name = c.CatName }).ToListAsync();
        return new { status = "0", message = "success", str = new { Table = cats } };
    }
    public Task<object> GetMemberGradeList()
    {
        // Canonical IMEI Membership Grade list — must be hard-coded so the
        // Add/Edit dropdowns always show every selectable grade, even ones
        // no member currently has. Mirrors the old PHP API's response shape
        // (FindRotarian/GetMemberGradeList) including the original ids.
        var grades = new[]
        {
            new { id = 1, name = "Fellow" },
            new { id = 3, name = "Member" },
            new { id = 5, name = "Associate" },
            new { id = 6, name = "Associate Member" },
            new { id = 7, name = "Graduate" },
            new { id = 9, name = "Student" },
        };
        return Task.FromResult<object>(
            new { status = "0", message = "success", str = new { Table = grades } }
        );
    }
    public async Task<object> GetClubList()
    {
        var clubs = await _db.Clubs
            .OrderBy(c => c.ClubName)
            .Select(c => new { id = c.GroupId, name = c.ClubName }).ToListAsync();
        return new { status = "0", message = "success", str = new { Table = clubs } };
    }
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
        var query = _db.GroupMembers.Include(gm => gm.MemberProfile).Include(gm => gm.Group).Where(gm => gm.GroupId == grpId && gm.IsActive);
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
    private readonly IConfiguration _configuration;
    public PastPresidentService(AppDbContext db, IHttpClientFactory httpClientFactory, IConfiguration configuration) { _db = db; _configuration = configuration; }
    public async Task<object> GetPastPresidentsList(PastPresidentRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        var query = _db.PastPresidents.Where(pp => pp.GroupId == grpId);
        if (!string.IsNullOrEmpty(request.SearchText)) query = query.Where(pp => pp.MemberName != null && pp.MemberName.Contains(request.SearchText));
        var presidents = await query.OrderBy(pp => pp.Id)
            .Select(pp => new { PastPresidentId = pp.Id.ToString(), MemberName = pp.MemberName, PhotoPath = pp.PhotoPath, TenureYear = pp.TenureYear }).ToListAsync();
        return new { TBPastPresidentListResult = new { status = "0", message = "success", updatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), TBPastPresidentList = new { newRecords = presidents, updatedRecords = new List<object>(), deletedRecords = "" } } };
    }

    public async Task<object> AddPastPresident(AddPastPresidentRequest request)
    {
        var grpId = int.TryParse(request.GroupId, out var gid) ? gid : 0;
        var photoPath = request.PhotoPath;
        if (!string.IsNullOrEmpty(photoPath) && photoPath.StartsWith("data:image"))
        {
            var base64Data = photoPath.Substring(photoPath.IndexOf(",") + 1);
            var ext = photoPath.Contains("png") ? ".png" : ".jpg";
            var fileName = $"{Guid.NewGuid()}{ext}";
            var dir = Path.Combine("wwwroot", "uploads", "pastpresident"); Directory.CreateDirectory(dir);
            await File.WriteAllBytesAsync(Path.Combine(dir, fileName), Convert.FromBase64String(base64Data));
            var appBaseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? "";
            photoPath = $"{appBaseUrl}/uploads/pastpresident/{fileName}";
        }
        var pp = new PastPresident { GroupId = grpId, MemberName = request.MemberName, PhotoPath = photoPath, TenureYear = request.TenureYear, Designation = request.Designation };
        _db.PastPresidents.Add(pp);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<object> UpdatePastPresident(UpdatePastPresidentRequest request)
    {
        var ppId = int.TryParse(request.PastPresidentId, out var pid) ? pid : 0;
        var pp = await _db.PastPresidents.FindAsync(ppId);
        if (pp == null) return new { status = "1", message = "Not found" };
        if (request.MemberName != null) pp.MemberName = request.MemberName;
        if (request.PhotoPath != null)
        {
            var photoPath = request.PhotoPath;
            if (!string.IsNullOrEmpty(photoPath) && photoPath.StartsWith("data:image"))
            {
                var base64Data = photoPath.Substring(photoPath.IndexOf(",") + 1);
                var ext = photoPath.Contains("png") ? ".png" : ".jpg";
                var fileName = $"{Guid.NewGuid()}{ext}";
                var dir = Path.Combine("wwwroot", "uploads", "pastpresident"); Directory.CreateDirectory(dir);
                await File.WriteAllBytesAsync(Path.Combine(dir, fileName), Convert.FromBase64String(base64Data));
                var appBaseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? "";
                photoPath = $"{appBaseUrl}/uploads/pastpresident/{fileName}";
            }
            pp.PhotoPath = photoPath;
        }
        if (request.TenureYear != null) pp.TenureYear = request.TenureYear;
        if (request.Designation != null) pp.Designation = request.Designation;
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
    }

    public async Task<object> DeletePastPresident(string pastPresidentId)
    {
        var ppId = int.TryParse(pastPresidentId, out var pid) ? pid : 0;
        var pp = await _db.PastPresidents.FindAsync(ppId);
        if (pp == null) return new { status = "1", message = "Not found" };
        _db.PastPresidents.Remove(pp);
        await _db.SaveChangesAsync();
        return new { status = "0", message = "success" };
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
