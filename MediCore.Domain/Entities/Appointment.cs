using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediCore.Domain.Enum;

namespace MediCore.Domain.Entities;

[Table("Appointment")]
public class Appointment
{
    [Key]
    public int AppointmentID {get; set;}
    [ForeignKey("PatientIDNavigator")]
    public int PatientID {get; set;}
    [ForeignKey("DoctorIDNavigator")]
    public int DoctorID {get; set;}
    public DateOnly Date {get; set;}
    public TimeOnly Time {get; set;}
    public AppointmentStatusOption Status {get; set;}

    public virtual Patient? PatientIDNavigator {get; set;}
    public virtual Doctor? DoctorIDNavigator {get; set;}
}
