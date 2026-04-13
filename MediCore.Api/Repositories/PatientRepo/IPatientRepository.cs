using System;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.PatientRepo;

public interface IPatientRepository
{
    Task<bool> UserExistsAsync(int userId);
    Task<bool> PatientUserExistsAsync(int userId);
    Task<bool> InsuranceExistsAsync(int insuranceId);
    Task<bool> DuplicateInsuranceAssignedAsync(int insuranceId);
    Task<Patient> CreateAsync(Patient patient);

    // Task<List<PatientDetailsDto>> GetAllPatientsAsync();
     Task<List<Patient>> GetAllPatientsAsync();

}
