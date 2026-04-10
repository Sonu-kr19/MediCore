using System;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Api.Repositories.PatientRepo;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;
using MediCore.Api.Repositories.AuditRepo;

namespace MediCore.Api.Services.PatientServices;

// Contains all business logic for patient operations — no direct DB access here
public class PatientService : IPatientService
{
    // Repository dependency for all database operations
    private readonly IPatientRepository _patientRepository;
    // Repository dependency for logging all patient actions to the audit table
    private readonly IAuditLogRepository _auditLogRepository;

    // Injects the patient repository via constructor injection
    public PatientService(IPatientRepository patientRepository, IAuditLogRepository auditLogRepository)
    {
        _patientRepository    = patientRepository;
        _auditLogRepository   = auditLogRepository;
    }

    // Validates that the UserID exists, DOB is not a future date, and if InsuranceID is provided,
    // it must exist and must not already be assigned to another patient.
    // Maps the validated DTO to a Patient entity, persists it, and returns the generated PatientID.
    //logs the outcome in auditlogs table
    public async Task<int> RegisterPatientAsync(PatientRequestDto dto)
    {
        var userExists = await _patientRepository.UserExistsAsync(dto.UserID);
        if (!userExists)
        {
            await _auditLogRepository.LogAsync(dto.UserID, "PATIENT_REGISTER_FAILED", $"{dto.UserID} — {PatientErrorMessages.UserNotFound}");
            throw new ArgumentException(PatientErrorMessages.UserNotFound);
        }

        if (dto.DOB > DateOnly.FromDateTime(DateTime.Today))
        {
            await _auditLogRepository.LogAsync(dto.UserID, "PATIENT_REGISTER_FAILED", $"UserID: {dto.UserID} — {PatientErrorMessages.DOBFutureDate}");
            throw new ArgumentException(PatientErrorMessages.DOBFutureDate);
        }

        if (dto.InsuranceID.HasValue)
        {
            var insuranceExists = await _patientRepository.InsuranceExistsAsync(dto.InsuranceID.Value);
            if (!insuranceExists)
            {
                await _auditLogRepository.LogAsync(dto.UserID, "PATIENT_REGISTER_FAILED", $"UserID: {dto.UserID} — {PatientErrorMessages.InsuranceNotFound}");
                throw new ArgumentException(PatientErrorMessages.InsuranceNotFound);
            }

            var isDuplicate = await _patientRepository.DuplicateInsuranceAssignedAsync(dto.InsuranceID.Value);
            if (isDuplicate){
                await _auditLogRepository.LogAsync(dto.UserID, "PATIENT_REGISTER_FAILED", $"UserID: {dto.UserID} — {PatientErrorMessages.InsuranceAlreadyAssigned}");
                throw new InvalidOperationException(PatientErrorMessages.InsuranceAlreadyAssigned);
            }
        }

        var patient = new Patient
        {
            UserID      = dto.UserID,
            Name        = dto.Name,
            DOB         = dto.DOB,
            Gender      = dto.Gender,
            Address     = dto.Address,
            InsuranceID = dto.InsuranceID,
            Status      = true
        };

        var created = await _patientRepository.CreateAsync(patient);
        await _auditLogRepository.LogAsync(dto.UserID, "PATIENT_REGISTERED", $"PatientID: {created.PatientID} registered for UserID: {dto.UserID}");
        return created.PatientID;
    }

    // Retrieves all active patients from the repository, throws if none exist,
    // and maps each Patient entity to a PatientDetailsDto combining
    // Patient, User, and Insurance table fields for the API response.
    public async Task<List<PatientDetailsDto>> GetAllPatientsAsync()
    {
        var patients = await _patientRepository.GetAllPatientsAsync();

        if (patients.Count == 0)
        {   await _auditLogRepository.LogAsync(null, "PATIENT_GETALL_FAILED", PatientErrorMessages.PatientsNotFound);
            throw new Exception(PatientErrorMessages.PatientsNotFound);
        }

        await _auditLogRepository.LogAsync(null, "PATIENT_GETALL_SUCCESS", $"{patients.Count} active patient(s) retrieved");
        return patients.Select(p => new PatientDetailsDto
        {
            PatientID  = p.PatientID,
            Name = p.Name,
            DOB = p.DOB,
            Gender = p.Gender,
            Address = p.Address,
            InsuranceID = p.InsuranceID,
            Email = p.UserIDNavigator!.Email,
            Phone = p.UserIDNavigator!.Phone,
            InsuranceAmount = p.InsuranceIDNavigator != null? p.InsuranceIDNavigator.CoverageAmount : null
        }).ToList();
    }

}