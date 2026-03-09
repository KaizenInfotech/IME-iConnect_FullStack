namespace TouchBase.API.Models.DTOs.Member;

// ─── Requests ───

public class DirectoryListRequest
{
    public string? masterUID { get; set; }
    public string? grpID { get; set; }
    public string? searchText { get; set; }
    public string? page { get; set; }
}

public class MemberDetailRequest
{
    public string? memberProfileId { get; set; }
    public string? memberProfID { get; set; }
    public string? groupId { get; set; }
    public string? grpID { get; set; }
}

public class UpdateProfileRequest
{
    public string? ProfileId { get; set; }
    public string? memberMobile { get; set; }
    public string? memberName { get; set; }
    public string? memberEmailid { get; set; }
    public string? ProfilePicPath { get; set; }
    public string? ImageId { get; set; }
}

public class UpdatePersonalDetailsRequest
{
    public string? profileID { get; set; }
    public string? key { get; set; } // JSON array string
}

public class UpdateAddressRequest
{
    public string? addressID { get; set; }
    public string? addressType { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? country { get; set; }
    public string? pincode { get; set; }
    public string? phoneNo { get; set; }
    public string? fax { get; set; }
    public string? profileID { get; set; }
    public string? groupID { get; set; }
}

public class UpdateFamilyRequest
{
    public string? familyMemberId { get; set; }
    public string? memberName { get; set; }
    public string? relationship { get; set; }
    public string? dOB { get; set; }
    public string? anniversary { get; set; }
    public string? contactNo { get; set; }
    public string? particulars { get; set; }
    public string? profileId { get; set; }
    public string? emailID { get; set; }
    public string? bloodGroup { get; set; }
}

public class BodListRequest
{
    public string? grpId { get; set; }
    public string? searchText { get; set; }
    public string? YearFilter { get; set; }
}

public class MemberSyncRequest
{
    public string? grpID { get; set; }
    public string? updatedOn { get; set; }
}

public class UpdateMemberRequest
{
    public string? mobile_num_hide { get; set; }
    public string? secondary_num_hide { get; set; }
    public string? email_hide { get; set; }
    public string? DOB { get; set; }
    public string? DOA { get; set; }
    public string? company_name { get; set; }
    public string? MemProfileId { get; set; }
}

public class SaveProfileRequest
{
    public string? MemberId { get; set; }
    public string? remark { get; set; }
    public string? Category { get; set; }
}

public class ProfilePhotoRequest
{
    public string? ProfileID { get; set; }
    public string? GrpID { get; set; }
    public string? Type { get; set; }
}

public class GetUpdatedProfileRequest
{
    public string? profileId { get; set; }
    public string? groupId { get; set; }
}

public class GoverningCouncilRequest
{
    public string? searchText { get; set; }
    public string? YearFilter { get; set; }
}

// ─── Responses ───

public class DirectoryListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? resultCount { get; set; }
    public string? TotalPages { get; set; }
    public string? currentPage { get; set; }
    public List<MemberListItemDto>? MemberListResults { get; set; }
}

public class MemberListItemDto
{
    public string? masterID { get; set; }
    public string? grpID { get; set; }
    public string? profileID { get; set; }
    public string? isAdmin { get; set; }
    public string? memberName { get; set; }
    public string? memberEmail { get; set; }
    public string? memberMobile { get; set; }
    public string? memberCountry { get; set; }
    public string? profilePic { get; set; }
    public string? familyPic { get; set; }
    public string? designation { get; set; }
    public string? companyName { get; set; }
    public string? bloodGroup { get; set; }
    public string? countryCode { get; set; }
}

public class MemberDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<MemberDetailDto>? MemberDetails { get; set; }
}

public class MemberDetailDto
{
    public string? masterID { get; set; }
    public string? grpID { get; set; }
    public string? profileID { get; set; }
    public string? isAdmin { get; set; }
    public string? memberName { get; set; }
    public string? memberEmail { get; set; }
    public string? memberMobile { get; set; }
    public string? memberCountry { get; set; }
    public string? profilePic { get; set; }
    public string? isPersonalDetVisible { get; set; }
    public string? isBusinDetVisible { get; set; }
    public string? isFamilDetailVisible { get; set; }
    public List<PersonalMemberDetailDto>? personalMemberDetails { get; set; }
    public List<PersonalMemberDetailDto>? businessMemberDetails { get; set; }
    public List<FamilyMemberDetailDto>? familyMemberDetails { get; set; }
    public List<AddressDetailDto>? addressDetails { get; set; }
}

public class PersonalMemberDetailDto
{
    public string? key { get; set; }
    public string? value { get; set; }
    public string? uniquekey { get; set; }
    public string? colType { get; set; }
}

public class FamilyMemberDetailDto
{
    public string? familyMemberId { get; set; }
    public string? memberName { get; set; }
    public string? relationship { get; set; }
    public string? dOB { get; set; }
    public string? emailID { get; set; }
    public string? anniversary { get; set; }
    public string? contactNo { get; set; }
    public string? particulars { get; set; }
    public string? bloodGroup { get; set; }
}

public class AddressDetailDto
{
    public string? addressID { get; set; }
    public string? addressType { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? country { get; set; }
    public string? pincode { get; set; }
    public string? phoneNo { get; set; }
    public string? fax { get; set; }
}

public class BodListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<BodMemberDto>? BODResult { get; set; }
}

public class BodMemberDto
{
    public string? BOD_pkID { get; set; }
    public string? ProfileID { get; set; }
    public string? MemberName { get; set; }
    public string? MemberDesignation { get; set; }
    public string? pic { get; set; }
    public string? masterUID { get; set; }
    public string? grpID { get; set; }
    public string? membermobile { get; set; }
    public string? Email { get; set; }
    public string? clubName { get; set; }
    public string? YearFilter { get; set; }
}

public class GoverningCouncilResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public GoverningCouncilResult? Result { get; set; }
}

public class GoverningCouncilResult
{
    public List<CouncilMemberDto>? Table { get; set; }
}

public class CouncilMemberDto
{
    public string? BOD_pkID { get; set; }
    public string? ProfileID { get; set; }
    public string? TempMemberMasterID { get; set; }
    public string? FK_Master_Designation_ID { get; set; }
    public string? PhoneNo { get; set; }
    public string? Email { get; set; }
    public string? YearFilter { get; set; }
    public string? MemberName { get; set; }
    public string? masterUID { get; set; }
    public string? sr_NO { get; set; }
    public string? grpID { get; set; }
    public string? pic { get; set; }
    public string? MemberDesignation { get; set; }
    public string? membermobile { get; set; }
}

public class UpdateResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
}
