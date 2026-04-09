using System;
using MediCore.Api.DTOs.PatientDtos;

namespace MediCore.Api.Services.PatientServices;

public interface IPatientService
{
    Task<int> RegisterPatientAsync(PatientRequestDto dto);
    Task<List<PatientDetailsDto>> GetAllPatientsAsync();
}
