using MediCore.Api.DTOs.ComplianceDtos;

namespace MediCore.Api.Services.ComplianceServices
{
    public interface IComplianceService
    {
        Task<CreateComplianceRecordResponseDto> CreateComplianceRecordAsync(CreateComplianceRecordRequestDto request);
    }
}
