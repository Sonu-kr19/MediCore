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

    /// <summary>
    /// Service method to create an appointment with doctor by Patient or admin
    /// </summary>
    /// <param name="dto">An object of appointment containing doctor id, patient id and timeslot with idempotency key</param>
    /// <returns>An object of appointment if create or already exist, else return error.</returns>
    Task<(AppointmentResponseDto result, bool isNew)> BookAppointment(int patientId,AppointmentRequestDto appointmentRequestDto);
    Task CancelAppointmentAsync(int appointmentId);
}
