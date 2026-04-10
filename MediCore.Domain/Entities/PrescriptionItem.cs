using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

public class PrescriptionItem
{
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PrescriptionItemID {get; set;}
    [ForeignKey("Prescription")]
    public int PrescriptionID {get; set;}
    public string Medicine {get;set;}
    public string Dosage {get; set;}
    public string Frequency {get; set;}
    public string Duration {get;set;}
    public virtual Prescription Prescription {get; set;}
}
