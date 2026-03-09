namespace TouchBase.API.Models.DTOs.Dashboard;

// ─── Requests ───

public class ModuleListRequest
{
    public string? groupId { get; set; }
    public string? memberProfileId { get; set; }
}

public class DashboardRequest
{
    public string? groupId { get; set; }
    public string? memberProfileId { get; set; }
}

public class AdminSubmodulesRequest
{
    public string? Fk_groupID { get; set; }
    public string? fk_ProfileID { get; set; }
}

public class UpdateModuleDashboardRequest
{
    public string? memberProfileId { get; set; }
    public string? modulelist { get; set; }
}

public class AssistanceGovRequest
{
    public string? grpID { get; set; }
    public string? profileId { get; set; }
}

// ─── Responses ───

public class ModuleListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<GroupModuleDto>? GroupListResult { get; set; }
}

public class GroupModuleDto
{
    public string? groupModuleId { get; set; }
    public string? groupId { get; set; }
    public string? moduleId { get; set; }
    public string? moduleName { get; set; }
    public string? moduleStaticRef { get; set; }
    public string? image { get; set; }
    public string? masterProfileID { get; set; }
    public string? isCustomized { get; set; }
    public string? moduleOrderNo { get; set; }
    public string? notificationCount { get; set; }
    public string? modulePriceRs { get; set; }
    public string? modulePriceUS { get; set; }
    public string? moduleInfo { get; set; }
}

public class DashboardResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<DashboardBannerDto>? BannerList { get; set; }
    public List<DashboardBannerDto>? SliderList { get; set; }
}

public class DashboardBannerDto
{
    public string? bannerId { get; set; }
    public string? bannerImage { get; set; }
    public string? bannerTitle { get; set; }
    public string? bannerDescription { get; set; }
    public string? bannerUrl { get; set; }
    public string? bannerType { get; set; }
}

public class NotificationCountResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? notificationCount { get; set; }
}

public class AdminSubmodulesResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<AdminSubmoduleDto>? list { get; set; }
}

public class AdminSubmoduleDto
{
    public string? moduleId { get; set; }
    public string? moduleName { get; set; }
    public string? moduleStaticRef { get; set; }
    public string? image { get; set; }
    public string? groupId { get; set; }
    public string? groupModuleId { get; set; }
    public string? moduleOrderNo { get; set; }
    public string? notificationCount { get; set; }
}
