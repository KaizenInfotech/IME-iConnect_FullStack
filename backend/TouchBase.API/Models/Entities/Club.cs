namespace TouchBase.API.Models.Entities;

public class Club
{
    public int Id { get; set; }
    public int? GroupId { get; set; }
    public string? ClubName { get; set; }
    public string? ClubType { get; set; }
    public string? ClubId { get; set; }
    public string? District { get; set; }
    public int? DistrictId { get; set; }
    public string? CharterDate { get; set; }
    public string? MeetingDay { get; set; }
    public string? MeetingTime { get; set; }
    public string? Website { get; set; }
    public string? Lat { get; set; }
    public string? Longi { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PresidentName { get; set; }
    public string? PresidentMobile { get; set; }
    public string? PresidentEmail { get; set; }
    public string? SecretaryName { get; set; }
    public string? SecretaryMobile { get; set; }
    public string? SecretaryEmail { get; set; }
    public string? GovernorName { get; set; }
    public string? GovernorMobile { get; set; }
    public string? GovernorEmail { get; set; }
    public string? Distance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group? Group { get; set; }
}
