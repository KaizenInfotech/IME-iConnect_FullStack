using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Dashboard;
using TouchBase.API.Models.DTOs.Group;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IDashboardService _dashboardService;
    public GroupController(IGroupService groupService, IDashboardService dashboardService)
    {
        _groupService = groupService;
        _dashboardService = dashboardService;
    }

    [HttpPost("GetAllCountriesAndCategories")]
    public async Task<IActionResult> GetAllCountriesAndCategories()
    {
        try { return Ok(await _groupService.GetAllCountriesAndCategories()); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAllGroupsList")]
    public async Task<IActionResult> GetAllGroupsList([FromBody] AllGroupsRequest request)
    {
        try { return Ok(await _groupService.GetAllGroupsList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("CreateGroup")]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        try { return Ok(await _groupService.CreateGroup(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("CreateSubGroup")]
    public async Task<IActionResult> CreateSubGroup([FromBody] CreateSubGroupRequest request)
    {
        try { return Ok(await _groupService.CreateSubGroup(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetGroupDetail")]
    public async Task<IActionResult> GetGroupDetail([FromBody] GroupDetailRequest request)
    {
        try { return Ok(await _groupService.GetGroupDetail(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetSubGroupList")]
    public async Task<IActionResult> GetSubGroupList([FromBody] SubGroupRequest request)
    {
        try { return Ok(await _groupService.GetSubGroupList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetSubGroupDetail")]
    public async Task<IActionResult> GetSubGroupDetail([FromBody] SubGroupDetailRequest request)
    {
        try { return Ok(await _groupService.GetSubGroupDetail(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddMemberToGroup")]
    public async Task<IActionResult> AddMemberToGroup([FromBody] AddMemberRequest request)
    {
        try { return Ok(await _groupService.AddMemberToGroup(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddMultipleMemberToGroup")]
    public async Task<IActionResult> AddMultipleMemberToGroup([FromBody] AddMultipleMembersRequest request)
    {
        try { return Ok(await _groupService.AddMultipleMembersToGroup(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GlobalSearchGroup")]
    public async Task<IActionResult> GlobalSearchGroup([FromBody] GlobalSearchRequest request)
    {
        try { return Ok(await _groupService.GlobalSearchGroup(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeleteByModuleName")]
    public async Task<IActionResult> DeleteByModuleName([FromBody] DeleteByModuleRequest request)
    {
        try { return Ok(await _groupService.DeleteByModuleName(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeleteImage")]
    public async Task<IActionResult> DeleteImage([FromBody] object request)
    {
        try { return Ok(await _groupService.DeleteImage(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateModuleDashboard")]
    public async Task<IActionResult> UpdateModuleDashboard([FromBody] UpdateModuleDashboardRequest request)
    {
        try { return Ok(await _dashboardService.UpdateModuleDashboard(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("RemoveGroupCategory")]
    public async Task<IActionResult> RemoveGroupCategory([FromBody] RemoveCategoryRequest request)
    {
        try { return Ok(await _groupService.RemoveGroupCategory(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateMemberGroupCategory")]
    public async Task<IActionResult> UpdateMemberGroupCategory([FromBody] UpdateCategoryRequest request)
    {
        try { return Ok(await _groupService.UpdateMemberGroupCategory(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetGroupModulesList")]
    public async Task<IActionResult> GetGroupModulesList([FromBody] ModuleListRequest request)
    {
        try { return Ok(await _groupService.GetGroupModulesList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetNotificationCount")]
    public async Task<IActionResult> GetNotificationCount([FromBody] object request)
    {
        try { return Ok(await _groupService.GetNotificationCount(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetEmail")]
    public async Task<IActionResult> GetEmail([FromBody] GetEmailRequest request)
    {
        try { return Ok(await _groupService.GetEmail(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetNewDashboard")]
    public async Task<IActionResult> GetNewDashboard([FromBody] DashboardRequest request)
    {
        try { return Ok(await _groupService.GetNewDashboard(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetRotaryLibraryData")]
    public async Task<IActionResult> GetRotaryLibraryData([FromBody] object request)
    {
        try { return Ok(await _groupService.GetRotaryLibraryData(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: lowercase 'g' in route to match Flutter's ApiConstants
    [HttpPost("getAdminSubModules")]
    public async Task<IActionResult> GetAdminSubModules([FromBody] AdminSubmodulesRequest request)
    {
        try { return Ok(await _groupService.GetAdminSubModules(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetEntityInfo")]
    public async Task<IActionResult> GetEntityInfo([FromBody] EntityInfoRequest request)
    {
        try { return Ok(await _groupService.GetEntityInfo(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAllGroupListSync")]
    public async Task<IActionResult> GetAllGroupListSync([FromBody] GroupSyncRequest request)
    {
        try { return Ok(await _groupService.GetAllGroupListSync(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClubDetails")]
    public async Task<IActionResult> GetClubDetails([FromBody] object request)
    {
        try { return Ok(await _groupService.GetClubDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetClubHistory")]
    public async Task<IActionResult> GetClubHistory([FromBody] object request)
    {
        try { return Ok(await _groupService.GetClubHistory(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("Feedback")]
    public async Task<IActionResult> Feedback([FromBody] FeedbackRequest request)
    {
        try { return Ok(await _groupService.SubmitFeedback(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: lowercase 'g' and no 'e' in route to match Flutter's ApiConstants
    [HttpPost("getZonelist")]
    public async Task<IActionResult> GetZoneList([FromBody] object request)
    {
        try { return Ok(await _groupService.GetZoneList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: iOS typo "Pupup" preserved
    [HttpPost("getMobilePupup")]
    public async Task<IActionResult> GetMobilePopup([FromBody] PopupRequest request)
    {
        try { return Ok(await _groupService.GetMobilePopup(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    // Note: iOS typo "Pupupflag" preserved
    [HttpPost("UpdateMobilePupupflag")]
    public async Task<IActionResult> UpdateMobilePopupFlag([FromBody] UpdatePopupRequest request)
    {
        try { return Ok(await _groupService.UpdateMobilePopupFlag(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateDeviceTokenNumber")]
    public async Task<IActionResult> UpdateDeviceTokenNumber([FromBody] DeviceTokenRequest request)
    {
        try { return Ok(await _groupService.UpdateDeviceTokenNumber(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAssistanceGov")]
    public async Task<IActionResult> GetAssistanceGov([FromBody] AssistanceGovRequest request)
    {
        try { return Ok(await _groupService.GetAssistanceGov(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

}
