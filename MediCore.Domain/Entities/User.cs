using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediCore.Domain.Enum;

namespace MediCore.Domain.Entities;

[Table("User")]
public class User
{
    [Key]
    public int UserID { get; set; }
    public RoleOption RoleName { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Email {get; set; }
    [Required]
    [MaxLength(255)]
    public string Password { get; set; }
    [MaxLength(15)]
    public string Phone {get; set;}
    public bool Status {get; set;}
    public virtual Patient? Patient {get; set;}
    public virtual Doctor? Doctor {get; set;}
    public virtual Nurse? Nurse {get; set;}
    public virtual Technician? Technician {get; set;}
    public virtual ICollection<Schedule> Schedules {get; set;}=new List<Schedule>();
    public virtual ICollection<Appointment> Appointments {get; set;}=new List<Appointment>();
}
