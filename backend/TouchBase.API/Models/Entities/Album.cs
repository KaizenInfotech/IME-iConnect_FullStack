namespace TouchBase.API.Models.Entities;

public class Album
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Type { get; set; }
    public string? ModuleId { get; set; }
    public string? ShareType { get; set; }
    public string? CategoryId { get; set; }
    public string? AlbumCategoryText { get; set; }
    public string? OtherCategoryText { get; set; }
    public string? Beneficiary { get; set; }
    public string? CostOfProject { get; set; }
    public string? CostOfProjectType { get; set; }
    public string? WorkingHour { get; set; }
    public string? WorkingHourType { get; set; }
    public string? ProjectDate { get; set; }
    public string? NumberOfRotarian { get; set; }
    public string? MemberIds { get; set; }
    public string? SubgrpIds { get; set; }
    public string? IsSubGrpAdmin { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
    public ICollection<AlbumPhoto> Photos { get; set; } = new List<AlbumPhoto>();
}
