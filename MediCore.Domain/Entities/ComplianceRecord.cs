using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("ComplianceRecord")]
public class ComplianceRecord
{
    [Key]
    public int ComplianceRecordID {get; set;}
    public int PatientID {get; set;}
    public string Type {get; set;}
    public string Result {get; set;}
    public DateTime Date {get;set;}
    public string Note {get; set;}

    public virtual Patient? PatientIDNavigator {get; set;}

}
