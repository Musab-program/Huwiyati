namespace Huwiyati.Application.Documents.Verification.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Verification.DTOs;
using Huwiyati.Domain.Enums;

/// <summary>
/// Query handler that verifies scanned document QR payloads across all system document types.
/// </summary>
public class VerifyDocumentByQrPayloadQueryHandler
{
    private readonly IApplicationDbContext _context;

    public VerifyDocumentByQrPayloadQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Step 1: Main entry point for verifying a scanned QR payload against all document repositories.
    /// </summary>
    public async Task<ApiResponse<DocumentVerificationResultDto>> VerifyAsync(
        string qrPayload,
        CancellationToken cancellationToken = default)
    {
        // Step 1.1: Guard clause against empty or whitespace payloads
        if (string.IsNullOrWhiteSpace(qrPayload))
        {
            return ApiResponse<DocumentVerificationResultDto>.Failure("Verification payload cannot be empty.", statusCode: 400);
        }

        // Step 1.2: Check National ID Cards repository
        var result = await TryVerifyNationalIdCardAsync(qrPayload, cancellationToken);
        if (result != null) return ApiResponse<DocumentVerificationResultDto>.Success(result, "Document verified successfully.");

        // Step 1.3: Check Passports repository
        result = await TryVerifyPassportAsync(qrPayload, cancellationToken);
        if (result != null) return ApiResponse<DocumentVerificationResultDto>.Success(result, "Document verified successfully.");

        // Step 1.4: Check Family Cards repository
        result = await TryVerifyFamilyCardAsync(qrPayload, cancellationToken);
        if (result != null) return ApiResponse<DocumentVerificationResultDto>.Success(result, "Document verified successfully.");

        // Step 1.5: Check Birth Certificates repository
        result = await TryVerifyBirthCertificateAsync(qrPayload, cancellationToken);
        if (result != null) return ApiResponse<DocumentVerificationResultDto>.Success(result, "Document verified successfully.");

        // Step 1.6: Check Death Certificates repository
        result = await TryVerifyDeathCertificateAsync(qrPayload, cancellationToken);
        if (result != null) return ApiResponse<DocumentVerificationResultDto>.Success(result, "Document verified successfully.");

        // Step 1.7: Return 404 Failure if no matching document payload was found
        return ApiResponse<DocumentVerificationResultDto>.Failure("Invalid or unverified document payload.", statusCode: 404);
    }

