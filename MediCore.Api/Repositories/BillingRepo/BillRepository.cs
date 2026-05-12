using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.BillingRepo;

public class BillRepository:IBillRepository
{
   
private readonly MediCoreDbContext _context;

    public BillRepository(MediCoreDbContext context)
    {
        _context = context;
    }

     //Bill inserted
     //BillID generated
     //BillItems inserted with BillID
    public async Task<int> CreateBillAsync(Bill bill)
    {     
      var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return bill.BillID;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
