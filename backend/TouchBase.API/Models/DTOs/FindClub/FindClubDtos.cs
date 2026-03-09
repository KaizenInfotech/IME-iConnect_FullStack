namespace TouchBase.API.Models.DTOs.FindClub;

public class ClubSearchRequest
{
    public string? keyword { get; set; }
    public string? country { get; set; }
    public string? stateProvinceCity { get; set; }
    public string? district { get; set; }
    public string? meetingDay { get; set; }
}

public class ClubNearMeRequest
{
    public string? lat { get; set; }
    public string? longi { get; set; }
}

public class ClubDetailRequest
{
    public string? grpId { get; set; }
}

public class ClubSearchResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<ClubItemDto>? ClubResult { get; set; }
}

public class ClubItemDto
{
    public string? GroupId { get; set; }
    public string? ClubName { get; set; }
    public string? ClubType { get; set; }
    public string? ClubId { get; set; }
    public string? District { get; set; }
    public string? CharterDate { get; set; }
    public string? MeetingDay { get; set; }
    public string? MeetingTime { get; set; }
    public string? Website { get; set; }
    public string? distance { get; set; }
}

public class ClubDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public ClubDetailDto? ClubDetailResult { get; set; }
}

public class ClubDetailDto
{
    public string? clubId { get; set; }
    public string? districtId { get; set; }
    public string? clubName { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? country { get; set; }
    public string? meetingDay { get; set; }
    public string? meetingTime { get; set; }
    public string? clubWebsite { get; set; }
    public string? lat { get; set; }
    public string? longi { get; set; }
    public string? presidentName { get; set; }
    public string? presidentMobile { get; set; }
    public string? presidentEmail { get; set; }
    public string? secretaryName { get; set; }
    public string? secretaryMobile { get; set; }
    public string? secretaryEmail { get; set; }
    public string? governorName { get; set; }
    public string? governorMobile { get; set; }
    public string? governorEmail { get; set; }
}

public class ClubMembersResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<ClubMemberDto>? Result { get; set; }
}

public class ClubMemberDto
{
    public string? memberName { get; set; }
    public string? designation { get; set; }
    public string? profileId { get; set; }
    public string? pic { get; set; }
    public string? memberMobile { get; set; }
}

public class CommitteeListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public CommitteeResult? ClubResult { get; set; }
}

public class CommitteeResult
{
    public List<CommitteeDto>? Table { get; set; }
    public List<CommitteeMemberDto>? Table1 { get; set; }
}

public class CommitteeDto
{
    public int? Pk_subcomittee_id { get; set; }
    public string? committeName { get; set; }
}

public class CommitteeMemberDto
{
    public int? Pk_subcomittee_id { get; set; }
    public string? committeName { get; set; }
    public string? membername { get; set; }
    public string? mobilenumber { get; set; }
    public string? EmailId { get; set; }
    public string? designation { get; set; }
    public string? branch { get; set; }
    public string? othermobilenumber { get; set; }
    public string? otheremailid { get; set; }
}
