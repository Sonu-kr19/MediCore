using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Dispense")]
public class Dispense
{
    [Key]
    public int DispenseID { get; set; }
    [ForeignKey("PrescriptionIDNavigator")]
    public int PrescriptionID { get; set; }
    public int? PharmacistID { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public virtual Prescription? PrescriptionIDNavigator { get; set; }
    public virtual Pharmacist? PharmacistIDNavigator { get; set; }
}
