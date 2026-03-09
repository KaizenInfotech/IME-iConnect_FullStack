using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Document;
using TouchBase.API.Models.DTOs.Upload;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentSafeController : ControllerBase
{
    private readonly IDocumentService _documentService;
    public DocumentSafeController(IDocumentService documentService) => _documentService = documentService;

    [HttpPost("AddDocument")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddDocument([FromForm] AddDocumentFormRequest request)
    {
        try { return Ok(await _documentService.AddDocument(request.file!, request.grpID!, request.profileID!, request.docTitle!)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetDocumentList")]
    public async Task<IActionResult> GetDocumentList([FromBody] DocumentListRequest request)
    {
        try { return Ok(await _documentService.GetDocumentList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateDocumentIsRead")]
    public async Task<IActionResult> UpdateDocumentIsRead([FromBody] UpdateDocReadRequest request)
    {
        try { return Ok(await _documentService.UpdateDocumentIsRead(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}
