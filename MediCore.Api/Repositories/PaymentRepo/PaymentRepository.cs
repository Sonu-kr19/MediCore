using System;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace MediCore.Api.Repositories.PaymentRepo;

public class PaymentRepository:IPaymentRepository
{
    private readonly MediCoreDbContext _context;
    public PaymentRepository(MediCoreDbContext context)
    {
        _context = context;
    }
    public async Task<Payment?> GetByReferenceAsync(string paymentReference)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.PaymentReference == paymentReference);
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task<Bill?> GetBillByIdAsync(int billId)
    {
        return await _context.Bills.FindAsync(billId);
    }

    public async Task UpdateBillAsync(Bill bill)
    {
        _context.Bills.Update(bill);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
