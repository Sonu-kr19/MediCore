using MediCore.Api.DTOs.EmrDtos;
using MediCore.Api.DTOs.LabReportDtos;
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
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == emrRequest.DoctorID);

            if (doctor == null)
                throw new Exception("Doctor not found");

            var emr = new Domain.Entities.EMR
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

            // Auto-create treatment log
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
        var response = await _context.EMRs
            .Where(e => e.PatientID == patientId)
            .Include(e => e.Prescriptions)
            .ThenInclude(p => p.PrescriptionItems) 
            .Include(e => e.EMRLabReport)
                .ThenInclude(el => el.LabReport)
            .OrderByDescending(e => e.Date)
            .Select(e => new EmrResponseDto
            {
                EmrId = e.EMRID,
                Date = e.Date,
                Diagnosis = e.Diagnosis,
                TreatmentPlan = e.TreatmentPlan,
                Prescriptions = e.Prescriptions == null ? null
                : e.Prescriptions.Select(p => new PrescriptionSummaryDto
        {
            PrescriptionID = p.PrescriptionID,
            Date = p.Date,
            Status = p.Status,
            PrescriptionItems = p.PrescriptionItems == null ? null
                : p.PrescriptionItems.Select(pi => new PrescriptionItemSummaryDto
                {
                    PrescriptionItemID = pi.PrescriptionItemID,
                    Medicine = pi.Medicine,
                    Dosage = pi.Dosage,
                    Frequency = pi.Frequency,
                    Duration = pi.Duration
                })
                .ToList()
            })
            .ToList(),
            LabReports = e.EMRLabReport
                .Where(el => el.LabReport.Status == true)
                .Select(el => new LabReportSummaryDto
                {
                    EmrId = el.EMRID,
                    LabReportID = el.LabReport.LabReportID,
                    LabTestID = el.LabReport.LabTestID,
                    FileURI = el.LabReport.FileURI,
                    Date = el.LabReport.Date,
                    Status = el.LabReport.Status
                })
                .ToList()
        })
        .ToListAsync();

            return response;
    }

    //ATTACH LAB REPORT TO EMR
    public async Task<LabReportResponseDto> AttachLabReportAsync(LabReportRequestDto request)
    {
        try
        {
            // 1. Check EMR exists
            var emr = await _context.EMRs
                .FirstOrDefaultAsync(e => e.EMRID == request.EmrId);

            if (emr == null)
                throw new KeyNotFoundException($"EMR with ID {request.EmrId} not found.");

            // 2. Check LabReport exists and is active
            var labReport = await _context.LabReports
                .FirstOrDefaultAsync(lr => lr.LabReportID == request.LabReportID
                                        && lr.Status == true);

            if (labReport == null)
                throw new KeyNotFoundException(
                    $"Lab report with ID {request.LabReportID} not found or inactive.");

            // 3. Prevent duplicate attachment
            var alreadyAttached = await _context.EMRLabReport
                .AnyAsync(el => el.EMRID == request.EmrId
                             && el.LabReportID == request.LabReportID);

            if (alreadyAttached)
                throw new InvalidOperationException(
                    $"Lab report {request.LabReportID} is already attached to EMR {request.EmrId}.");

            // 4. Create link record
            var attachment = new EMRLabReport
            {
                EMRID = request.EmrId,
                LabReportID = request.LabReportID,
                AttachedAt = DateTime.UtcNow
            };

            _context.EMRLabReport.Add(attachment);

            // 5. Auto treatment log
            var treatmentLog = new TreatmentLog
            {
                EMRID = request.EmrId,
                Notes = $"Lab report #{labReport.LabReportID} attached (File: {labReport.FileURI})."
            };

            _context.TreatmentLogs.Add(treatmentLog);

            await _context.SaveChangesAsync();

            return new LabReportResponseDto
            {
                EmrId = attachment.EMRID,
                LabReportID = labReport.LabReportID,
                LabTestID = labReport.LabTestID,
                FileURI = labReport.FileURI,
                Date = labReport.Date,
                Status = labReport.Status,
                CreatedAt = attachment.AttachedAt
            };
        }
        catch (DbUpdateException ex)
        {
            throw new Exception(ex.InnerException?.Message);
        }
    }
}