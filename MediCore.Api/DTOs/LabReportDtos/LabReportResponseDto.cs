using System;

namespace MediCore.Api.DTOs.LabReportDtos
{
    public class LabReportResponseDto
    {
        public int EmrId { get; set; }
        public int LabReportID { get; set; }
        public int LabTestID { get; set; }
        public string FileURI { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
