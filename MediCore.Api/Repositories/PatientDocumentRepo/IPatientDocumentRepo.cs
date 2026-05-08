
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.PatientDocumentRepo;

//for document upload we required if already patient exist or not
// and create document upload for patient
//for downloading files we need patient id
// for download using file stream 
// after upload for verify the status we need update
public interface IPatientDocumentRepo
{
    Task<bool> PatientExistsAsync(int patientId);
    Task<PatientDocument> CreateAsync(PatientDocument document);
    Task<List<PatientDocument>> GetByPatientIdAsync(int patientId);
    Task<List<PatientDocument>> GetDownloadableByPatientIdAsync(int patientId);
    Task<PatientDocument?> GetByIdAsync(int documentId);
    Task UpdateAsync(PatientDocument document);
}