using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.AppointmentServices;

public interface IAppointmentRepository
{
    /// <summary>
    ///  Method for Fetching the available slots of doctor.
    /// </summary>
    /// <param name="doctorId"></param>
    /// <param name="date"></param>
    /// <returns>Return the Schedule of doctor.</returns>
    Task<List<Schedule>> GetFreeSlots(int doctorId, DateOnly date);

    /// <summary>
    /// Method for checking Doctor exists with this doctor id.
    /// </summary>
    /// <param name="doctorId"></param>
    /// <returns>Return boolean value</returns>
    Task<bool> DoctorExists(int doctorId);
}
