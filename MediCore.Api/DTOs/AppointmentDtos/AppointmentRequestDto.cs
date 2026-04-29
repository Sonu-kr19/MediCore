using System;

namespace MediCore.Api.DTOs.AppointmentDtos;

public class AppointmentRequestDto
{
    public int DoctorID { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string IdempotencyKey { get; set; }
}
