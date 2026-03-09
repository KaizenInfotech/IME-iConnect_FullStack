namespace TouchBase.API.Models.Entities;

public class MemberProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? MemberName { get; set; }
    public string? MemberEmail { get; set; }
    public string? MemberMobile { get; set; }
    public string? MemberCountry { get; set; }
    public string? ProfilePic { get; set; }
    public string? FamilyPic { get; set; }
    public string? Designation { get; set; }
    public string? CompanyName { get; set; }
    public string? BloodGroup { get; set; }
    public string? CountryCode { get; set; }
    public string? Dob { get; set; }
    public string? Doa { get; set; }
    public string? SecondaryMobileNo { get; set; }
    public string? WhatsappNum { get; set; }
    public string? MembershipGrade { get; set; }
    public string? ImeiMembershipId { get; set; }
    public string? Classification { get; set; }
    public string? Category { get; set; }
    public string? CategoryId { get; set; }
    public string? HideNum { get; set; }
    public string? HideMail { get; set; }
    public string? HideWhatsnum { get; set; }
    public string? IsPersonalDetVisible { get; set; }
    public string? IsBusinDetVisible { get; set; }
    public string? IsFamilDetailVisible { get; set; }
    public string? MobileNumHide { get; set; }
    public string? SecondaryNumHide { get; set; }
    public string? EmailHide { get; set; }
    public string? DobHide { get; set; }
    public string? DoaHide { get; set; }
    public string? CompanyNameHide { get; set; }
    public string? Keywords { get; set; }
    public string? DonorRecognition { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
    public ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
    public ICollection<AddressDetail> Addresses { get; set; } = new List<AddressDetail>();
    public ICollection<EventResponse> EventResponses { get; set; } = new List<EventResponse>();
    public ICollection<DocumentReadStatus> DocumentReadStatuses { get; set; } = new List<DocumentReadStatus>();
}
