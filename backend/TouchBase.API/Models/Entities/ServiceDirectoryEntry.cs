namespace TouchBase.API.Models.Entities;

public class ServiceCategory
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<ServiceDirectoryEntry> Entries { get; set; } = new List<ServiceDirectoryEntry>();
}

public class ServiceDirectoryEntry
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int? ServiceCategoryId { get; set; }
    public string? MemberName { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? ContactNo { get; set; }
    public string? ContactNo2 { get; set; }
    public string? CountryCode1 { get; set; }
    public string? CountryCode2 { get; set; }
    public string? PaxNo { get; set; }
    public string? Email { get; set; }
    public string? Keywords { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Zip { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Website { get; set; }
    public string? ModuleId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public ServiceCategory? ServiceCategory { get; set; }
}
