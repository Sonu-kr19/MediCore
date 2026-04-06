using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("TreatmentLog")]
public class TreatmentLog
{
    [Key]
    public int TreatmentLogID { get; set; }
    [ForeignKey("EMRIDNavigator")]
    public int EMRID { get; set; }
    [ForeignKey("NurseIDNavigator")]
    public int? NurseID { get; set; }
    public string Notes { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
    public virtual EMR? EMRIDNavigator { get; set; }
    public virtual Nurse? NurseIDNavigator { get; set; }
}
