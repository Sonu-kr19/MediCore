using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.LabTestRepository;

public interface ILabTestRepository
{
    Task AddLabTestAsync(LabTest labTest);
    Task<List<LabTest>> GetAllLabTestsAsync();
}