    /// <summary>
    /// Step 2: Private helper method to verify National ID Card payload using projection Select (No Include)
    /// Simplified anonymous member names to fix IDE0037 warnings.
    /// </summary>
    private async Task<DocumentVerificationResultDto?> TryVerifyNationalIdCardAsync(string qrPayload, CancellationToken ct)
    {
        var card = await _context.NationalIdCards
            .AsNoTracking()
            .Where(c => c.QrCodePayload == qrPayload)
            .Select(c => new
            {
                c.Status,
                c.IssueDate,
                c.ExpiryDate,
                c.Person.NationalNumber,
                FullName = (c.Person.FirstName + " " + c.Person.FatherName + " " + c.Person.GrandfatherName + " " + c.Person.FamilyName).Trim(),
                c.Person.PhotoUrl,
                c.OrganizationBranch.BranchName
            })
            .FirstOrDefaultAsync(ct);

        if (card == null) return null;

        var isExpired = card.ExpiryDate < DateOnly.FromDateTime(DateTime.UtcNow);
        var isValid = card.Status == NationalIdCardStatus.Active && !isExpired;

        return new DocumentVerificationResultDto
        {
            IsValid = isValid,
            DocumentType = "NationalIdCard",
            DocumentNumber = card.NationalNumber,
            HolderName = card.FullName,
            HolderPhotoUrl = card.PhotoUrl,
            Status = isValid ? "Active" : (isExpired ? "Expired" : card.Status.ToString()),
            IssueDate = card.IssueDate,
            ExpiryDate = card.ExpiryDate,
            IssuingBranchName = card.BranchName,
            VerificationTimestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Step 3: Private helper method to verify Passport payload using projection Select (No Include)
    /// Simplified anonymous member names to fix IDE0037 warnings.
    /// </summary>
    private async Task<DocumentVerificationResultDto?> TryVerifyPassportAsync(string qrPayload, CancellationToken ct)
    {
        var passport = await _context.Passports
            .AsNoTracking()
            .Where(p => p.QrCodePayload == qrPayload)
            .Select(p => new
            {
                p.Status,
                p.IssueDate,
                p.ExpiryDate,
                p.PassportNumber,
                FullName = (p.Person.FirstName + " " + p.Person.FatherName + " " + p.Person.GrandfatherName + " " + p.Person.FamilyName).Trim(),
                p.Person.PhotoUrl,
                p.IssuingBranch.BranchName
            })
            .FirstOrDefaultAsync(ct);

        if (passport == null) return null;

        var isExpired = passport.ExpiryDate < DateOnly.FromDateTime(DateTime.UtcNow);
        var isValid = passport.Status == PassportStatus.Active && !isExpired;

        return new DocumentVerificationResultDto
        {
            IsValid = isValid,
            DocumentType = "Passport",
            DocumentNumber = passport.PassportNumber,
            HolderName = passport.FullName,
            HolderPhotoUrl = passport.PhotoUrl,
            Status = isValid ? "Active" : (isExpired ? "Expired" : passport.Status.ToString()),
            IssueDate = passport.IssueDate,
            ExpiryDate = passport.ExpiryDate,
            IssuingBranchName = passport.BranchName,
            VerificationTimestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Step 4: Private helper method to verify Family Card payload using projection Select (No Include)
    /// Simplified anonymous member names to fix IDE0037 warnings.
    /// </summary>
    private async Task<DocumentVerificationResultDto?> TryVerifyFamilyCardAsync(string qrPayload, CancellationToken ct)
    {
        var family = await _context.Families
            .AsNoTracking()
            .Where(f => f.QrCodePayload == qrPayload)
            .Select(f => new
            {
                f.Status,
                f.IssueDate,
                f.ExpiryDate,
                f.FamilyNumber,
                FullName = (f.HeadOfFamily.FirstName + " " + f.HeadOfFamily.FatherName + " " + f.HeadOfFamily.GrandfatherName + " " + f.HeadOfFamily.FamilyName).Trim(),
                PhotoUrl = f.HeadOfFamily.PhotoUrl,
                BranchName = f.IssuingBranch.BranchName
            })
            .FirstOrDefaultAsync(ct);

        if (family == null) return null;

        var isExpired = family.ExpiryDate < DateOnly.FromDateTime(DateTime.UtcNow);
        var isValid = family.Status == FamilyStatus.Active && !isExpired;

        return new DocumentVerificationResultDto
        {
            IsValid = isValid,
            DocumentType = "FamilyCard",
            DocumentNumber = family.FamilyNumber,
            HolderName = family.FullName,
            HolderPhotoUrl = family.PhotoUrl,
            Status = isValid ? "Active" : (isExpired ? "Expired" : family.Status.ToString()),
            IssueDate = family.IssueDate,
            ExpiryDate = family.ExpiryDate,
            IssuingBranchName = family.BranchName,
            VerificationTimestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Step 5: Private helper method to verify Birth Certificate payload using projection Select (No Include)
    /// Simplified anonymous member names to fix IDE0037 warnings.
    /// </summary>
    private async Task<DocumentVerificationResultDto?> TryVerifyBirthCertificateAsync(string qrPayload, CancellationToken ct)
    {
        var cert = await _context.BirthCertificates
            .AsNoTracking()
            .Where(b => b.QrCodePayload == qrPayload)
            .Select(b => new
            {
                b.IssueDate,
                b.CertificateNumber,
                ChildFullName = (b.ChildPerson.FirstName + " " + b.ChildPerson.FatherName + " " + b.ChildPerson.GrandfatherName + " " + b.ChildPerson.FamilyName).Trim(),
                PhotoUrl = b.ChildPerson.PhotoUrl,
                BranchName = b.HospitalBranch.BranchName
            })
            .FirstOrDefaultAsync(ct);

        if (cert == null) return null;

        return new DocumentVerificationResultDto
        {
            IsValid = true,
            DocumentType = "BirthCertificate",
            DocumentNumber = cert.CertificateNumber,
            HolderName = cert.ChildFullName,
            HolderPhotoUrl = cert.PhotoUrl,
            Status = "Active",
            IssueDate = cert.IssueDate,
            ExpiryDate = null,
            IssuingBranchName = cert.BranchName,
            VerificationTimestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Step 6: Private helper method to verify Death Certificate payload using projection Select (No Include)
    /// Simplified anonymous member names to fix IDE0037 warnings.
    /// </summary>
    private async Task<DocumentVerificationResultDto?> TryVerifyDeathCertificateAsync(string qrPayload, CancellationToken ct)
    {
        var cert = await _context.DeathCertificates
            .AsNoTracking()
            .Where(d => d.QrCodePayload == qrPayload)
            .Select(d => new
            {
                d.IssueDate,
                d.CertificateNumber,
                DeceasedFullName = (d.Person.FirstName + " " + d.Person.FatherName + " " + d.Person.GrandfatherName + " " + d.Person.FamilyName).Trim(),
                PhotoUrl = d.Person.PhotoUrl,
                BranchName = d.IssuingBranch != null ? d.IssuingBranch.BranchName : d.HospitalBranch.BranchName
            })
            .FirstOrDefaultAsync(ct);

        if (cert == null) return null;

        return new DocumentVerificationResultDto
        {
            IsValid = true,
            DocumentType = "DeathCertificate",
            DocumentNumber = cert.CertificateNumber,
            HolderName = cert.DeceasedFullName,
            HolderPhotoUrl = cert.PhotoUrl,
            Status = "Active",
            IssueDate = cert.IssueDate,
            ExpiryDate = null,
            IssuingBranchName = cert.BranchName,
            VerificationTimestamp = DateTime.UtcNow
        };
    }
}
