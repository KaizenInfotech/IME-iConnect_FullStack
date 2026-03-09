namespace TouchBase.API.Models.DTOs.PastPresident;

public class PastPresidentRequest
{
    public string? GroupId { get; set; }
    public string? SearchText { get; set; }
    public string? updateOn { get; set; }
}

public class PastPresidentResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public PastPresidentListWrapper? TBPastPresidentList { get; set; }
}

public class PastPresidentListWrapper
{
    public List<PastPresidentDto>? newRecords { get; set; }
}

public class PastPresidentDto
{
    public string? PastPresidentId { get; set; }
    public string? MemberName { get; set; }
    public string? PhotoPath { get; set; }
    public string? TenureYear { get; set; }
    public string? designation { get; set; }
}
