using MediCore.Api.DTOs.ComplianceDtos;
using MediCore.Api.DTOs.PatientDtos;

namespace MediCore.Api.Services.ComplianceServices
{
    public interface IComplianceService
    {
        Task<CreateComplianceRecordResponseDto> CreateComplianceRecordAsync(CreateComplianceRecordRequestDto request);
         Task LogComplianceEventAsync(int patientId, string type);
        Task<List<ComplianceResponseDto>> GetAllPendingAsync();
        Task<ComplianceResponseDto> VerifyAsync(int complianceRecordId, ComplianceVerifyDto dto);
        
    }
}
