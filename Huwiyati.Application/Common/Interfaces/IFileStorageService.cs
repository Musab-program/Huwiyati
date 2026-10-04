namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Application.Common.Models;
using Huwiyati.Domain.Enums;

/// <summary>
/// Interface for file storage operations (saving and deleting profile photos and document attachments).
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Saves an uploaded file stream to local storage under a specified category (photos or documents).
    /// </summary>
    /// <param name="fileStream">The image or document file stream.</param>
    /// <param name="fileName">Original file name for extension checking.</param>
    /// <param name="category">Target file category enum (ProfilePhoto or DocumentScan).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>FileUploadResult containing success flag and relative file path or error message.</returns>
    Task<FileUploadResult> SaveFileAsync(Stream fileStream, string fileName, FileCategory category = FileCategory.ProfilePhoto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing file from storage by its relative path.
    /// </summary>
    /// <param name="relativePath">The relative URL path of the file to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if file was deleted or did not exist; false otherwise.</returns>
    Task<bool> DeleteFileAsync(string relativePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Alias method to delete a photo file by relative path.
    /// </summary>
    Task<bool> DeletePhotoAsync(string relativePath, CancellationToken cancellationToken = default);
}