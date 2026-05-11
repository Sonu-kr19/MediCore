
using System;

namespace MediCore.Api.DTOs.AuditDtos
{
    public class GetAuditRequestDto
    {
        public string? Findings { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
