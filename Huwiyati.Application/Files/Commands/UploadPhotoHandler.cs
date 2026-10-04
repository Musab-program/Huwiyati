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

    public UploadPhotoHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    /// <summary>
    /// Step 1: Handle photo or document image stream upload and return ApiResponse with relative URL
    /// </summary>
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
