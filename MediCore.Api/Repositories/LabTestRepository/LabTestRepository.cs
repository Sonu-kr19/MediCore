using System;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.LabTestRepository;

public class LabTestRepository: ILabTestRepository
{
    private readonly MediCoreDbContext _context;
    public LabTestRepository(MediCoreDbContext context)
    {
        _context = context;
    }
    public  async Task<LabTest>  AddLabTestAsync(LabTest labTest)
    {
        try
        {
            await _context.LabTests.AddAsync(labTest);
            await _context.SaveChangesAsync();
            return labTest;
        }
        catch (System.Exception)
        {
            throw new Exception("An error occurred while adding the lab test. Please try again.");
        }
        
    }

    public Task<bool> DoctorExistsAsync(int doctorId)
    {
        var doctorExists = _context.Users.Any(u => u.UserID == doctorId && u.RoleName.ToString() == "Doctor");
        return Task.FromResult(doctorExists);
    }

    public Task<bool> PatientExistsAsync(int patientId)
    {
        var patientExists = _context.Patients.Any(p => p.PatientID == patientId);
        return Task.FromResult(patientExists);
    }

    public Task<bool> TechnicianExistsAsync(int technicianId)
    {
        var technicianExists = _context.Users.Any(u => u.UserID == technicianId && u.RoleName.ToString() == "Lab_Technician");
        return Task.FromResult(technicianExists);
    }
    public async Task<List<LabTest>> GetPendingLabTestsAsync()
    {
        return  await _context.LabTests.Where(l => l.Status == false).ToListAsync();
    }
}