using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Prescriptions")]
public class Prescription
{
    [Key]
    public int PrescriptionID { get; set; }
    [ForeignKey("EMRIDNavigator")]
    public int EMRID { get; set; }
    [ForeignKey("DoctorIDNavigator")]
    public int DoctorID { get; set; }
    public DateTime Date {get; set;}
    public bool Status {get; set;}
    public virtual EMR? EMRIDNavigator { get; set; }
    public virtual Doctor? DoctorIDNavigator { get; set; }
    public virtual ICollection<PrescriptionItem>? PrescriptionItems {get; set;}
}
