using System;

namespace MediCore.Api.DTOs.EmrDtos;

public class AttachLabReportResponseDto
{
    // public int AttachmentId { get; set; }
    public int EmrId { get; set; }
    public DateTime AttachedAt { get; set; }

    // from LabReport entity
    public int LabReportID { get; set; }
    public int LabTestID { get; set; }
    public string FileURI { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool Status { get; set; }
}
