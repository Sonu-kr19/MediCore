using MediCore.Api.DTOs.AuditDtos;
using MediCore.Api.Repositories.AuditRepo;

namespace MediCore.Api.Services.AuditServices
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<GetAuditListResponseDto> GetAuditsAsync(
            GetAuditRequestDto request)
        {
            // Call repository with filters
            var audits = await _auditRepository.GetAuditsAsync(
                adminId: null,                 // Optional, can be extended later
                scope: null,                   // Optional
                findings: request.Findings,
                fromDate: request.FromDate,
                toDate: request.ToDate,
                status: null                   // Optional
            );

            // Map entities to response DTO
            var response = new GetAuditListResponseDto
            {
                Audits = audits.Select(a => new AuditResponseDto
                {
                    AuditID = a.AuditID,
                    AdminID = a.AdminID,
                    Scope = a.Scope,
                    Findings = a.Findings,
                    Date = a.Date,
                    Status = a.Status
                }).ToList(),
                TotalCount = audits.Count
            };

            return response;
        }
    }
}