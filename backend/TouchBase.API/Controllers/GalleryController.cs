using Microsoft.AspNetCore.Mvc;
using TouchBase.API.Models.DTOs.Gallery;
using TouchBase.API.Services.Interfaces;

namespace TouchBase.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GalleryController : ControllerBase
{
    private readonly IGalleryService _galleryService;
    public GalleryController(IGalleryService galleryService) => _galleryService = galleryService;

    [HttpPost("GetAlbumsList")]
    public async Task<IActionResult> GetAlbumsList([FromBody] AlbumListRequest request)
    {
        try { return Ok(await _galleryService.GetAlbumsList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAlbumsList_New")]
    public async Task<IActionResult> GetAlbumsListNew([FromBody] ShowcaseAlbumsRequest request)
    {
        try { return Ok(await _galleryService.GetAlbumsListNew(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAlbumPhotoList_New")]
    public async Task<IActionResult> GetAlbumPhotoList([FromBody] AlbumPhotoListRequest request)
    {
        try { return Ok(await _galleryService.GetAlbumPhotoList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddUpdateAlbum_New")]
    public async Task<IActionResult> AddUpdateAlbum([FromBody] CreateAlbumRequest request)
    {
        try { return Ok(await _galleryService.AddUpdateAlbum(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddUpdateAlbumPhoto")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddUpdateAlbumPhoto([FromForm] TouchBase.API.Models.DTOs.Upload.AddAlbumPhotoFormRequest request)
    {
        try { return Ok(await _galleryService.AddUpdateAlbumPhoto(request.file!, request.photoId!, request.desc!, request.albumId!, request.groupId!, request.createdBy!)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeleteAlbumPhoto")]
    public async Task<IActionResult> DeleteAlbumPhoto([FromBody] DeletePhotoRequest request)
    {
        try { return Ok(await _galleryService.DeleteAlbumPhoto(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetAlbumDetails_New")]
    public async Task<IActionResult> GetAlbumDetails([FromBody] AlbumDetailRequest request)
    {
        try { return Ok(await _galleryService.GetAlbumDetails(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetMER_List")]
    public async Task<IActionResult> GetMerList([FromBody] MerListRequest request)
    {
        try { return Ok(await _galleryService.GetMerList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("GetYear")]
    public async Task<IActionResult> GetYear([FromBody] YearRequest request)
    {
        try { return Ok(await _galleryService.GetYear(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("Fillyearlist")]
    public async Task<IActionResult> FillYearList([FromBody] object request)
    {
        try { return Ok(await _galleryService.FillYearList(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("AddMER")]
    public async Task<IActionResult> AddMER([FromBody] AddMerRequest request)
    {
        try { return Ok(await _galleryService.AddMer(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("UpdateMER")]
    public async Task<IActionResult> UpdateMER([FromBody] UpdateMerRequest request)
    {
        try { return Ok(await _galleryService.UpdateMer(request)); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }

    [HttpPost("DeleteMER")]
    public async Task<IActionResult> DeleteMER([FromBody] DeleteMerRequest request)
    {
        try { return Ok(await _galleryService.DeleteMer(request.MER_ID ?? "")); }
        catch (Exception ex) { return Ok(new { status = "1", message = ex.Message }); }
    }
}

public class AddMerRequest
{
    public string? GroupId { get; set; }
    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? FilePath { get; set; }
    public string? PublishDate { get; set; }
    public string? ExpiryDate { get; set; }
    public string? FinanceYear { get; set; }
    public string? TransType { get; set; }
}

public class UpdateMerRequest
{
    public string? MER_ID { get; set; }
    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? FilePath { get; set; }
    public string? PublishDate { get; set; }
    public string? ExpiryDate { get; set; }
    public string? FinanceYear { get; set; }
    public string? TransType { get; set; }
}

public class DeleteMerRequest
{
    public string? MER_ID { get; set; }
}