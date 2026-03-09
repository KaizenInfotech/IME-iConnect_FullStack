namespace TouchBase.API.Models.DTOs.Auth;

// ─── Requests ───

public class LoginRequest
{
    public string? mobileNo { get; set; }
    public string? deviceToken { get; set; }
    public string? countryCode { get; set; }
    public string? loginType { get; set; }
}

public class OtpVerifyRequest
{
    public string? mobileNo { get; set; }
    public string? deviceToken { get; set; }
    public string? countryCode { get; set; }
    public string? deviceName { get; set; }
    public string? imeiNo { get; set; }
    public string? versionNo { get; set; }
    public string? loginType { get; set; }
}

public class WelcomeScreenRequest
{
    public string? masterUID { get; set; }
    public string? mobileno { get; set; }
    public string? loginType { get; set; }
}

public class MemberDetailsRequest
{
    public string? masterUID { get; set; }
}

public class RegistrationRequest
{
    public string? mobileNo { get; set; }
    public string? countryCode { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? email { get; set; }
}

// ─── Responses (matching old API format for Flutter compatibility) ───

/// Top-level login/OTP response.
/// Flutter expects: { status, message, otp, isexists, masterUID, ds: { Table: [...] }, token }
public class LoginResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? otp { get; set; }
    public string? isexists { get; set; }
    public string? masterUID { get; set; }
    public string? isMemeberNotRegistered { get; set; }
    public LoginDs? ds { get; set; }
    public string? token { get; set; }
}

/// Flutter expects: { Table: [ { masterUID, grpid0, grpid1, ... } ] }
public class LoginDs
{
    public List<LoginTable>? Table { get; set; }
}

public class LoginTable
{
    public int? masterUID { get; set; }
    public int? grpid0 { get; set; }
    public string? grpid1 { get; set; }
    public string? GrpName { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? IMEI_Mem_Id { get; set; }
    public int? memberProfileId { get; set; }
    public string? profileImage { get; set; }
    public int? groupMasterID { get; set; }
}

/// Welcome screen response.
/// Flutter expects: { WelcomeResult: { status, Name, grpPartResults: [...] } }
/// or flat: { status, Name, grpPartResults: [...] }
public class WelcomeResponse
{
    public string? status { get; set; }
    public string? Name { get; set; }
    public List<WelcomeGroupItem>? grpPartResults { get; set; }
}

public class WelcomeGroupItem
{
    public int? Id { get; set; }
    public string? GrpName { get; set; }
    public string? GrpImg { get; set; }
    public string? GrpProfileId { get; set; }
    public string? MyCategory { get; set; }
    public string? IsGrpAdmin { get; set; }
}