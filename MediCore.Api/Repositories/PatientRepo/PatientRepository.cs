using System;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.PatientRepo;

// Handles all direct database operations for the Patient entity
public class PatientRepository : IPatientRepository
{
    // EF Core DB context used to query and persist data
    private readonly MediCoreDbContext _db;

    // Injects the database context via constructor injection
    public PatientRepository(MediCoreDbContext db)
    {
        _db = db;
    }

    // Checks if a User with the given ID exists — used to validate UserID before creating a patient
    public async Task<bool> UserExistsAsync(int userId)
        => await _db.Users.AnyAsync(u => u.UserID == userId);

    // Checks if an Insurance record with the given ID exists — used to validate InsuranceID before linking
    public async Task<bool> InsuranceExistsAsync(int insuranceId)
        => await _db.Insurances.AnyAsync(i => i.InsuranceID == insuranceId);

    // Checks if the given InsuranceID is already linked to another patient — prevents duplicate assignment
    public async Task<bool> DuplicateInsuranceAssignedAsync(int insuranceId)
        => await _db.Patients.AnyAsync(p => p.InsuranceID == insuranceId);

    // Check if this UserID already has a patient record — prevents duplicate registration.
    public async Task<bool> PatientUserExistsAsync(int userId)
        => await _db.Patients.AnyAsync(p => p.UserID == userId);

    // Persists a new patient record to the database and returns it with the generated PatientID
    public async Task<Patient> CreateAsync(Patient patient)
    {
        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();
        return patient;
    }

    // Fetches all active patients with their related User and Insurance data
    public async Task<List<Patient>> GetAllPatientsAsync()
    {
        return await _db.Patients
            // Exclude soft-deleted patients — Status=false means deleted
            .Where(p => p.Status == true)
            // Join User table to get Email and Phone via navigation property
            .Include(p => p.UserIDNavigator)
            // Join Insurance table to get CoverageAmount; may be null if unlinked
            .Include(p => p.InsuranceIDNavigator)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(int userId)
    {
        return await _db.Patients.Where(p => p.Status == true).Include(p => p.UserIDNavigator).Include(p => p.InsuranceIDNavigator).FirstOrDefaultAsync(p=>p.UserID==userId);
    }

}