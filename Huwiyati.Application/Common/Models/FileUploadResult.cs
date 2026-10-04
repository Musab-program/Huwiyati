namespace Huwiyati.Application.Common.Models;

/// <summary>
/// Result object representing the outcome of a file storage operation without throwing exceptions.
/// </summary>
public class FileUploadResult
{
    public bool IsSuccess { get; set; }
    public string? FilePath { get; set; }
    public string? ErrorMessage { get; set; }
    public int StatusCode { get; set; } = 400;
}
