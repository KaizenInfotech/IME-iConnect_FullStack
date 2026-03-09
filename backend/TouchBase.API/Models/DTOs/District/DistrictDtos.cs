namespace TouchBase.API.Models.DTOs.District;

public class DistrictMemberListRequest
{
    public string? masterUID { get; set; }
    public string? grpID { get; set; }
    public string? searchText { get; set; }
    public string? pageNo { get; set; }
    public string? recordCount { get; set; }
}

public class ClassificationListRequest
{
    public string? grpID { get; set; }
    public string? pageNo { get; set; }
    public string? recordCount { get; set; }
    public string? searchText { get; set; }
}

public class MemberByClassificationRequest
{
    public string? classification { get; set; }
    public string? grpID { get; set; }
}

public class DistrictClubsRequest
{
    public string? groupId { get; set; }
    public string? search { get; set; }
}

public class DistrictMemberDetailRequest
{
    public string? memberProfileId { get; set; }
    public string? groupId { get; set; }
}

public class DistrictCommitteeRequest
{
    public string? groupId { get; set; }
}

// ─── Responses ───

public class DistrictMemberListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? resultCount { get; set; }
    public string? totalPages { get; set; }
    public string? currentPage { get; set; }
    public List<DistrictMemberDto>? Result { get; set; }
}

public class DistrictMemberDto
{
    public string? profileId { get; set; }
    public string? memberName { get; set; }
    public string? memberMobile { get; set; }
    public string? masterUID { get; set; }
    public string? pic { get; set; }
    public string? grpID { get; set; }
    public string? club_name { get; set; }
}

public class ClassificationListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<ClassificationItemDto>? Result { get; set; }
}

public class ClassificationItemDto
{
    public string? classification { get; set; }
    public int? count { get; set; }
}

public class DistrictClubListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<DistrictClubDto>? Clubs { get; set; }
}

public class DistrictClubDto
{
    public string? grpID { get; set; }
    public string? clubId { get; set; }
    public string? clubName { get; set; }
    public string? meetingDay { get; set; }
    public string? meetingTime { get; set; }
}

public class DistrictCommitteeResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<DistrictCommitteeMemberDto>? Result { get; set; }
}

public class DistrictCommitteeMemberDto
{
    public string? memberName { get; set; }
    public string? designation { get; set; }
    public string? pic { get; set; }
    public string? mobile { get; set; }
    public string? email { get; set; }
}
