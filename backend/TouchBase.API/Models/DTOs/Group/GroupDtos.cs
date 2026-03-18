namespace TouchBase.API.Models.DTOs.Group;

// ─── Requests ───

public class AllGroupsRequest
{
    public string? masterUID { get; set; }
    public string? imeiNo { get; set; }
}

public class CreateGroupRequest
{
    public string? grpId { get; set; }
    public string? grpName { get; set; }
    public string? grpImageID { get; set; }
    public string? grpType { get; set; }
    public string? grpCategory { get; set; }
    public string? addrss1 { get; set; }
    public string? addrss2 { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? pincode { get; set; }
    public string? country { get; set; }
    public string? emailid { get; set; }
    public string? mobile { get; set; }
    public string? userId { get; set; }
    public string? website { get; set; }
    public string? other { get; set; }
}

public class GroupDetailRequest
{
    public string? memberMainId { get; set; }
    public string? groupId { get; set; }
}

public class AddMemberRequest
{
    public string? mobile { get; set; }
    public string? userName { get; set; }
    public string? groupId { get; set; }
    public string? masterID { get; set; }
    public string? countryId { get; set; }
    public string? memberEmail { get; set; }
}

public class AddMultipleMembersRequest
{
    public string? groupId { get; set; }
    public string? masterID { get; set; }
    public string? memberData { get; set; }
}

public class GlobalSearchRequest
{
    public string? memId { get; set; }
    public string? otherMemId { get; set; }
}

public class DeleteByModuleRequest
{
    public string? typeID { get; set; }
    public string? type { get; set; }
    public string? profileID { get; set; }
}

public class UpdateCategoryRequest
{
    public string? memberProfileId { get; set; }
    public string? mycategory { get; set; }
    public string? memberMainId { get; set; }
}

public class RemoveCategoryRequest
{
    public string? memberProfileId { get; set; }
    public string? memberMainId { get; set; }
}

public class EntityInfoRequest
{
    public string? grpID { get; set; }
    public string? moduleID { get; set; }
    public string? SearchText { get; set; }
}

public class FeedbackRequest
{
    public string? groupId { get; set; }
    public string? profileId { get; set; }
    public string? feedback { get; set; }
}

public class PopupRequest
{
    public string? groupId { get; set; }
    public string? profileId { get; set; }
}

public class UpdatePopupRequest
{
    public string? groupId { get; set; }
    public string? profileId { get; set; }
    public string? popupId { get; set; }
}

public class DeviceTokenRequest
{
    public string? MobileNumber { get; set; }
    public string? DeviceToken { get; set; }
    public string? Platform { get; set; }
}

public class SubGroupRequest
{
    public string? groupId { get; set; }
}

public class CreateSubGroupRequest
{
    public string? subGroupTitle { get; set; }
    public string? memberProfileId { get; set; }
    public string? groupId { get; set; }
    public string? memberMainId { get; set; }
    public string? parentID { get; set; }
}

public class SubGroupDetailRequest
{
    public string? groupId { get; set; }
    public string? subgrpId { get; set; }
}

public class GroupSyncRequest
{
    public string? masterUID { get; set; }
    public string? imeiNo { get; set; }
    public string? loginType { get; set; }
    public string? mobileNo { get; set; }
    public string? countryCode { get; set; }
    public string? updatedOn { get; set; }
}

public class GetEmailRequest
{
    public string? groupId { get; set; }
    public string? profileId { get; set; }
}

public class RemoveSelfRequest
{
    public string? memberProfileId { get; set; }
    public string? grpID { get; set; }
}

// ─── Responses ───

public class AllGroupsResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? version { get; set; }
    public List<GroupResultDto>? AllGroupListResults { get; set; }
    public List<GroupResultDto>? PersonalGroupListResults { get; set; }
    public List<GroupResultDto>? SocialGroupListResults { get; set; }
    public List<GroupResultDto>? BusinessGroupListResults { get; set; }
}

public class GroupResultDto
{
    public string? grpId { get; set; }
    public string? grpName { get; set; }
    public string? grpImg { get; set; }
    public string? grpProfileId { get; set; }
    public string? grpProfileid { get; set; } // Flutter checks both casings
    public string? myCategory { get; set; }
    public string? isGrpAdmin { get; set; }
    public string? moduleId { get; set; }
}

public class CreateGroupResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? grdId { get; set; } // Note: iOS typo preserved
}

public class GroupDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<GroupDetailDto>? getGroupDetailResult { get; set; }
}

public class GroupDetailDto
{
    public string? grpId { get; set; }
    public string? grpName { get; set; }
    public string? grpImg { get; set; }
    public string? grpType { get; set; }
    public string? grpCategory { get; set; }
    public string? addrss1 { get; set; }
    public string? addrss2 { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? pincode { get; set; }
    public string? country { get; set; }
    public string? emailid { get; set; }
    public string? mobile { get; set; }
    public string? website { get; set; }
    public string? totalMembers { get; set; }
}

public class SubGroupListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<SubGroupDto>? SubGroupResult { get; set; }
}

public class SubGroupDto
{
    public string? subgrpId { get; set; }
    public string? subgrpTitle { get; set; }
    public string? noOfmem { get; set; }
}

public class SubGroupDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<SubGroupMemberDto>? SubGroupResult { get; set; }
}

public class SubGroupMemberDto
{
    public string? profileId { get; set; }
    public string? memname { get; set; }
    public string? mobile { get; set; }
}

public class AddMemberGroupResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? totalMember { get; set; }
}

public class GlobalSearchResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? membername { get; set; }
    public string? membermobile { get; set; }
    public string? profilepicpath { get; set; }
    public List<GlobalGroupItemDto>? AllGlobalGroupListResults { get; set; }
}

public class GlobalGroupItemDto
{
    public string? grpId { get; set; }
    public string? grpName { get; set; }
    public string? grpImg { get; set; }
    public string? grpProfileId { get; set; }
    public string? isMygrp { get; set; }
}

public class EntityInfoResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? groupName { get; set; }
    public string? groupImg { get; set; }
    public string? contactNo { get; set; }
    public string? address { get; set; }
    public string? email { get; set; }
    public List<EntityInfoItemDto>? EntityInfoResult { get; set; }
    public List<AdminInfoItemDto>? AdminInfoResult { get; set; }
}

public class EntityInfoItemDto
{
    public string? key { get; set; }
    public string? value { get; set; }
}

public class AdminInfoItemDto
{
    public string? memberName { get; set; }
    public string? designation { get; set; }
    public string? mobile { get; set; }
    public string? mobileNo { get; set; }
    public string? email { get; set; }
    public string? emailID { get; set; }
    public string? pic { get; set; }
    public string? profileId { get; set; }
    public string? profileID { get; set; }
}

public class CountryCategoryResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<CountryDto>? CountryLists { get; set; }
    public List<CategoryDto>? CategoryLists { get; set; }
}

public class CountryDto
{
    public string? countryId { get; set; }
    public string? countryCode { get; set; }
    public string? countryName { get; set; }
}

public class CategoryDto
{
    public string? catId { get; set; }
    public string? catName { get; set; }
}

public class ZoneListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<ZoneItemDto>? zonelistResult { get; set; }
}

public class ZoneItemDto
{
    public string? PK_zoneID { get; set; }
    public string? zoneName { get; set; }
}
