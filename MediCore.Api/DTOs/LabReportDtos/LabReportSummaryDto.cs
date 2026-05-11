using System;
using MediCore.Api.DTOs.LabReportDtos;

namespace MediCore.Api.DTOs.LabReportDtos;

public class LabReportSummaryDto
{
    public int EmrId { get; set; }
    public int LabReportID { get; set; }
    public int LabTestID { get; set; }
    public string FileURI { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool Status { get; set; }
}
