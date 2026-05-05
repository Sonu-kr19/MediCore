using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.AppointmentRepository;

public interface IScheduleRepository
{
    /// <summary>
    /// Method to check availability of doctor
    /// </summary>
    /// <param name="doctorId">A unique identifier of doctor.</param>
    /// <param name="date">Date</param>
    /// <param name="timeSlot">Time</param>
    /// <returns>Return schedule if availability is true, else null</returns>
    Task<Schedule?> GetSlotAsync(int doctorId, DateOnly date, TimeOnly timeSlot);

    /// <summary>
    /// method to check availability of doctor
    /// </summary>
    /// <param name="doctorId">Id</param>
    /// <param name="date">date</param>
    /// <param name="timeSlot">time</param>
    /// <returns>Return schedule if availability is false, else null</returns>
    Task<Schedule?> GetAllotedSlotAsync(int doctorId, DateOnly date, TimeOnly timeSlot);

    /// <summary>
    /// method to update schedule
    /// </summary>
    /// <param name="schedule"></param>
    /// <returns></returns>
    Task UpdateAsync(Schedule schedule);

}
