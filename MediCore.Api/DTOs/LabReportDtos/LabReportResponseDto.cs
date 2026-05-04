using System;

namespace MediCore.Api.DTOs.LabReportDtos
{
    public class LabReportResponseDto
    {
        public int LabReportID { get; set; }
        public int LabTestID { get; set; }
        public string FileURI { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
