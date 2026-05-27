namespace TouchBase.API.Models.Entities;

// One row per celebrant per day per type. The unique index on
// (CelebrationDate, MemberProfileId, Type) is what prevents duplicate
// pushes if the daily job re-runs (e.g. after a server restart).
public class CelebrationNotificationLog
{
    public int Id { get; set; }
    public DateTime CelebrationDate { get; set; }
    public int MemberProfileId { get; set; }
    public string Type { get; set; } = "";
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
