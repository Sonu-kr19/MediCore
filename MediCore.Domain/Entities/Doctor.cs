using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediCore.Domain.Enum;

namespace MediCore.Domain.Entities;

[Table("Doctor")]
public class Doctor
{
    [Key]
    public int DoctorID {get; set;}
    [ForeignKey("UserIDNavigator")]
    public int UserID {get;set;}
    public DateOnly DOB {get; set;}
    public GenderOption Gender {get; set;}
    public string Speciality {get; set;}
    public bool Status {get; set;}

    public virtual User? UserIDNavigator {get; set;}
    public virtual ICollection<EMR> EMRs {get;set;}
    public virtual ICollection<Schedule> Schedules {get; set;}=new List<Schedule>();
    public virtual ICollection<Appointment> Appointments {get; set;}=new List<Appointment>();
    public virtual ICollection<Prescription> Prescriptions {get; set;}=new List<Prescription>();
    // public virtual ICollection<LabTest> LabTests {get; set;}
}
