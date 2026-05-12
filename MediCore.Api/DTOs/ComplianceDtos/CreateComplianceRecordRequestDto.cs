using System;

namespace MediCore.Api.DTOs.ComplianceDtos;

public class CreateComplianceRecordRequestDto
{
    public int PatientID { get; set; }
    public string Type { get; set; }
    public string Result { get; set; }
    public string Note { get; set; }
}
