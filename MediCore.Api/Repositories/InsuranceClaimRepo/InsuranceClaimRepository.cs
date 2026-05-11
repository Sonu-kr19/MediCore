using System.Threading.Tasks;
using System.Linq;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.InsuranceClaimRepo
{
    public class InsuranceClaimRepository:IInsuranceClaimRepository
    {
        private readonly MediCoreDbContext _context;
        public InsuranceClaimRepository(MediCoreDbContext context)
        {
            _context = context;
        }
        public Task<bool> ClaimExistsForBillAsync(int billId)
        {
            bool exists =_context.InsuranceClaims.Any(c => c.BillID == billId);
            return Task.FromResult(exists);
        }
        public Task<bool> InsuranceExistsAsync(int insuranceId)
        {
            Insurance? insurance = _context.Insurances.Find(insuranceId);
            if (insurance == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(insurance.Status);
        }
        public Task<bool> BillExistsAsync(int billId)
        {
            Bill? bill = _context.Bills.Find(billId);
            return Task.FromResult(bill != null);
        }
        public async Task<InsuranceClaim> CreateAsync(InsuranceClaim claim)
        {
            _context.InsuranceClaims.Add(claim);
            await _context.SaveChangesAsync();
            return claim;
        }
        public Task<InsuranceClaim?> GetByIdAsync(int claimId)
        {
            InsuranceClaim? claim = _context.InsuranceClaims.Find(claimId);
            return Task.FromResult(claim);
        }
    }
}