using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Pharmacist")]
public class Pharmacist
{
    [Key]
    public int PharmacistID { get; set; }
    [ForeignKey("UserIDNavigator")]
    public int UserID { get; set; }
    public virtual User? UserIDNavigator { get; set; }
    public virtual ICollection<Prescription>? Prescriptions { get; set; }
    public virtual ICollection<Medicine>? Medicines { get; set; }
    public virtual ICollection<Dispense>? Dispenses { get; set; }
}
