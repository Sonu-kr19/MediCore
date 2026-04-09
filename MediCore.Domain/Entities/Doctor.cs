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
}
