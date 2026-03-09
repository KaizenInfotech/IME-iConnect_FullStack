namespace TouchBase.API.Models.Entities;

public class LeaderboardEntry
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int? ZoneId { get; set; }
    public string? ClubName { get; set; }
    public string? Points { get; set; }
    public string? Year { get; set; }
    public string? MembersCount { get; set; }
    public string? TrfCount { get; set; }
    public string? TotalProjects { get; set; }
    public string? ProjectCost { get; set; }
    public string? ManHoursCount { get; set; }
    public string? BeneficiaryCount { get; set; }
    public string? RotariansCount { get; set; }
    public string? RotaractorsCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public Zone? Zone { get; set; }
}
