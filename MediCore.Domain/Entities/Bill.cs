using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Bill")]
public class Bill
{
    [Key]
    public int BillID {get; set;}
    [ForeignKey(nameof(PatientID))]
    public int PatientID {get; set;}
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount {get; set;}
    [Column(TypeName = "decimal(18,2)")]
    public decimal PaidAmount { get; set; } = 0;
    public DateTime Date {get; set;}
    public bool Status {get; set;}

    public virtual Patient Patient {get; set;}
    public virtual ICollection<BillItem> BillItems {get;set;}
    
}
