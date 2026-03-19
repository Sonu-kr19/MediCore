using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("LabReport")]
public class LabReport
{
    [Key]
    public int LabReportID {get; set;}
    [ForeignKey("LabTestIDNavigator")]
    public int LabTestID {get; set;}
    public string FileURI {get; set;}
    public DateTime Date {get; set;}
    public bool Status {get; set; }

    public virtual LabTest? LabTestIDNavigator {get; set;}
}
