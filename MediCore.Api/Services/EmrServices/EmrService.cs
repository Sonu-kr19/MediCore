using MediCore.Api.DTOs.EmrDtos;
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

    //CREATE EMR + AUTO TREATMENT LOG
    public async Task<EmrResponseDto> CreateEmrAsync(EmrRequestDto emrRequest)
    {
        try
        {
            // Check if Doctor exists
            var doctor = await _context.Users.FirstOrDefaultAsync(u=>u.UserID == emrRequest.DoctorID);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            var emr = new MediCore.Domain.Entities.EMR
            {
                PatientID = emrRequest.PatientID,
                DoctorID = emrRequest.DoctorID,
                Diagnosis = emrRequest.Diagnosis,
                TreatmentPlan = emrRequest.TreatmentPlan,
                Date = DateTime.UtcNow,
                Status = true
            };

            _context.EMRs.Add(emr);
            await _context.SaveChangesAsync();

            //Auto-create treatment log
            var treatmentLog = new TreatmentLog
            {
                EMRID = emr.EMRID,
                Notes = "EMR record created"
            };

            _context.TreatmentLogs.Add(treatmentLog);
            await _context.SaveChangesAsync();

            return new EmrResponseDto
            {
                EmrId = emr.EMRID,
                Date = emr.Date,
                Diagnosis = emr.Diagnosis,
                TreatmentPlan = emr.TreatmentPlan
            };
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }

    //GET EMR SUMMARY
    public async Task<List<EmrResponseDto>> GetEmrAsync(int patientId)
    {
        return await _context.EMRs
            .Where(e => e.PatientID == patientId)
            .Include(e => e.Prescriptions) 
            .OrderByDescending(e => e.Date)
            .Select(e => new EmrResponseDto
            {
                EmrId = e.EMRID,
                Date = e.Date,
                Diagnosis = e.Diagnosis,
                TreatmentPlan = e.TreatmentPlan,
                Prescriptions = e.Prescriptions.ToList()
            })
        .ToListAsync();
    }
}