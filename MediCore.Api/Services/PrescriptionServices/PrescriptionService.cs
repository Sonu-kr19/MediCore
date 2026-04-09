using System;
using System.Linq;
using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Api.Repositories.PrescriptionRepo;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Services.PrescriptionServices;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _repository;
    public PrescriptionService(IPrescriptionRepository repository)
    {
        _repository = repository;
    }
    public async Task<PrescriptionResponseDto> CreatePrescriptionAsync(PrescriptionRequestDto Request)
    {
        if (Request.PrescriptionItems == null || !Request.PrescriptionItems.Any())
            throw new ArgumentException("At least one prescription item is required.");
    
        foreach (var item in Request.PrescriptionItems)
        {
            if (string.IsNullOrWhiteSpace(item.Dosage) || !item.Dosage.Any(char.IsDigit))
                throw new ArgumentException($"Invalid dosage for {item.MedicineName}.");
        }
    
        var newPrescription = new Prescription
        {
            EMRID    = Request.EmrID,
            DoctorID = Request.DoctorID,
            Date     = DateTime.UtcNow,
            Status   = true,
            PrescriptionItems = Request.PrescriptionItems.Select(m => new PrescriptionItem
            {
                Medicine  = m.MedicineName,
                Dosage    = m.Dosage,
                Frequency = m.Frequency,
                Duration  = m.Duration
            }).ToList()
        };
    
        // Single save — EF Core inserts Prescription + all PrescriptionItems
        // in one transaction and wires up the FK (PrescriptionID) automatically.
        await _repository.CreatePrescriptionAsync(newPrescription);
    
        return new PrescriptionResponseDto
        {
            PrescriptionID       = newPrescription.PrescriptionID,
            EmrId                = Request.EmrID,
            TotalPrescriptionItems = newPrescription.PrescriptionItems.Count
        };
    }
}

