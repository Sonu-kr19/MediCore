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
    public int PatientID {get; set;}    
    public int DoctorID {get; set;}
    public DateOnly Date {get; set;}
    public TimeOnly Time {get; set;}
    public string IdempotencyKey { get; set; } // for unique appointment between doctor and patient
    public AppointmentStatusOption Status {get; set;}
    public virtual Patient? PatientIDNavigator {get; set;}
    public virtual User? Doctor {get; set;}
}