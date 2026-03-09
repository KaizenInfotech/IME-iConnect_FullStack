namespace TouchBase.API.Models.DTOs.FindRotarian;

public class RotarianSearchRequest
{
    public string? name { get; set; }
    public string? Grade { get; set; }
    public string? memberMobile { get; set; }
    public string? club { get; set; }
    public string? Category { get; set; }
}

public class RotarianDetailRequest
{
    public string? memberProfileId { get; set; }
}

public class ZoneChapterResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public ZoneChapterResult? ZoneChapterResult { get; set; }
}

public class ZoneChapterResult
{
    public List<ZoneItemDto>? Table { get; set; }
    public List<ChapterItemDto>? Table1 { get; set; }
}

public class ZoneItemDto
{
    public string? ZoneID { get; set; }
    public string? ZoneName { get; set; }
}

public class ChapterItemDto
{
    public string? ChapterID { get; set; }
    public string? ChapterName { get; set; }
    public string? ZoneID { get; set; }
}

public class RotarianSearchResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<RotarianItemDto>? RotarianResult { get; set; }
}

public class RotarianItemDto
{
    public string? masterUID { get; set; }
    public string? groupID { get; set; }
    public string? profileID { get; set; }
    public string? clubName { get; set; }
    public string? memberMobile { get; set; }
    public string? member_Name { get; set; }
    public string? mem_Category { get; set; }
    public string? Grade { get; set; }
    public string? pic { get; set; }
}

public class RotarianDetailResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public RotarianDetailResult? Result { get; set; }
}

public class RotarianDetailResult
{
    public List<RotarianDetailDto>? Table { get; set; }
}

public class RotarianDetailDto
{
    public string? memberName { get; set; }
    public string? pic { get; set; }
    public string? masterUID { get; set; }
    public string? memberMobile { get; set; }
    public string? secondaryMobile { get; set; }
    public string? memberEmail { get; set; }
    public string? email { get; set; }
    public string? businessAddress { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? country { get; set; }
    public string? pincode { get; set; }
    public string? businessName { get; set; }
    public string? designation { get; set; }
    public string? phoneNo { get; set; }
    public string? fax { get; set; }
    public string? classification { get; set; }
    public string? clubName { get; set; }
    public string? clubDesignation { get; set; }
    public string? bloodGroup { get; set; }
    public string? donorRecognition { get; set; }
    public string? keywords { get; set; }
    public string? chaptrBrnchName { get; set; }
    public string? membershipNo { get; set; }
    public string? membershipGrade { get; set; }
    public string? categoryName { get; set; }
    public string? dob { get; set; }
    public string? doa { get; set; }
    public string? anniversary { get; set; }
    public string? whatsappNum { get; set; }
    public string? hideWhatsnum { get; set; }
    public string? hideMail { get; set; }
    public string? hideNum { get; set; }
    public string? address { get; set; }
    public string? companyName { get; set; }
}

public class CategoryListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<CategoryItemDto>? Result { get; set; }
}

public class CategoryItemDto
{
    public string? id { get; set; }
    public string? name { get; set; }
}

public class GradeListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<GradeItemDto>? Result { get; set; }
}

public class GradeItemDto
{
    public string? id { get; set; }
    public string? name { get; set; }
}

public class ClubListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public List<ClubItemDto>? Result { get; set; }
}

public class ClubItemDto
{
    public string? id { get; set; }
    public string? name { get; set; }
}
