using AutoMapper;
using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Domain.Entities;
using MediCore.Domain.Enum;

namespace MediCore.Api.Mapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        // Connect AppointmentRequestDto to Appointment object
        CreateMap<AppointmentRequestDto, Appointment>()
            .ForMember(dest => dest.Status,
               opt => opt.MapFrom(_ => AppointmentStatusOption.Scheduled));
        
        // Connect Appointment to AppointmentRequestDto object
        CreateMap<Appointment, AppointmentResponseDto>();
    }
}
