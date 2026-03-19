using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediCore.Domain.Enum;

namespace MediCore.Domain.Entities;

[Table("Payment")]
public class Payment
{
    [Key]
    public int PaymentID {get; set;}
    [ForeignKey("BillIDNavigator")]
    public int BillID {get; set;}
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount {get; set;}
    public DateTime Date {get; set;}
    public PaymentOption Method {get; set;}
    public bool Status {get; set;}

    public virtual Bill? BillIDNaviagator{get; set;}
}
