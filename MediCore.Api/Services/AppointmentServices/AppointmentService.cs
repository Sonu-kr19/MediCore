using AutoMapper;
using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Api.Mapper;
using MediCore.Api.Repositories.AppointmentRepository;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;
using MediCore.Domain.Enum;

namespace MediCore.Api.Services.AppointmentServices;

public class AppointmentService:IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;
    public AppointmentService(IAppointmentRepository repository, IMapper mapper)
    {
        _repository=repository;
        _mapper=mapper;
    }       
    public async Task<(AppointmentResponseDto result, bool isNew)> BookAppointment(AppointmentRequestDto appointmentRequestDto)
    {
        try
        {
            bool isNew = false;
            var appointment = await _repository.FindIdempotencyKey(appointmentRequestDto.IdempotencyKey);
            if (appointment==null)
            {
                var slots = await GetFreeSlots(appointmentRequestDto.DoctorID, appointmentRequestDto.Date);
                bool IsVacantSlot = CheckSlotsTiming(slots, appointmentRequestDto.Time);
                if (!IsVacantSlot)
                {
                    throw new ConflictException(ErrorMessages.SlotTaken);
                }
                var appointmentToAdd = _mapper.Map<AppointmentRequestDto, Appointment>(appointmentRequestDto);
                appointmentToAdd.Status=AppointmentStatusOption.Scheduled;
                var response = await _repository.CreateAppointment(appointmentToAdd);
                isNew = true;
                var result = _mapper.Map<Appointment, AppointmentResponseDto>(response);
                return (result,isNew);
            }
            return (_mapper.Map<Appointment, AppointmentResponseDto>(appointment), isNew);
        }
        catch (ConflictException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new MediCoreException(ErrorMessages.FailedToCreateAppointment);
        }
    }

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
    private bool CheckSlotsTiming(List<ScheduleResponseDto> slots, TimeOnly time)
    {
        foreach(var slot in slots)
        {
            if(slot.Availability==true && slot.TimeSlot == time)
            {
                return true;
            }
        }
        return false;
    }

    
    public async Task CancelAppointmentAsync(int appointmentId)
    {
        var appointment = await _repository.GetByIdAsync(appointmentId);

        if (appointment == null)
        {
            throw new KeyNotFoundException(ErrorMessage.AppointmentNotFound);
        }

        if (appointment.Status == AppointmentStatusOption.Cancelled)
        {
            throw new InvalidOperationException(ErrorMessage.AppointmentAlreadyCancelled);
        }

        if (appointment.Status == AppointmentStatusOption.Completed)
        {
            throw new InvalidOperationException(ErrorMessage.CompletedAppointmet);
        }
        
        if (appointment.Status == AppointmentStatusOption.Ongoing)
        {
         throw new InvalidOperationException(ErrorMessage.OngoingAppointment);
        }
        appointment.Status = AppointmentStatusOption.Cancelled;
        await _repository.UpdateAsync(appointment);
    }
}
