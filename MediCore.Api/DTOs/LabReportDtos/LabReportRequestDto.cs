using System;

namespace MediCore.Api.DTOs.LabReportDtos
{
    public class LabReportRequestDto
    {
        public int LabTestID { get; set; }
        public string FileURI { get; set; }
        public string Notes { get; set; }
    }
}
