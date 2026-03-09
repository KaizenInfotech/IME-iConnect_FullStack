namespace TouchBase.API.Models.DTOs.Document;

// ─── Requests ───

public class DocumentListRequest
{
    public string? grpID { get; set; }
    public string? memberProfileID { get; set; }
    public string? type { get; set; }
    public string? isAdmin { get; set; }
    public string? searchText { get; set; }
}

public class UpdateDocReadRequest
{
    public string? DocID { get; set; }
    public string? memberProfileID { get; set; }
}

// ─── Responses ───

public class DocumentListResponse
{
    public string? status { get; set; }
    public string? message { get; set; }
    public string? smscount { get; set; }
    public List<DocumentItemDto>? DocumentLsitResult { get; set; } // Note: iOS typo "Lsit" preserved
}

public class DocumentItemDto
{
    public string? docID { get; set; }
    public string? docTitle { get; set; }
    public string? docType { get; set; }
    public string? docURL { get; set; }
    public string? createDateTime { get; set; }
    public string? docAccessType { get; set; }
    public string? isRead { get; set; }
}
