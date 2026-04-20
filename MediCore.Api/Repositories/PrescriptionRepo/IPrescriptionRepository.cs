using System;
using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Domain.Entities;
namespace MediCore.Api.Repositories.PrescriptionRepo;
public interface IPrescriptionRepository
{
    Task<Prescription> CreatePrescriptionAsync(Prescription prescription);
    Task<List<Prescription>> GetQueuedPrescriptionsAsync(int pageNumber, int pageSize);
    Task<int> GetQueuedPrescriptionsCountAsync();
}

