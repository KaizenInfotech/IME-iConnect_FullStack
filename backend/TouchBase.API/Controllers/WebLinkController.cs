using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.WebLink;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebLinkController : ControllerBase
{
    private readonly IWebLinkService _webLinkService;
    public WebLinkController(IWebLinkService webLinkService) => _webLinkService = webLinkService;

    [HttpPost("GetWebLinksList")]
    public async Task<IActionResult> GetWebLinksList([FromBody] WebLinkListRequest request)
    {
        try { return Ok(await _webLinkService.GetWebLinksList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
