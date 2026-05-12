using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.PaymentRepo;

public interface IPaymentRepository
{
    Task<Payment?> GetByReferenceAsync(string paymentReference);
    Task AddPaymentAsync(Payment payment);
    Task<Bill?> GetBillByIdAsync(int billId);
    Task UpdateBillAsync(Bill bill);
    Task SaveChangesAsync();
}
