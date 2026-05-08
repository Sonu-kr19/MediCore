
using System.Security.Cryptography;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Api.Repositories.PatientDocumentRepo;
using MediCore.Api.Services.ComplianceService;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.PatientDocumentServices;

public class PatientDocumentService : IPatientDocumentService
{
    private static readonly HashSet<string> ValidDocTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Passport", "IDCard", "InsuranceCard", "MedicalReport", "Other"
    };

    private readonly IPatientDocumentRepo _documentRepo;
    private readonly IComplianceService _complianceService;

    public PatientDocumentService(
        IPatientDocumentRepo documentRepo,
        IComplianceService complianceService)
    {
        _documentRepo      = documentRepo;
        _complianceService = complianceService;
    }

    public async Task<PatientDocumentResponseDto> UploadDocumentAsync(int patientId, PatientDocumentRequestDto dto)
    {
        // Validate before any DB operation.
        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException(PatientDocumentErrorMessages.FileRequired);

        if (string.IsNullOrWhiteSpace(dto.DocType))
            throw new ArgumentException(PatientDocumentErrorMessages.DocTypeRequired);

        if (!ValidDocTypes.Contains(dto.DocType))
            throw new ArgumentException(PatientDocumentErrorMessages.InvalidDocType);

        var patientExists = await _documentRepo.PatientExistsAsync(patientId);
        if (!patientExists)
            throw new KeyNotFoundException(PatientDocumentErrorMessages.PatientNotFound);

        // Read file bytes into memory using MemoryStream — no disk write.
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await dto.File.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }

        // Hash file bytes using SHA256 — hashed name stored as FileURI.
        string hashedName;
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(fileBytes);
            hashedName    = Convert.ToHexString(hashBytes);
        }

        var extension = Path.GetExtension(dto.File.FileName);
        var fileUri   = $"{hashedName}{extension}";

        var document = new PatientDocument
        {
            PatientID          = patientId,
            DocType            = dto.DocType,
            FileURI            = fileUri,
            FileData           = fileBytes,
            UploadedDate       = DateTime.UtcNow,
            VerificationStatus = false
        };

        var created = await _documentRepo.CreateAsync(document);

        // Trigger compliance event after successful upload.
        await _complianceService.LogComplianceEventAsync(patientId, dto.DocType!);

        return new PatientDocumentResponseDto
        {
            DocumentId = created.PatientDocumentID,
            FileUri    = fileUri,
            Message    = "Document uploaded successfully. Pending verification."
        };
    }

    public async Task<List<PatientDocumentDetailsDto>> GetDocumentsByPatientAsync(int patientId)
    {
        var documents = await _documentRepo.GetByPatientIdAsync(patientId);

        if (documents.Count == 0)
            throw new Exception(PatientDocumentErrorMessages.NoDocumentsFound);

        // Map entity → DTO — FileData excluded from list response.
        return documents.Select(d => new PatientDocumentDetailsDto
        {
            PatientDocumentID  = d.PatientDocumentID,
            PatientID          = d.PatientID,
            DocType            = d.DocType,
            FileUri            = d.FileURI,
            UploadedDate       = d.UploadedDate,
            VerificationStatus = d.VerificationStatus
        }).ToList();
    }

    public async Task<List<PatientDocumentDetailsDto>> GetDownloadableDocumentsByPatientAsync(int patientId)
    {
        var documents = await _documentRepo.GetDownloadableByPatientIdAsync(patientId);

        if (documents.Count == 0)
            throw new Exception(PatientDocumentErrorMessages.NoDocumentsFound);

        return documents.Select(d => new PatientDocumentDetailsDto
        {
            PatientDocumentID  = d.PatientDocumentID,
            PatientID          = d.PatientID,
            DocType            = d.DocType,
            FileUri            = d.FileURI,
            UploadedDate       = d.UploadedDate,
            VerificationStatus = d.VerificationStatus
        }).ToList();
    }

    public async Task<(byte[] fileBytes, string fileName, string contentType)> DownloadDocumentAsync(int patientId)
    {
        var documents = await _documentRepo.GetDownloadableByPatientIdAsync(patientId);

        if (documents.Count == 0)
            throw new Exception(PatientDocumentErrorMessages.NoDocumentsFound);

        // If only one document — return it directly.
        if (documents.Count == 1)
        {
            var doc = documents.First();
            if (doc.FileData == null || doc.FileData.Length == 0)
                throw new Exception(PatientDocumentErrorMessages.FileDataNotFound);
            return (doc.FileData, doc.FileURI, GetContentType(doc.FileURI));
        }

        // Multiple documents — zip them all together.
        using var zipStream = new MemoryStream();
        using (var archive = new System.IO.Compression.ZipArchive(zipStream, System.IO.Compression.ZipArchiveMode.Create, true))
        {
            foreach (var doc in documents)
            {
                if (doc.FileData == null || doc.FileData.Length == 0) continue;
                var entry = archive.CreateEntry(doc.FileURI);
                using var entryStream = entry.Open();
                await entryStream.WriteAsync(doc.FileData);
            }
        }

        return (zipStream.ToArray(), $"patient_{patientId}_documents.zip", "application/zip");
    }

    public async Task<PatientDocumentDetailsDto> VerifyDocumentAsync(int patientId, int documentId)
    {
        var document = await _documentRepo.GetByIdAsync(documentId);
        if (document == null)
            throw new KeyNotFoundException(PatientDocumentErrorMessages.DocumentNotFound);

        // Ensure document belongs to the specified patient.
        if (document.PatientID != patientId)
            throw new ArgumentException(PatientDocumentErrorMessages.DocumentNotOwnedByPatient);

        // Prevent re-verifying an already approved document.
        if (document.VerificationStatus == true)
            throw new InvalidOperationException(PatientDocumentErrorMessages.DocumentAlreadyVerified);

        document.VerificationStatus = true;
        await _documentRepo.UpdateAsync(document);

        return new PatientDocumentDetailsDto
        {
            PatientDocumentID  = document.PatientDocumentID,
            PatientID          = document.PatientID,
            DocType            = document.DocType,
            UploadedDate       = document.UploadedDate,
            VerificationStatus = document.VerificationStatus
        };
    }

    // Map file extension to MIME content type for download response.
    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".pdf"  => "application/pdf",
            ".jpg"  => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png"  => "image/png",
            _       => "application/octet-stream"
        };
    }
}