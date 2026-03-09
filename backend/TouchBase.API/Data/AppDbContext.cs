using Microsoft.EntityFrameworkCore;
using TouchBase.API.Models.Entities;

namespace TouchBase.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Auth
    public DbSet<User> Users => Set<User>();
    public DbSet<Models.Entities.DeviceToken> DeviceTokens => Set<Models.Entities.DeviceToken>();

    // Members
    public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();
    public DbSet<FamilyMember> FamilyMembers => Set<FamilyMember>();
    public DbSet<AddressDetail> AddressDetails => Set<AddressDetail>();

    // Groups
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<GroupModule> GroupModules => Set<GroupModule>();
    public DbSet<SubGroup> SubGroups => Set<SubGroup>();
    public DbSet<SubGroupMember> SubGroupMembers => Set<SubGroupMember>();

    // Events
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventResponse> EventResponses => Set<EventResponse>();

    // Announcements
    public DbSet<Announcement> Announcements => Set<Announcement>();

    // Documents
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentReadStatus> DocumentReadStatuses => Set<DocumentReadStatus>();

    // Ebulletins
    public DbSet<Ebulletin> Ebulletins => Set<Ebulletin>();

    // Gallery
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<AlbumPhoto> AlbumPhotos => Set<AlbumPhoto>();

    // Attendance
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<AttendanceMember> AttendanceMembers => Set<AttendanceMember>();
    public DbSet<AttendanceVisitor> AttendanceVisitors => Set<AttendanceVisitor>();

    // Service Directory
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<ServiceDirectoryEntry> ServiceDirectoryEntries => Set<ServiceDirectoryEntry>();

    // Settings
    public DbSet<TouchbaseSetting> TouchbaseSettings => Set<TouchbaseSetting>();
    public DbSet<GroupSetting> GroupSettings => Set<GroupSetting>();

    // Clubs & Districts
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<District> Districts => Set<District>();

    // Web Links
    public DbSet<WebLink> WebLinks => Set<WebLink>();

    // Past Presidents
    public DbSet<PastPresident> PastPresidents => Set<PastPresident>();

    // Banners
    public DbSet<Banner> Banners => Set<Banner>();

    // Notifications
    public DbSet<Notification> Notifications => Set<Notification>();

    // Zones & Chapters
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Chapter> Chapters => Set<Chapter>();

    // Leaderboard
    public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();

    // MER
    public DbSet<MerItem> MerItems => Set<MerItem>();

    // Popups
    public DbSet<Popup> Popups => Set<Popup>();
    public DbSet<PopupStatus> PopupStatuses => Set<PopupStatus>();

    // Countries & Categories
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Category> Categories => Set<Category>();

    // Feedback
    public DbSet<Feedback> Feedbacks => Set<Feedback>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ─── User ───
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasIndex(u => u.MobileNo).IsUnique();
            e.HasIndex(u => u.Email);
        });

        // ─── MemberProfile ───
        modelBuilder.Entity<MemberProfile>(e =>
        {
            e.ToTable("member_profiles");
            e.HasOne(mp => mp.User)
             .WithMany(u => u.MemberProfiles)
             .HasForeignKey(mp => mp.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(mp => mp.UserId);
        });

        // ─── FamilyMember ───
        modelBuilder.Entity<FamilyMember>(e =>
        {
            e.ToTable("family_members");
            e.HasOne(fm => fm.MemberProfile)
             .WithMany(mp => mp.FamilyMembers)
             .HasForeignKey(fm => fm.MemberProfileId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── AddressDetail ───
        modelBuilder.Entity<AddressDetail>(e =>
        {
            e.ToTable("address_details");
            e.HasOne(a => a.MemberProfile)
             .WithMany(mp => mp.Addresses)
             .HasForeignKey(a => a.MemberProfileId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Group ───
        modelBuilder.Entity<Group>(e =>
        {
            e.ToTable("groups");
            e.HasIndex(g => g.GrpName);
            e.HasOne(g => g.District)
             .WithMany(d => d.Groups)
             .HasForeignKey(g => g.DistrictId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ─── GroupMember ───
        modelBuilder.Entity<GroupMember>(e =>
        {
            e.ToTable("group_members");
            e.HasOne(gm => gm.Group)
             .WithMany(g => g.Members)
             .HasForeignKey(gm => gm.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(gm => gm.MemberProfile)
             .WithMany(mp => mp.GroupMemberships)
             .HasForeignKey(gm => gm.MemberProfileId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(gm => new { gm.GroupId, gm.MemberProfileId }).IsUnique();
        });

        // ─── GroupModule ───
        modelBuilder.Entity<GroupModule>(e =>
        {
            e.ToTable("group_modules");
            e.HasOne(gm => gm.Group)
             .WithMany(g => g.Modules)
             .HasForeignKey(gm => gm.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── SubGroup ───
        modelBuilder.Entity<SubGroup>(e =>
        {
            e.ToTable("sub_groups");
            e.HasOne(sg => sg.Group)
             .WithMany(g => g.SubGroups)
             .HasForeignKey(sg => sg.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SubGroupMember>(e =>
        {
            e.ToTable("sub_group_members");
            e.HasOne(sgm => sgm.SubGroup)
             .WithMany(sg => sg.Members)
             .HasForeignKey(sgm => sgm.SubGroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Event ───
        modelBuilder.Entity<Event>(e =>
        {
            e.ToTable("events");
            e.HasOne(ev => ev.Group)
             .WithMany(g => g.Events)
             .HasForeignKey(ev => ev.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(ev => ev.GroupId);
            e.HasIndex(ev => ev.EventDate);
        });

        // ─── EventResponse ───
        modelBuilder.Entity<EventResponse>(e =>
        {
            e.ToTable("event_responses");
            e.HasOne(er => er.Event)
             .WithMany(ev => ev.Responses)
             .HasForeignKey(er => er.EventId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(er => er.MemberProfile)
             .WithMany(mp => mp.EventResponses)
             .HasForeignKey(er => er.MemberProfileId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(er => new { er.EventId, er.MemberProfileId }).IsUnique();
        });

        // ─── Announcement ───
        modelBuilder.Entity<Announcement>(e =>
        {
            e.ToTable("announcements");
            e.HasOne(a => a.Group)
             .WithMany(g => g.Announcements)
             .HasForeignKey(a => a.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Document ───
        modelBuilder.Entity<Document>(e =>
        {
            e.ToTable("documents");
            e.HasOne(d => d.Group)
             .WithMany(g => g.Documents)
             .HasForeignKey(d => d.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DocumentReadStatus>(e =>
        {
            e.ToTable("document_read_statuses");
            e.HasOne(drs => drs.Document)
             .WithMany(d => d.ReadStatuses)
             .HasForeignKey(drs => drs.DocumentId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(drs => drs.MemberProfile)
             .WithMany(mp => mp.DocumentReadStatuses)
             .HasForeignKey(drs => drs.MemberProfileId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(drs => new { drs.DocumentId, drs.MemberProfileId }).IsUnique();
        });

        // ─── Ebulletin ───
        modelBuilder.Entity<Ebulletin>(e =>
        {
            e.ToTable("ebulletins");
            e.HasOne(eb => eb.Group)
             .WithMany(g => g.Ebulletins)
             .HasForeignKey(eb => eb.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Album ───
        modelBuilder.Entity<Album>(e =>
        {
            e.ToTable("albums");
            e.HasOne(a => a.Group)
             .WithMany(g => g.Albums)
             .HasForeignKey(a => a.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AlbumPhoto>(e =>
        {
            e.ToTable("album_photos");
            e.HasOne(ap => ap.Album)
             .WithMany(a => a.Photos)
             .HasForeignKey(ap => ap.AlbumId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Attendance ───
        modelBuilder.Entity<AttendanceRecord>(e =>
        {
            e.ToTable("attendance_records");
            e.HasOne(ar => ar.Group)
             .WithMany(g => g.AttendanceRecords)
             .HasForeignKey(ar => ar.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AttendanceMember>(e =>
        {
            e.ToTable("attendance_members");
            e.HasOne(am => am.AttendanceRecord)
             .WithMany(ar => ar.AttendanceMembers)
             .HasForeignKey(am => am.AttendanceRecordId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AttendanceVisitor>(e =>
        {
            e.ToTable("attendance_visitors");
            e.HasOne(av => av.AttendanceRecord)
             .WithMany(ar => ar.AttendanceVisitors)
             .HasForeignKey(av => av.AttendanceRecordId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Service Directory ───
        modelBuilder.Entity<ServiceCategory>(e =>
        {
            e.ToTable("service_categories");
        });

        modelBuilder.Entity<ServiceDirectoryEntry>(e =>
        {
            e.ToTable("service_directory_entries");
            e.HasOne(sde => sde.Group)
             .WithMany(g => g.ServiceDirectoryEntries)
             .HasForeignKey(sde => sde.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(sde => sde.ServiceCategory)
             .WithMany(sc => sc.Entries)
             .HasForeignKey(sde => sde.ServiceCategoryId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ─── Settings ───
        modelBuilder.Entity<TouchbaseSetting>(e =>
        {
            e.ToTable("touchbase_settings");
            e.HasOne(ts => ts.User)
             .WithMany(u => u.TouchbaseSettings)
             .HasForeignKey(ts => ts.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GroupSetting>(e =>
        {
            e.ToTable("group_settings");
            e.HasOne(gs => gs.Group)
             .WithMany(g => g.GroupSettings)
             .HasForeignKey(gs => gs.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Club ───
        modelBuilder.Entity<Club>(e =>
        {
            e.ToTable("clubs");
            e.HasIndex(c => c.ClubName);
            e.HasOne(c => c.Group)
             .WithOne()
             .HasForeignKey<Club>(c => c.GroupId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ─── District ───
        modelBuilder.Entity<District>(e =>
        {
            e.ToTable("districts");
        });

        // ─── WebLink ───
        modelBuilder.Entity<WebLink>(e =>
        {
            e.ToTable("web_links");
            e.HasOne(wl => wl.Group)
             .WithMany(g => g.WebLinks)
             .HasForeignKey(wl => wl.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── PastPresident ───
        modelBuilder.Entity<PastPresident>(e =>
        {
            e.ToTable("past_presidents");
            e.HasOne(pp => pp.Group)
             .WithMany(g => g.PastPresidents)
             .HasForeignKey(pp => pp.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Banner ───
        modelBuilder.Entity<Banner>(e =>
        {
            e.ToTable("banners");
            e.HasOne(b => b.Group)
             .WithMany(g => g.Banners)
             .HasForeignKey(b => b.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Notification ───
        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("notifications");
            e.HasOne(n => n.User)
             .WithMany(u => u.Notifications)
             .HasForeignKey(n => n.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Zone & Chapter ───
        modelBuilder.Entity<Zone>(e =>
        {
            e.ToTable("zones");
        });

        modelBuilder.Entity<Chapter>(e =>
        {
            e.ToTable("chapters");
            e.HasOne(c => c.Zone)
             .WithMany(z => z.Chapters)
             .HasForeignKey(c => c.ZoneId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── LeaderboardEntry ───
        modelBuilder.Entity<LeaderboardEntry>(e =>
        {
            e.ToTable("leaderboard_entries");
            e.HasOne(le => le.Group)
             .WithMany(g => g.LeaderboardEntries)
             .HasForeignKey(le => le.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(le => le.Zone)
             .WithMany(z => z.LeaderboardEntries)
             .HasForeignKey(le => le.ZoneId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ─── MerItem ───
        modelBuilder.Entity<MerItem>(e =>
        {
            e.ToTable("mer_items");
            e.HasOne(m => m.Group)
             .WithMany(g => g.MerItems)
             .HasForeignKey(m => m.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Popup ───
        modelBuilder.Entity<Popup>(e =>
        {
            e.ToTable("popups");
            e.HasOne(p => p.Group)
             .WithMany(g => g.Popups)
             .HasForeignKey(p => p.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PopupStatus>(e =>
        {
            e.ToTable("popup_statuses");
            e.HasOne(ps => ps.Popup)
             .WithMany(p => p.Statuses)
             .HasForeignKey(ps => ps.PopupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Country & Category ───
        modelBuilder.Entity<Country>(e =>
        {
            e.ToTable("countries");
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("categories");
        });

        // ─── Feedback ───
        modelBuilder.Entity<Feedback>(e =>
        {
            e.ToTable("feedbacks");
            e.HasOne(f => f.Group)
             .WithMany(g => g.Feedbacks)
             .HasForeignKey(f => f.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── DeviceToken ───
        modelBuilder.Entity<Models.Entities.DeviceToken>(e =>
        {
            e.ToTable("device_tokens");
            e.HasOne(dt => dt.User)
             .WithMany(u => u.DeviceTokens)
             .HasForeignKey(dt => dt.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
