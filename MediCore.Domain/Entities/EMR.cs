using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("EMR")]
public class EMR
{
    [Key]
    public int EMRID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public string? Diagnosis { get; set; }
    public string? TreatmentPlan { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
    public virtual Patient? Patient { get; set; } 
    public virtual ICollection<Prescription>? Prescriptions { get; set; }
    public virtual User? Doctor { get; set; } 

     public virtual ICollection<EMRLabReport> EMRLabReport { get; set; } = new List<EMRLabReport>();
}