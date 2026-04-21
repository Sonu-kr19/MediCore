using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Prescriptions")]
public class Prescription
{
    [Key]
    public int PrescriptionID { get; set; }
    public int EMRID { get; set; }
    public int DoctorID { get; set; }
    public DateTime Date {get; set;}
    public bool Status {get; set;}
    public virtual EMR? EMR { get; set; }
    [ForeignKey("DoctorID")]
    public virtual User? Doctor { get; set; }
    public virtual ICollection<PrescriptionItem>? PrescriptionItems {get; set;}
}
