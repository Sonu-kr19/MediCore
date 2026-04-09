using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Insurance")]
public class Insurance
{

    [Key]
    public int InsuranceID { get; set; }

    public string PolicyNumber { get; set; } = null!;
    public string ProviderName { get; set; } = null!;
    public decimal CoverageAmount { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool Status { get; set; }

    // One insurance policy can have many claims.
    public virtual ICollection<InsuranceClaim> InsuranceClaims { get; set; }

}
