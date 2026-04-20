using System;
using System.ComponentModel.DataAnnotations;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;

namespace MediCore.Api.DTOs.PrescriptionDtos;

public class PrescriptionRequestDto
{   
    [Required]
    public int EmrID { get; set; }
    [Required]
    public int DoctorID { get; set; }
    public List<PrescriptionItemRequestDto>? PrescriptionItems { get; set; }
}
