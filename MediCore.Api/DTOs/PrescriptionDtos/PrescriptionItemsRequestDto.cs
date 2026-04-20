using System;

namespace MediCore.Api.DTOs.PrescriptionDtos;

public class PrescriptionItemRequestDto
{
    public required string Medicine { get; set; }
    public required string Dosage { get; set; }
    public required string Frequency { get; set; }
    public required string Duration { get; set; }
}
