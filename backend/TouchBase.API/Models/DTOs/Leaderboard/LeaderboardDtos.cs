namespace TouchBase.API.Models.DTOs.Leaderboard;

public class LeaderboardRequest
{
    public string? GroupID { get; set; }
    public string? RowYear { get; set; }
    public string? ProfileID { get; set; }
    public string? fk_zoneid { get; set; }
}

public class LeaderboardResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? MembersCount { get; set; }
    public string? TRFCount { get; set; }
    public string? TotalProjects { get; set; }
    public string? ProjectCost { get; set; }
    public string? ManHoursCount { get; set; }
    public string? BeneficiaryCount { get; set; }
    public string? RotariansCount { get; set; }
    public string? RotaractoresCount { get; set; }
    public List<LeaderBoardEntryDto>? leaderBoardResult { get; set; }
}

public class LeaderBoardEntryDto
{
    public string? clubName { get; set; }
    public string? Points { get; set; }
}

public class ZoneListRequest
{
    public string? grpId { get; set; }
}
