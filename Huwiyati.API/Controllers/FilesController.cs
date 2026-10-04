namespace Huwiyati.API.Controllers;

using Huwiyati.Application.Common;
using Huwiyati.Application.Files.Commands;
using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller providing file management endpoints (photo uploads for profile and document features).
/// </summary>
[ApiController]
[Route("api/v1/files")]
[Authorize(Roles = $"{AppRoles.Citizen},{AppRoles.Employee},{AppRoles.Admin}")]
public class FilesController : ControllerBase
{
    private readonly UploadPhotoHandler _uploadPhotoHandler;

    public FilesController(UploadPhotoHandler uploadPhotoHandler)
    {
        _uploadPhotoHandler = uploadPhotoHandler;
    }

    /// <summary>
    /// Step 1: Upload a citizen profile photo (Allowed: JPG, JPEG, PNG, WEBP - Max: 3MB)
    /// Saved under wwwroot/uploads/photos/
    /// </summary>
    /// <param name="photo">The image file uploaded via multipart/form-data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Relative URL path of the uploaded photo.</returns>
    [HttpPost("upload-photo")]
    public async Task<IActionResult> UploadPhoto(IFormFile photo, CancellationToken cancellationToken)
    {
        if (photo == null || photo.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Failure("No image file was uploaded.", statusCode: 400));
        }

        using var stream = photo.OpenReadStream();
        var result = await _uploadPhotoHandler.HandleAsync(stream, photo.FileName, FileCategory.ProfilePhoto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Step 2: Upload a document scan image (e.g. Marriage Contract document scan) (Allowed: JPG, JPEG, PNG, WEBP - Max: 3MB)
    /// Saved under wwwroot/uploads/documents/
    /// </summary>
    /// <param name="document">The document scan file uploaded via multipart/form-data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Relative URL path of the uploaded document file.</returns>
    [HttpPost("upload-document")]
    public async Task<IActionResult> UploadDocument(IFormFile document, CancellationToken cancellationToken)
    {
        if (document == null || document.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Failure("No document scan file was uploaded.", statusCode: 400));
        }

        using var stream = document.OpenReadStream();
        var result = await _uploadPhotoHandler.HandleAsync(stream, document.FileName, FileCategory.DocumentScan, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
