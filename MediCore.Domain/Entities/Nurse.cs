using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;
[Table("Nurse")]
public class Nurse
{
    [Key]
    public int NurseID { get; set; }
    [ForeignKey("UserIDNavigator")]
    public int UserID { get; set; }
    public virtual User? UserIDNavigator { get; set; }
    public virtual ICollection<ComplianceRecord> ComplianceRecords {get; set;}=new List<ComplianceRecord>();    
}
