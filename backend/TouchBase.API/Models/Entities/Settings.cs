namespace TouchBase.API.Models.Entities;

public class TouchbaseSetting
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? GrpId { get; set; }
    public string? GrpVal { get; set; }
    public string? GrpName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}

public class GroupSetting
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int MemberProfileId { get; set; }
    public string? ModuleId { get; set; }
    public string? ModVal { get; set; }
    public string? ModName { get; set; }
    public string? IsMob { get; set; }
    public string? IsEmail { get; set; }
    public string? IsPersonal { get; set; }
    public string? IsFamily { get; set; }
    public string? IsBusiness { get; set; }
    public string? ShowMobileSelfClub { get; set; }
    public string? ShowMobileOutsideClub { get; set; }
    public string? ShowEmailSelfClub { get; set; }
    public string? ShowEmailOutsideClub { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Group Group { get; set; } = null!;
}
