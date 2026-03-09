namespace TouchBase.API.Models.DTOs.Common;

public class ApiResponse<T>
{
    public string status { get; set; } = "0";
    public string message { get; set; } = "success";
    public T? data { get; set; }
    public string? serverError { get; set; }
    public string? ErrorName { get; set; }

    public static ApiResponse<T> Success(T? data, string msg = "success") =>
        new() { status = "0", message = msg, data = data };

    public static ApiResponse<T> Fail(string msg, string? error = null) =>
        new() { status = "1", message = msg, serverError = error };
}

public class ApiResponse
{
    public string status { get; set; } = "0";
    public string message { get; set; } = "success";
    public object? data { get; set; }
    public string? serverError { get; set; }
    public string? ErrorName { get; set; }

    public static ApiResponse Success(object? data = null, string msg = "success") =>
        new() { status = "0", message = msg, data = data };

    public static ApiResponse Fail(string msg, string? error = null) =>
        new() { status = "1", message = msg, serverError = error };
}
