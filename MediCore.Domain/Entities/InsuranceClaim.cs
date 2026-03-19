using System;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Domain.Entities;

public class InsuranceClaim
{
    [Key]
    public int InsuranceID { get; set; }
    public int UserID { get; set; }
    public int BillID { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public virtual Bill Bill { get; set; }
    public virtual User User { get; set; }
}
