using System;
using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.AppointmentServices;

public interface IScheduleService
{
    /// <summary>
    /// Service method to fetch ScheduleResponseDto of doctor for appointment
    /// </summary>
    /// <param name="doctorId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<List<ScheduleResponseDto>> GetFreeSlots(int doctorId, DateOnly date);
}
