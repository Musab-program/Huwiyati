namespace Huwiyati.Application.Common;

/// <summary>
/// Represents a standardized unified API response envelope for all application operations.
/// </summary>
/// <typeparam name="T">The type of data returned in the payload.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the HTTP response status code.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the human-readable result message or description.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payload data if the operation was successful.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the list of error messages if the operation failed.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Creates a successful <see cref="ApiResponse{T}"/> envelope with the provided payload and status.
    /// </summary>
    /// <param name="data">The operation data payload.</param>
    /// <param name="message">An optional informative message.</param>
    /// <param name="statusCode">The HTTP status code (defaults to 200 OK).</param>
    /// <returns>A new populated success response instance.</returns>
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

    /// <summary>
    /// Creates a failure <see cref="ApiResponse{T}"/> envelope with the provided error message and status.
    /// </summary>
    /// <param name="message">The failure summary message.</param>
    /// <param name="errors">An optional collection of granular validation or business error messages.</param>
    /// <param name="statusCode">The HTTP status code (defaults to 400 Bad Request).</param>
    /// <returns>A new populated failure response instance.</returns>
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
