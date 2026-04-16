using System;

namespace MediCore.Api.DTOs.AppointmentDtos;

public class AppointmentResponseDto
{
    public int AppointmentID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string IdempotencyKey { get; set; }
    public string Status { get; set; }
}
