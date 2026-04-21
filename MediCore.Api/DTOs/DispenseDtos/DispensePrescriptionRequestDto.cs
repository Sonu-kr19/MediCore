
using System.ComponentModel.DataAnnotations;

namespace MediCore.Api.DTOs.DispenseDtos
{
    public class DispensePrescriptionRequestDto
    {
        [Required(ErrorMessage = "prescriptionId is required")]
        public int PrescriptionID { get; set; }
    }
}
