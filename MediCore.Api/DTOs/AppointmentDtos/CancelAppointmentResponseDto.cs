using System;

namespace MediCore.Api.DTOs.AppointmentDtos;

public class CancelAppointmentResponseDto
{
    public int AppointmentId { get; set; }
    public string? Message { get; set; }
}
