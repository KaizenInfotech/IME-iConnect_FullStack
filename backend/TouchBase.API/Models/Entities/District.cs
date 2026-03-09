namespace TouchBase.API.Models.Entities;

public class District
{
    public int Id { get; set; }
    public string? DistrictName { get; set; }
    public string? DistrictNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Group> Groups { get; set; } = new List<Group>();
}
