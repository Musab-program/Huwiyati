namespace Huwiyati.Application.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Operation completed successfully.", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failure(string message, List<string>? errors = null, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
