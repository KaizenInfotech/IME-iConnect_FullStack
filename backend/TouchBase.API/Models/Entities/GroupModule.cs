namespace TouchBase.API.Models.Entities;

public class GroupModule
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public string? ModuleStaticRef { get; set; }
    public string? Image { get; set; }
    public string? MasterProfileId { get; set; }
    public string? IsCustomized { get; set; }
    public string? ModuleOrderNo { get; set; }
    public string? NotificationCount { get; set; }
    public string? ModulePriceRs { get; set; }
    public string? ModulePriceUS { get; set; }
    public string? ModuleInfo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
