using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("LabTest")]
public class LabTest
{
    [Key]
    public int LabTestID {get; set;}
    public int PatientID {get; set;}
    public int DoctorID {get; set;}
    public int? TechnicianID {get; set;}
    public string Type {get; set;}
    public DateTime Date {get; set;}
    public int? TechnicianID {get; set;}
    public bool Status {get; set;}

    public virtual LabReport? LabReport {get; set;}
    public virtual Patient? PatientIDNavigator { get; set; }
    public virtual User? Doctor {get; set;}
    public virtual User? TechnicianIDNavigator { get; set; }
}
