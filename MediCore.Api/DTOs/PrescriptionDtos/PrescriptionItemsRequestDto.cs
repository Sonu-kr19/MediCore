using System.ComponentModel.DataAnnotations;

namespace MediCore.Api.DTOs.PrescriptionDtos
{
    public class PrescriptionItemRequestDto
    {
        [Required]
        public string MedicineId { get; set; }

        [Required]
        public string Dosage { get; set; }

        [Required]
        public string Frequency { get; set; }

        [Required]
        public string DurationInDays { get; set; }
    }
}