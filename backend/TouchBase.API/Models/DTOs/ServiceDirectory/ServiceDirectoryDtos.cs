namespace TouchBase.API.Models.DTOs.ServiceDirectory;

public class ServiceCategoriesRequest
{
    public string? groupId { get; set; }
    public string? moduleID { get; set; }
}

public class ServiceDetailRequest
{
    public string? groupId { get; set; }
    public string? serviceDirId { get; set; }
}

public class AddServiceRequest
{
    public string? serviceId { get; set; }
    public string? groupId { get; set; }
    public string? memberName { get; set; }
    public string? description { get; set; }
    public string? image { get; set; }
    public string? countryCode1 { get; set; }
    public string? mobileNo1 { get; set; }
    public string? countryCode2 { get; set; }
    public string? mobileNo2 { get; set; }
    public string? paxNo { get; set; }
    public string? email { get; set; }
    public string? keywords { get; set; }
    public string? address { get; set; }
    public string? latitude { get; set; }
    public string? longitude { get; set; }
    public string? createdBy { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? addressCountry { get; set; }
    public string? zipcode { get; set; }
    public string? moduleId { get; set; }
    public string? website { get; set; }
}

public class ServiceCategoriesResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public ServiceCategoriesResult? Result { get; set; }
}

public class ServiceCategoriesResult
{
    public List<ServiceCategoryDto>? Category { get; set; }
    public List<ServiceDirectoryItemDto>? DirectoryData { get; set; }
}

public class ServiceCategoryDto
{
    public string? CategoryName { get; set; }
    public int? ID { get; set; }
    public int? TotalCount { get; set; }
}

public class ServiceDirectoryItemDto
{
    public string? serviceDirId { get; set; }
    public string? memberName { get; set; }
    public string? image { get; set; }
    public string? contactNo { get; set; }
    public string? contactNo2 { get; set; }
    public string? description { get; set; }
    public string? paxNo { get; set; }
    public string? email { get; set; }
    public string? keywords { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? country { get; set; }
    public string? zip { get; set; }
    public string? latitude { get; set; }
    public string? longitude { get; set; }
    public string? categoryId { get; set; }
    public string? website { get; set; }
    public string? moduleId { get; set; }
}
