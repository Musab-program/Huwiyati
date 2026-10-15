namespace Huwiyati.Application.Files.Commands;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Enums;

/// <summary>
/// Application handler responsible for receiving uploaded photo streams and returning relative photo URL.
/// </summary>
public class UploadPhotoHandler
{
    private readonly IFileStorageService _fileStorageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadPhotoHandler"/> class.
    /// </summary>
    /// <param name="fileStorageService">The file storage service used to persist incoming file streams.</param>
    public UploadPhotoHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    /// <summary>
    /// Handles the upload of an image or document stream and persists it into storage.
    /// </summary>
    /// <param name="fileStream">The readable stream containing binary file content.</param>
    /// <param name="fileName">The original name or desired filename including extension.</param>
    /// <param name="category">The file category classifying the target storage path (defaults to <see cref="FileCategory.ProfilePhoto"/>).</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the relative file storage path on success.</returns>
    public async Task<ApiResponse<string>> HandleAsync(Stream fileStream, string fileName, FileCategory category = FileCategory.ProfilePhoto, CancellationToken cancellationToken = default)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            return ApiResponse<string>.Failure("No image file stream was provided.", statusCode: 400);
        }

        var uploadResult = await _fileStorageService.SaveFileAsync(fileStream, fileName, category, cancellationToken);

        if (!uploadResult.IsSuccess)
        {
            return ApiResponse<string>.Failure(uploadResult.ErrorMessage!, statusCode: uploadResult.StatusCode);
        }

        return ApiResponse<string>.Success(uploadResult.FilePath!, message: "File uploaded successfully.", statusCode: 200);
    }
}
