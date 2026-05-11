using System;
namespace MediCore.Api.DTOs.AuditDtos
{
    public class GetAuditListResponseDto
    {
        public List<AuditResponseDto> Audits { get; set; } = new();

        public int TotalCount { get; set; }
    }
}