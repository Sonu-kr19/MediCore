using System;
using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.PrescriptionRepo;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly MediCoreDbContext _context;
    public PrescriptionRepository(MediCoreDbContext context)
    {
        _context = context;
    }

    public Task CreatePrescriptionAsync(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        return _context.SaveChangesAsync();
    }
}
