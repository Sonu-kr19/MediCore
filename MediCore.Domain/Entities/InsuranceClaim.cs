using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;
[Table("InsuranceClaim")]
public class InsuranceClaim
{
    [Key]
    public int ClaimID { get; set; }
    [ForeignKey("Insurance")]
    public int InsuranceID { get; set; }
    [ForeignKey("User")]
    public int UserID { get; set; }
    [ForeignKey("Bill")]
    public int BillID { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public virtual Bill Bill { get; set; }
    public virtual User User { get; set; }
    public virtual Insurance? Insurance { get; set; }
}
