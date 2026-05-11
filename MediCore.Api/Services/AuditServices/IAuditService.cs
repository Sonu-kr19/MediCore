using MediCore.Api.DTOs.AuditDtos;

namespace MediCore.Api.Services.AuditServices
{
    public interface IAuditService
    {
        Task<GetAuditListResponseDto> GetAuditsAsync(
            GetAuditRequestDto request
        );
    }
}