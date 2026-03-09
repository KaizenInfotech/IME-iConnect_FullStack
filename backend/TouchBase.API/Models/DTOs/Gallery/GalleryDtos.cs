namespace TouchBase.API.Models.DTOs.Gallery;

// ─── Requests ───

public class AlbumListRequest
{
    public string? profileId { get; set; }
    public string? groupId { get; set; }
    public string? updatedOn { get; set; }
    public string? moduleId { get; set; }
}

public class ShowcaseAlbumsRequest
{
    public string? groupId { get; set; }
    public string? district_id { get; set; }
    public string? club_id { get; set; }
    public string? category_id { get; set; }
    public string? year { get; set; }
    public string? SharType { get; set; }
    public string? profileId { get; set; }
    public string? moduleId { get; set; }
    public string? searchText { get; set; }
}

public class AlbumPhotoListRequest
{
    public string? albumId { get; set; }
    public string? groupId { get; set; }
    public string? updatedOn { get; set; }
    public string? Financeyear { get; set; }
}

public class AlbumDetailRequest
{
    public string? albumId { get; set; }
}

public class CreateAlbumRequest
{
    public string? albumId { get; set; }
    public string? groupId { get; set; }
    public string? type { get; set; }
    public string? memberIds { get; set; }
    public string? albumTitle { get; set; }
    public string? albumDescription { get; set; }
    public string? albumImage { get; set; }
    public string? createdBy { get; set; }
    public string? isSubGrpAdmin { get; set; }
    public string? subgrpIDs { get; set; }
    public string? moduleId { get; set; }
    public string? shareType { get; set; }
    public string? categoryId { get; set; }
    public string? dateofproject { get; set; }
    public string? costofproject { get; set; }
    public string? beneficiary { get; set; }
    public string? manhourspent { get; set; }
    public string? Manhourspenttype { get; set; }
    public string? NumberofRotarian { get; set; }
    public string? OtherCategorytext { get; set; }
    public string? costofprojecttype { get; set; }
}

public class DeletePhotoRequest
{
    public string? photoId { get; set; }
    public string? albumId { get; set; }
    public string? deletedBy { get; set; }
}

public class YearRequest
{
    public string? grpID { get; set; }
    public string? Type { get; set; }
}

public class MerListRequest
{
    public string? FinanceYear { get; set; }
    public string? TransType { get; set; }
    public string? grpID { get; set; }
}

// ─── Responses ───

public class AlbumListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public AlbumListResult? Result { get; set; }
}

public class AlbumListResult
{
    public string? updatedOn { get; set; }
    public List<AlbumItemDto>? newAlbums { get; set; }
    public List<AlbumItemDto>? updatedAlbums { get; set; }
    public string? deletedAlbums { get; set; }
}

public class AlbumItemDto
{
    public string? albumId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? GroupID { get; set; }
    public string? ModuleID { get; set; }
    public string? Type { get; set; }
    public string? isAdmin { get; set; }
    public string? beneficiary { get; set; }
    public string? cost_of_project { get; set; }
    public string? cost_of_project_type { get; set; }
    public string? working_hour { get; set; }
    public string? working_hour_type { get; set; }
    public string? project_date { get; set; }
    public string? clubname { get; set; }
    public string? sharetype { get; set; }
    public string? NumberOfRotarian { get; set; }
    public string? albumCategoryID { get; set; }
    public string? albumCategoryText { get; set; }
}

public class AlbumPhotoListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public AlbumPhotoListResult? Result { get; set; }
}

public class AlbumPhotoListResult
{
    public string? updatedOn { get; set; }
    public List<AlbumPhotoDto>? newPhotos { get; set; }
    public List<AlbumPhotoDto>? updatedPhotos { get; set; }
    public string? deletedPhotos { get; set; }
}

public class AlbumPhotoDto
{
    public string? photoId { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
}

public class AlbumDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public AlbumDetailResultWrapper? AlbumDetailResult { get; set; }
}

public class AlbumDetailResultWrapper
{
    public List<AlbumDetailDto>? AlbumDetail { get; set; }
}

public class AlbumDetailDto
{
    public string? albumTitle { get; set; }
    public string? albumDescription { get; set; }
    public string? albumImage { get; set; }
    public string? type { get; set; }
    public string? groupId { get; set; }
    public string? albumId { get; set; }
    public string? memberIds { get; set; }
    public string? beneficiary { get; set; }
    public string? NumberOfRotarian { get; set; }
    public string? projectCost { get; set; }
    public string? projectDate { get; set; }
    public string? workingHour { get; set; }
    public string? albumCategoryID { get; set; }
    public string? albumCategoryText { get; set; }
    public string? otherCategoryText { get; set; }
    public string? shareType { get; set; }
    public string? costOfProjectType { get; set; }
    public string? workingHourType { get; set; }
}

public class CreateAlbumResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? galleryid { get; set; }
}

public class DeletePhotoResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
}
