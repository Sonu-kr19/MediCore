
using MediCore.Api.DTOs.PatientDtos;

namespace MediCore.Api.Services.PatientDocumentServices;

//for uploaddocument into database we need upload document
// for seeing document we rrquired get document for patient
// for storing into database we used file stream
//after uploading documents we have to update the status
public interface IPatientDocumentService
{
    Task<PatientDocumentResponseDto> UploadDocumentAsync(int patientId, PatientDocumentRequestDto dto);
    Task<List<PatientDocumentDetailsDto>> GetDocumentsByPatientAsync(int patientId);
    Task<(byte[] fileBytes, string fileName, string contentType)> DownloadDocumentAsync(int patientId);
    Task<PatientDocumentDetailsDto> VerifyDocumentAsync(int patientId, int documentId);
}