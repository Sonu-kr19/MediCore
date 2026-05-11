using System;

namespace MediCore.Api.DTOs.InsuranceClaimDtos;

public class InsuranceClaimResponseDto
{
    public int ClaimID { get; set; }
    public int BillID { get; set; }
    public int InsuranceID { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime Date { get; set; }
}
