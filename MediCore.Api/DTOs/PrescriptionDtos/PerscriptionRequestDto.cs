using System.ComponentModel.DataAnnotations;

namespace MediCore.Api.DTOs.PrescriptionDtos{
public class PrescriptionRequestDto
{   
    [Required]
    public int EmrID { get; set; }
    [Required]
    public int DoctorID { get; set; }
    public List<PrescriptionItemRequestDto>? PrescriptionItems { get; set; }
 }
}
