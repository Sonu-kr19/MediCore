using MediCore.Api.DTOs.UserDtos;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Services.EMR;

public class EmrService : IEmrService
{
    private readonly MediCoreDbContext _context;

    public EmrService(MediCoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmrDto>> GetEmrAsync(int patientId)
    {
        return await _context.EMRs
            .Where(e => e.PatientID == patientId)
            .OrderByDescending(e => e.Date)
            .Select(e => new EmrDto
            {
                EmrId = e.EMRID,
                Date = e.Date,
                Diagnosis = e.Diagnosis,
                TreatmentPlan = e.TreatmentPlan,

                Prescriptions = _context.Prescriptions
                    .Where(p => p.EMRID == e.EMRID)
                    .ToList()
            })
            .ToListAsync();
    }
}