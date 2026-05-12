using MediCore.Api.DTOs.LabReportDtos;

namespace MediCore.Api.DTOs.EmrDtos;

public class EmrResponseDto
{
    public int EmrId { get; set; }
    public DateTime Date { get; set; }
    public string? Diagnosis { get; set; }
    public string? TreatmentPlan { get; set; }
    public List<PrescriptionSummaryDto>? Prescriptions { get; set; }
    public List<LabReportSummaryDto>? LabReports { get; set; } 
}