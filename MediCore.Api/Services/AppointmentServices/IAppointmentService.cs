using System;
using MediCore.Api.DTOs.AppointmentDtos;

namespace MediCore.Api.Services.AppointmentServices;

public interface IAppointmentService
{
    /// <summary>
    /// Service method to fetch ScheduleResponseDto of doctor for appointment
    /// </summary>
    /// <param name="doctorId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<List<ScheduleResponseDto>> GetFreeSlots(int doctorId, DateOnly date);
}
