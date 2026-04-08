using System;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.LabTestRepository;

public class LabTestRepository: ILabTestRepository
{
    private readonly MediCoreDbContext _context;
    public LabTestRepository(MediCoreDbContext context)
    {
        _context = context;
    }
    public  async Task  AddLabTestAsync(LabTest labTest)
    {
        _context.LabTests.AddAsync(labTest);
        await _context.SaveChangesAsync();
    }

    public async Task<List<LabTest>> GetAllLabTestsAsync()
    {
        return  await _context.LabTests.ToListAsync();
    }
}
