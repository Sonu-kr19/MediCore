using System;

using System.ComponentModel.DataAnnotations;
using MediCore.Domain.Enum;

namespace MediCore.Api.DTOs.PatientDtos;

// DTO used to receive patient creation/update data from the client (API request body)
public class PatientRequestDto
{
    // links this patient record to an existing user account in the system
    [Required]
    public int UserID { get; set; }

    // full name of the patient, used for display and identification
    [Required]
    public string Name { get; set; } = null!;

    // date of birth to calculate age and support medical history tracking
    [Required]
    public DateOnly DOB { get; set; }

    // gender of the patient, stored as an enum to enforce a fixed set of valid options
    [Required]
    public GenderOption Gender { get; set; }

    // patient's residential address, capped at 300 chars to prevent oversized input
    [Required, MaxLength(300)]
    public string Address { get; set; } = null!;

    // Optional insurance ID; null means the patient has no linked insurance plan
    public int? InsuranceID { get; set; }

}