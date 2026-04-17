using System.ComponentModel.DataAnnotations;

namespace MediCore.Api.DTOs.PrescriptionDtos
{
    public class PrescriptionRequestDto
    {
        [Required]
        public int DoctorID { get; set; }

        [Required]
        public List<PrescriptionItemRequestDto> PrescriptionItems { get; set; }=new();
    }
}