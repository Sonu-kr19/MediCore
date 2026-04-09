using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Technician")]
public class Technician
{
    [Key]
    public int TechnicianID {get; set;}
    [ForeignKey("UserIDNavigator")]
    public int UserID {get; set;}

    public virtual User? UserIDNavigator {get; set;}
    // public virtual ICollection<LabTest> LabTests {get; set;} = new List<LabTest>();
}
