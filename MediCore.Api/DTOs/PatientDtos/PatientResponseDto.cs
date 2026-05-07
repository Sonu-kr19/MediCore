using System;

using MediCore.Domain.Enum;
namespace MediCore.Api.DTOs.PatientDtos;

public class PatientResponseDto
{
    //for showing response in website for registered patient
    public int PatientID { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly DOB { get; set; }
    public GenderOption Gender { get; set; }
    public string Address { get; set; } = null!;
    public int? InsuranceID { get; set; }


}
