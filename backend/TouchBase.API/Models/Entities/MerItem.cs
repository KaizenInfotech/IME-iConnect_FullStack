namespace TouchBase.API.Models.Entities;

public class MerItem
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? FilePath { get; set; }
    public string? PublishDate { get; set; }
    public string? ExpiryDate { get; set; }
    public string? FinanceYear { get; set; }
    public string? TransType { get; set; } // 1=MER, 2=iMélange
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
