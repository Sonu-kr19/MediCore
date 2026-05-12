

using Microsoft.EntityFrameworkCore;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.PatientDocumentRepo;

public class PatientDocumentRepo : IPatientDocumentRepo
{
    private readonly MediCoreDbContext _db;

    public PatientDocumentRepo(MediCoreDbContext db)
    {
        _db = db;
    }

    // Check patient exists and is active before linking document.
    public async Task<bool> PatientExistsAsync(int patientId)
        => await _db.Patients.AnyAsync(p => p.PatientID == patientId && p.Status == true);

    // Persist document metadata and file bytes in DB.
    public async Task<PatientDocument> CreateAsync(PatientDocument document)
    {
        _db.PatientDocuments.Add(document);
        await _db.SaveChangesAsync();
        return document;
    }

    // Fetch all documents for a specific patient — excludes FileData to keep response light.
    public async Task<List<PatientDocument>> GetByPatientIdAsync(int patientId)
    {
        return await _db.PatientDocuments
            .Where(d => d.PatientID == patientId)
            .ToListAsync();
    }

    // Fetch all documents with FileData — used for downloadable list.
    public async Task<List<PatientDocument>> GetDownloadableByPatientIdAsync(int patientId)
    {
        return await _db.PatientDocuments
            .Where(d => d.PatientID == patientId)
            .ToListAsync();
    }

    // Fetch single document by ID — includes FileData for download.
    public async Task<PatientDocument?> GetByIdAsync(int documentId)
    {
        return await _db.PatientDocuments
            .FirstOrDefaultAsync(d => d.PatientDocumentID == documentId);
    }

    // Update verification status — called by verify endpoint.
    public async Task UpdateAsync(PatientDocument document)
    {
        _db.PatientDocuments.Update(document);
        await _db.SaveChangesAsync();
    }
}