using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Schedule")]
public class Schedule
{
    [Key]
    public int ScheduleID {get; set;}
    public int DoctorID {get; set;}
    public DateOnly Date {get; set;}
    public TimeOnly TimeSlot {get; set;}
    public bool Availability {get; set;}

    public virtual Doctor? DoctorIDNavigator {get; set;}
}
