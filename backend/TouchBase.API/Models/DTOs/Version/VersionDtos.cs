namespace TouchBase.API.Models.DTOs.Version;

public class VersionListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? latestVersion { get; set; }
    public string? forceUpdate { get; set; }
    public string? storeUrl { get; set; }
}
