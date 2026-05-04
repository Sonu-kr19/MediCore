using System;

namespace MediCore.Api.DTOs.LabReportDtos
{
    public class QueuedLabReportDto
    {
        public int LabReportID { get; set; }
        public int LabTestID { get; set; }
        public string LabTestName { get; set; }
        public string FileURI { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
