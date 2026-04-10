using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Api.Repositories.AppointmentRepository;
using MediCore.Api.Utilities;

namespace MediCore.Api.Services.AppointmentServices;

public class AppointmentService:IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    public AppointmentService(IAppointmentRepository repository)
    {
        _repository=repository;
    }    
    // public Task BookAppointment(AppointmentRequestDto appointmentRequestDto)
    // {
    //     throw new NotImplementedException();
    // }

     public async Task<List<ScheduleResponseDto>> GetFreeSlots(int doctorId, DateOnly date)
      {
        // Validating doctorId input, It should not be negative and zero
        if (doctorId <= 0)
        {
            throw new ArgumentException(ErrorMessages.InvalidDoctorId);
        }

        if(date==default)
        {
            throw new ArgumentException(ErrorMessages.DateRequired);
        }

        // Coming boolean value for the existing doctor or not
        var exists = await _repository.DoctorExists(doctorId);
        if (!exists)
        {
            throw new KeyNotFoundException(ErrorMessages.DoctorNotFound);
        }
        // List of schedule comming from repository
        var schedule = await _repository.GetFreeSlots(doctorId, date);

        return schedule.Select(s=> new ScheduleResponseDto
        {
            ScheduleID = s.ScheduleID,
            DoctorId = s.DoctorID,
            DoctorName = s.Doctor.Name,
            Date = s.Date,
            TimeSlot = s.TimeSlot,
            Availability = s.Availability
        }).ToList();
    }
}
