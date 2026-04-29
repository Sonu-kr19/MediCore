using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.AppointmentRepository;

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

    /// <summary>
    /// Method for creating an appointment with doctor and saved in database.
    /// </summary>
    /// <param name="appointment">An object of appointment.</param>
    /// <returns>An object of new appointment created.</returns>
    Task<Appointment> CreateAppointment(Appointment appointment);

    /// <summary>
    /// Method to check idempotency key in database.
    /// </summary>
    /// <param name="key">A unique string for checking appointment in database.</param>
    /// <returns>return appointment if key is matched, else return null</returns>
    Task<Appointment?> FindIdempotencyKey(string key); 
    
    Task<Appointment?> GetByIdAsync(int appointmentId);
    Task UpdateAsync(Appointment appointment);
}
