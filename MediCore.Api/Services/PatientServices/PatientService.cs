using System;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Api.Repositories.PatientRepo;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;
using MediCore.Api.Repositories.AuditRepo;
using MediCore.Api.Utilities.Helpers;
using AutoMapper;
using MediCore.Domain.Enum;

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
        PatientHelper.Validate(dto.Name, dto.Address, dto.DOB, dto.Gender, dto.InsuranceID);

        //updated only Patient can register . no other role cannot be register
        //added audit log for it if success or fail
        var role = await _patientRepository.GetUserRoleAsync(dto.UserID);

        if (role != RoleOption.Patient)
        {
            await _auditLogRepository.LogAsync(
                dto.UserID,
                "PATIENT_REGISTER_FAILED",
                $"UserID: {dto.UserID} — Invalid role: {role}"
            );
            throw new ArgumentException(
                "Only users with Patient role can register as a patient."
            );
        }

        var userExists = await _patientRepository.UserExistsAsync(dto.UserID);
        if (!userExists)
        {
            await _auditLogRepository.LogAsync(dto.UserID, "PATIENT_REGISTER_FAILED", $"{dto.UserID} — {PatientErrorMessages.UserNotFound}");
            throw new ArgumentException(PatientErrorMessages.UserNotFound);
        }

        var patientExists = await _patientRepository.PatientUserExistsAsync(dto.UserID);
        if (patientExists)
            throw new InvalidOperationException(PatientErrorMessages.PatientAlreadyRegistered);

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

    public async Task<PatientResponseDto?> GetByIdAsync(int userId)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(userId);
            if (patient == null)
            {
                throw new MediCoreException(ErrorMessages.PatientNotFound);
            } 
            return new PatientResponseDto
            {
                PatientID  = patient.PatientID,
                Name = patient.Name,
                DOB = patient.DOB,
                Gender = patient.Gender,
                Address = patient.Address,
                InsuranceID = patient.InsuranceID
                
            };
        }
        catch (MediCoreException)
        {
            throw;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    

    //searching a patient 
    //the term takes patient id, name or insurance id
    //is user direcly search it will display first 20 records by default
    // or otherwise it will show the specific patient data only
    public async Task<PagedResult<PatientResponseDto>> SearchPatientsAsync(PatientSearchDto request)
    {
        var term = string.IsNullOrWhiteSpace(request.Search) ? string.Empty : request.Search.Trim();

        List<Patient> allMatches;
        if (string.IsNullOrWhiteSpace(term))
            allMatches = await _patientRepository.GetAllPatientsAsync();
        else
            allMatches = await _patientRepository.SearchPatientsAsync(term);

        // Pagination logic lives in the service — not the repository.
        var totalCount = allMatches.Count;
        var paged = allMatches
            .OrderBy(p => p.PatientID)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

         
        // Map entity → DTO in the service layer.
        var data = paged.Select(p => new PatientResponseDto
        {
            PatientID = p.PatientID,
            Name = p.Name,
            Gender = p.Gender,
            DOB = p.DOB,
            Address = p.Address,
            InsuranceID = p.InsuranceID
        }).ToList();

        return new PagedResult<PatientResponseDto>
        {
            Data = data,
            TotalCount = totalCount,
        };
    }

    //deleting the patient softly
    //only changing the status in to false
    // and added errors if patient is not exist, or if patient is already deleted 
    // and finally printing response as patient id and message is patient delete successfully
     public async Task<PatientDeleteResponseDto> DeletePatientAsync(int patientId)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);
        if (patient == null)
            throw new KeyNotFoundException(PatientErrorMessages.PatientNotFound);

        if (patient.Status == false)
            throw new InvalidOperationException(PatientErrorMessages.PatientAlreadyDeleted);

        await _patientRepository.SoftDeleteAsync(patient);

        return new PatientDeleteResponseDto
        {
            PatientID = patientId,
            Message = $"PatientID {patientId} deleted successfully."
        };
    } 

}