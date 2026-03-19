using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("PatientDocument")]
public class PatientDocument
{
    [Key]
    public int PatientDocumentID {get; set;}
    [ForeignKey("PatientIDNavigator")]
    public int PatientID {get; set;}
    public string DocType {get; set;}
    public string FileURI {get; set;}
    public DateTime UploadedDate {get; set;}
    public bool VerificationStatus {get; set;}

    public virtual Patient? PatientIDNavigator {get; set;}
}
