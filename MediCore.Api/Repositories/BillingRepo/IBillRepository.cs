using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.BillingRepo;

public interface IBillRepository
{
  Task<int> CreateBillAsync(Bill bill);
}
