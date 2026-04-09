using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediCore.Domain.Enum;
namespace MediCore.Domain.Entities;
[Table("Patient")]
public class Patient
{
    [Key]
    public int PatientID {get; set;}
    [ForeignKey("UserIDNavigator")]
    public int UserID {get; set;}
    public DateOnly DOB {get; set;}
    public GenderOption Gender {get; set;}
    public string Address {get; set;}
    [ForeignKey("InsuranceIDNavigator")]
    public int? InsuranceID {get; set;}
    public bool Status {get; set;}
    public virtual User? UserIDNavigator {get; set;}
    public virtual InsuranceClaim? InsuranceIDNavigator {get; set;}
    public virtual ICollection<PatientDocument> PatientDocuments {get; set;}
    public virtual ICollection<Appointment> Appointments {get; set;}
    public virtual ICollection<EMR> EMRs {get; set;}
    public virtual ICollection<Bill> Bills {get; set;}
    public virtual ICollection<LabTest> LabTests {get; set;}
    public virtual ICollection<ComplianceRecord> ComplianceRecords {get; set;}
}
