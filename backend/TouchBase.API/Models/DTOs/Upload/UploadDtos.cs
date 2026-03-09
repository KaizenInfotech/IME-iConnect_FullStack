using Microsoft.AspNetCore.Http;

namespace TouchBase.API.Models.DTOs.Upload;

public class UploadImageResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? imageId { get; set; }
}

public class UploadImageFormRequest
{
    public IFormFile? file { get; set; }
    public string? module { get; set; }
}

public class AddDocumentFormRequest
{
    public IFormFile? file { get; set; }
    public string? grpID { get; set; }
    public string? profileID { get; set; }
    public string? docTitle { get; set; }
}

public class AddAlbumPhotoFormRequest
{
    public IFormFile? file { get; set; }
    public string? photoId { get; set; }
    public string? desc { get; set; }
    public string? albumId { get; set; }
    public string? groupId { get; set; }
    public string? createdBy { get; set; }
}

public class ProfilePhotoFormRequest
{
    public IFormFile? file { get; set; }
    public string? ProfileID { get; set; }
}