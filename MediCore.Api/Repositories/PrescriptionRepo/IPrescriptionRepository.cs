using System;
using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Domain.Entities;
namespace MediCore.Api.Repositories.PrescriptionRepo;

public interface IPrescriptionRepository
{
    Task CreatePrescriptionAsync(Prescription prescription);
}
