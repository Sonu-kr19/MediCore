using System;

namespace MediCore.Api.DTOs.AppointmentDtos;

public class RescheduleRequestDto
{
    public DateOnly NewDate { get; set; }
    public TimeOnly NewTime { get; set; }
}
