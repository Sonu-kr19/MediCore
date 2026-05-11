using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.InsuranceClaimRepo;

public interface IInsuranceClaimRepository
{
    Task<bool> ClaimExistsForBillAsync(int billId);
    Task<bool> InsuranceExistsAsync(int insuranceId);
    Task<bool> BillExistsAsync(int billId);
    Task<InsuranceClaim> CreateAsync(InsuranceClaim claim);
    Task<InsuranceClaim?> GetByIdAsync(int claimId);
}
