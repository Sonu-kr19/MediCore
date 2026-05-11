namespace MediCore.Api.DTOs.PatientDtos;

public class ComplianceVerifyDto
{
    // Must be "Approved" or "Rejected" — anything else returns 400.
    public string? Result { get; set; }

    // Optional reviewer note.
    public string? Note { get; set; }
}