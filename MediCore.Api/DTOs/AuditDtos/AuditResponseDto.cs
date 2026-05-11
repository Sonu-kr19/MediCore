using System;

namespace MediCore.Api.DTOs.AuditDtos
{
    public class AuditResponseDto
    {
        public int AuditID { get; set; }

        public int AdminID { get; set; }

        public string Scope { get; set; }

        public string Findings { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }
    }
}