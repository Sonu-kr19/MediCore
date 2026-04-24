using AutoMapper;
using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Api.DTOs.LabTestDto;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Domain.Entities;
using MediCore.Domain.Enum;

namespace MediCore.Api.Mapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        // Connect AppointmentRequestDto to Appointment object
        CreateMap<AppointmentRequestDto, Appointment>()
            .ForMember(dest=> dest.PatientID, opt => opt.Ignore())
            .ForMember(dest => dest.Status,
               opt => opt.MapFrom(_ => AppointmentStatusOption.Scheduled));
        
        // Connect Appointment to AppointmentRequestDto object
        CreateMap<Appointment, AppointmentResponseDto>();
        CreateMap<LabTestRequestDto, LabTest>()
            .ForMember(dest => dest.LabTestID, opt => opt.Ignore())
            .ForMember(dest => dest.DoctorID, opt => opt.Ignore());
        CreateMap<LabTest, LabTestResponseDto>();
            

    }
}
