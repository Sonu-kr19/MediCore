

using Microsoft.AspNetCore.Http;

namespace MediCore.Api.DTOs.PatientDtos;

public class PatientDocumentRequestDto
{
    // Actual file uploaded by client — bytes stored in DB.
    public IFormFile? File { get; set; }

    // Valid types: Passport, IDCard, InsuranceCard, MedicalReport, Other.
    public string? DocType { get; set; }
}