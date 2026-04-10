using System;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Api.DTOs.EmrDtos;

public class EmrRequestDto
{
    
    [Required]
    public int PatientID { get; set; }
    [Required]
    public int DoctorID { get; set; }
    [Required]
    public required string Diagnosis { get; set; }
    [Required]
    public required string TreatmentPlan { get; set; }
}
