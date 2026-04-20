using System;

namespace MediCore.Api.DTOs.PrescriptionDtos;

public class PrescriptionResponseDto
{
    public int PrescriptionID;

    public int EmrId { get; set; }

    public int TotalPrescriptionItems { get; set; }
    public List<PrescriptionItemRequestDto>? PrescriptionItems { get; set; }
}
