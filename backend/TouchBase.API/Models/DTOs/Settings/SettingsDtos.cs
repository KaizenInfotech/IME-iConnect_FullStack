namespace TouchBase.API.Models.DTOs.Settings;

// ─── Requests ───

public class TouchbaseSettingRequest
{
    public string? mainMasterId { get; set; }
}

public class UpdateTouchbaseSettingRequest
{
    public string? GroupId { get; set; }
    public string? UpdatedValue { get; set; }
    public string? mainMasterId { get; set; }
}

public class GroupSettingRequest
{
    public string? GroupProfileId { get; set; }
    public string? GroupId { get; set; }
}

public class UpdateGroupSettingRequest
{
    public string? GroupId { get; set; }
    public string? UpdatedValue { get; set; }
    public string? GroupProfileId { get; set; }
    public string? ModuleId { get; set; }
    public string? showMobileSeflfClub { get; set; }
    public string? showMobileOutsideClub { get; set; }
    public string? showEmailSeflfClub { get; set; }
    public string? showEmailOutsideClub { get; set; }
    public string? isMob { get; set; }
    public string? isEmail { get; set; }
    public string? isPersonal { get; set; }
    public string? isFamily { get; set; }
    public string? isBusiness { get; set; }
}

// ─── Responses ───

public class TouchbaseSettingResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public TouchbaseSettingResultWrapper? TBSettingResult { get; set; }
}

public class TouchbaseSettingResultWrapper
{
    public TouchbaseSettingResults? AllTBSettingResults { get; set; }
}

public class TouchbaseSettingResults
{
    public List<SettingItemDto>? TBSettingResults { get; set; }
}

public class SettingItemDto
{
    public string? grpId { get; set; }
    public string? grpVal { get; set; }
    public string? grpName { get; set; }
}

public class GroupSettingResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public GroupSettingResultWrapper? TBGroupSettingResult { get; set; }
}

public class GroupSettingResultWrapper
{
    public GroupSettingResult? GRpSettingResult { get; set; }
}

public class GroupSettingResult
{
    public string? isMob { get; set; }
    public string? isEmail { get; set; }
    public string? isPersonal { get; set; }
    public string? isFamily { get; set; }
    public string? isBusiness { get; set; }
    public List<GroupSettingItemDto>? GRpSettingDetails { get; set; }
}

public class GroupSettingItemDto
{
    public string? moduleId { get; set; }
    public string? modVal { get; set; }
    public string? modName { get; set; }
}
