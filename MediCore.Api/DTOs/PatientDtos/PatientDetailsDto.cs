using System;
using MediCore.Domain.Enum;

namespace MediCore.Api.DTOs.PatientDtos;

// DTO used to send full patient details back to the client (API response body)
// Combines data from Patient, User, and Insurance tables into one flat response
public class PatientDetailsDto
{
    // --- Patient Table Fields ---

    // Unique identifier of the patient record, used for lookups and updates
    public int PatientID { get; set; }

    // Full name of the patient for display purposes
    public string Name { get; set; } = null!;

    // Contact email pulled from the linked User account
    public string Email { get; set; } = null!;

    // Contact phone number pulled from the linked User account
    public string Phone { get; set; } = null!;

    // Date of birth used for age calculation and medical history context
    public DateOnly DOB { get; set; }

    // Gender stored as enum to ensure only valid predefined options are returned
    public GenderOption Gender { get; set; }

    // Residential address of the patient
    public string Address { get; set; } = null!;

    // --- Insurance Table Fields ---

    // Foreign key to the insurance plan; null means patient has no insurance linked
    public int? InsuranceID { get; set; }

    // Coverage amount from the linked insurance plan; null if no insurance is associated
    public decimal? InsuranceAmount { get; set; }

    // --- User Table Fields ---

    // TODO: add User-related fields here (e.g. UserID, Role, CreatedAt)
}