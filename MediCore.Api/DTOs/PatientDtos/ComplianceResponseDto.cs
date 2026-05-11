
namespace MediCore.Api.DTOs.PatientDtos;

public class ComplianceResponseDto
{
    public int ComplianceRecordID { get; set; }
    public int PatientID { get; set; }
    public string Type { get; set; } = null!;
    public string Result { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Note { get; set; } = null!;
}