using System;

namespace MediCore.Api.DTOs.ComplianceDtos;

public class CreateComplianceRecordResponseDto
{
    public int ComplianceId { get; set; }
    public string Message { get; set; }
}
