namespace TouchBase.API.Models.DTOs.WebLink;

public class WebLinkListRequest
{
    public string? GroupId { get; set; }
    public string? searchText { get; set; }
}

public class WebLinkListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public WebLinkResultWrapper? TBGetWebLinkListResult { get; set; }
}

public class WebLinkResultWrapper
{
    public List<WebLinkItemDto>? WebLinkListResult { get; set; }
}

public class WebLinkItemDto
{
    public string? weblinkId { get; set; }
    public string? groupId { get; set; }
    public string? title { get; set; }
    public string? fullDesc { get; set; }
    public string? linkUrl { get; set; }
}
