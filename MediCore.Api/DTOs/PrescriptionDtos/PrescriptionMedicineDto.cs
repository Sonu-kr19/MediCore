using System;

namespace MediCore.Api.DTOs.PrescriptionDtos;

public class PrescriptionMedicineDto
{
        public string? Medicine { get; set; }
        public string? MedicineName { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
}
