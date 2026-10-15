namespace Huwiyati.Application.Common.Models;

/// <summary>
/// Result object representing the outcome of a file storage operation without throwing exceptions.
/// </summary>
public class FileUploadResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the file upload succeeded.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the stored relative URL path of the saved file.
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// Gets or sets the error message if the upload failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the resulting HTTP status code.
    /// </summary>
    public int StatusCode { get; set; } = 400;
}
