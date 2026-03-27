using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

public class BillItem
{
    public int BillItemID {get; set;}
    [ForeignKey(nameof(BillID))]
    public int BillID {get; set;}
    public string ItemName {get; set;}
    public decimal Rate {get; set;}
    public virtual Bill Bill {get; set;}
}
