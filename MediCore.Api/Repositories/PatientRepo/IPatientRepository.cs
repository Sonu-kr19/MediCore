using System;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Domain.Entities;
using MediCore.Domain.Enum;

namespace MediCore.Api.Repositories.PatientRepo;

public interface IPatientRepository
{
    Task<bool> UserExistsAsync(int userId);
    Task<bool> PatientUserExistsAsync(int userId);
    Task<bool> InsuranceExistsAsync(int insuranceId);
    Task<bool> DuplicateInsuranceAssignedAsync(int insuranceId);
    Task<Patient> CreateAsync(Patient patient);
    Task<Patient?> GetByIdAsync(int userId);
     Task<List<Patient>> GetAllPatientsAsync();
     Task<bool> PatientExistsAsync(int patientId);
    Task<List<Patient>> SearchPatientsAsync(string term);
    Task<Patient?> GetPatientByIdAsync(int patientId);

    Task SoftDeleteAsync(Patient patient);
    Task<RoleOption?> GetUserRoleAsync(int userId);


}
