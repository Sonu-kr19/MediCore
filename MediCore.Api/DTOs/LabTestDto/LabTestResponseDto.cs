using System;

namespace MediCore.Api.DTOs.LabTestDto;

public class LabTestResponseDto
{
    public int LabTestID { get; set; }
    public int PatientID {get; set;}
    public int DoctorID {get; set;}
    public string Type {get; set;}
    public DateTime Date {get; set;}
    public int? TechnicianID {get; set;}
    public bool Status {get; set;}
}
