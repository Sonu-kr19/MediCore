using System;

namespace MediCore.Api.DTOs.EmrDtos;

public class PrescriptionSummaryDto
{
    public int PrescriptionID { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
    public List<PrescriptionItemSummaryDto> PrescriptionItems { get; set; } = new List<PrescriptionItemSummaryDto>();
}

public class PrescriptionItemSummaryDto
{
    public int PrescriptionItemID { get; set; }
    public string Medicine { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
}
