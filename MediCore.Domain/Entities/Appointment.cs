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

    public int PatientID {get; set;} // The ID
    
    public int DoctorID {get; set;}

    public DateOnly Date {get; set;}
    public TimeOnly Time {get; set;}
    public AppointmentStatusOption Status {get; set;}

    [ForeignKey("PatientID")] // Point to the int property
    public virtual Patient? PatientIDNavigator {get; set;}

    [ForeignKey("DoctorID")] // Point to the int property
    public virtual Doctor? Doctor {get; set;}
}