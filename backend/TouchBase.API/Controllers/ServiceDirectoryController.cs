using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.ServiceDirectory;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceDirectoryController : ControllerBase
{
    private readonly IServiceDirectoryService _serviceDirectoryService;
    public ServiceDirectoryController(IServiceDirectoryService serviceDirectoryService) => _serviceDirectoryService = serviceDirectoryService;

    [HttpPost("GetServiceCategoriesData")]
    public async Task<IActionResult> GetServiceCategoriesData([FromBody] ServiceCategoriesRequest request)
    {
        try { return Ok(await _serviceDirectoryService.GetServiceCategoriesData(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetServiceDirectoryCategories")]
    public async Task<IActionResult> GetServiceDirectoryCategories([FromBody] object request)
    {
        try { return Ok(await _serviceDirectoryService.GetServiceDirectoryCategories(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetServiceDirectoryDetails")]
    public async Task<IActionResult> GetServiceDirectoryDetails([FromBody] ServiceDetailRequest request)
    {
        try { return Ok(await _serviceDirectoryService.GetServiceDirectoryDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddServiceDirectory")]
    public async Task<IActionResult> AddServiceDirectory([FromBody] AddServiceRequest request)
    {
        try { return Ok(await _serviceDirectoryService.AddServiceDirectory(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
