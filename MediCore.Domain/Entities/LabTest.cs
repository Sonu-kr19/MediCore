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
    public bool Status {get; set;}

    public virtual LabReport? LabReport {get; set;}

    [ForeignKey("PatientID")] // Link to PatientID
    public virtual Patient? PatientIDNavigator { get; set; }

    [ForeignKey("DoctorID")] // Link to DoctorID
    public virtual Doctor? Doctor {get; set;}

    [ForeignKey("TechnicianID")] // Link to TechnicianID
    public virtual Technician? TechnicianIDNavigator { get; set; }
}