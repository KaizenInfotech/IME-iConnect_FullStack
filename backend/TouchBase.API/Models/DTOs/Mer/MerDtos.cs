namespace TouchBase.API.Models.DTOs.Mer;

public class YearRequest
{
    public string? Type { get; set; }
    public string? grpID { get; set; }
}

public class MerListRequest
{
    public string? FinanceYear { get; set; }
    public string? TransType { get; set; }
    public string? grpID { get; set; }
}

public class YearResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public YearResultWrapper? str { get; set; }
}

public class YearResultWrapper
{
    public List<YearItemDto>? Table { get; set; }
}

public class YearItemDto
{
    public string? FinanceYear { get; set; }
}

public class MerListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public MerResultWrapper? Result { get; set; }
}

public class MerResultWrapper
{
    public List<MerItemDto>? Table { get; set; }
}

public class MerItemDto
{
    public string? GrpID { get; set; }
    public string? MER_ID { get; set; }
    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? File_Path { get; set; }
    public string? publish_date { get; set; }
    public string? expiry_date { get; set; }
    public string? FinanceYear { get; set; }
}
