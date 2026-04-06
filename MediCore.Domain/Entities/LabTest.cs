using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("LabTest")]
public class LabTest
{
    [Key]
    public int LabTestID {get; set;}
    [ForeignKey("PatientIDNavigator")]
    public int PatientID {get; set;}
    public int DoctorID {get; set;}
    public string Type {get; set;}
    public DateTime Date {get; set;}
    [ForeignKey("TechnicianIDNavigator")]
    public int? TechnicianID {get; set;}
    public bool Status {get; set;}

    public virtual LabReport? LabReport {get; set;}
    public virtual Patient? PatientIDNavigator { get; set; }
    [ForeignKey("DoctorID")]
    public virtual User? Doctor {get; set;}
    public virtual Technician? TechnicianIDNavigator { get; set; }
}
