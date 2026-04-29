
using System.Collections.Generic;

namespace MediCore.Api.DTOs.DispenseDtos
{
    public class DispensePrescriptionResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        // Low-stock warnings (if any)
        public List<string> Warnings { get; set; } = new();
    }
}
