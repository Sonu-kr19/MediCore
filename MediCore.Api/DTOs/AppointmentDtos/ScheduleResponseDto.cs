using System;

namespace MediCore.Api.DTOs.AppointmentDtos;

public class ScheduleResponseDto
{
    public int ScheduleID { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly TimeSlot { get; set; }
    public bool Availability { get; set; }
}
