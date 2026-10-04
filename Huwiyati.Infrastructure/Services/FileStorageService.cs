namespace Huwiyati.Infrastructure.Services;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;
using Huwiyati.Domain.Enums;
using Microsoft.AspNetCore.Hosting;

/// <summary>
/// Infrastructure service handling secure file saving and deletion under wwwroot/uploads (photos or documents).
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long MaxFileSizeInBytes = 3 * 1024 * 1024; // 3 MB Unified Limit

    public FileStorageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// Step 1: Save photo or document image stream asynchronously under subfolder (photos or documents) mapped from FileCategory enum
    /// </summary>
    public async Task<FileUploadResult> SaveFileAsync(Stream fileStream, string fileName, FileCategory category = FileCategory.ProfilePhoto, CancellationToken cancellationToken = default)
    {
        // Step 1.1: Guard clause against null or empty stream
        if (fileStream == null || fileStream.Length == 0)
        {
            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = "No file was uploaded or file stream is empty.",
                StatusCode = 400
            };
        }

        // Step 1.2: Enforce maximum size limit (3MB)
        if (fileStream.Length > MaxFileSizeInBytes)
        {
            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = "File size exceeds the maximum allowed limit of 3MB.",
                StatusCode = 400
            };
        }

        // Step 1.3: Validate extension against whitelist
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (Array.IndexOf(AllowedExtensions, extension) < 0)
        {
            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = $"Invalid file extension. Allowed extensions are: {string.Join(", ", AllowedExtensions)}",
                StatusCode = 400
            };
        }

        // Step 1.4: Map FileCategory enum to target subfolder name
        var folderName = category switch
        {
            FileCategory.DocumentScan => "documents",
            _ => "photos"
        };

        // Step 1.5: Resolve root directory and ensure uploads/{folderName} directory exists
        var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsDirectory = Path.Combine(rootPath, "uploads", folderName);
        if (!Directory.Exists(uploadsDirectory))
        {
            Directory.CreateDirectory(uploadsDirectory);
        }

        // Step 1.6: Generate unique non-predictable filename
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(uploadsDirectory, uniqueFileName);

        // Step 1.7: Write stream to disk
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream, cancellationToken);
        }

        // Step 1.8: Return success result with relative URL path
        return new FileUploadResult
        {
            IsSuccess = true,
            FilePath = $"/uploads/{folderName}/{uniqueFileName}",
            StatusCode = 200
        };
    }

    /// <summary>
    /// Step 2: Delete existing file from disk by relative path
    /// </summary>
    public Task<bool> DeleteFileAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return Task.FromResult(false);

        var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var fullPath = Path.Combine(rootPath, relativePath.TrimStart('/', '\\'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// Alias method to delete photo file by relative path
    /// </summary>
    public Task<bool> DeletePhotoAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        return DeleteFileAsync(relativePath, cancellationToken);
    }
}
