using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("EMRLabReport")]
public class EMRLabReport
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("EMR")]
    public int EMRID { get; set; }

    [ForeignKey("LabReport")]
    public int LabReportID { get; set; }

    public DateTime AttachedAt { get; set; }

    // Navigation
    public virtual EMR EMR { get; set; } = null!;
    public virtual LabReport LabReport { get; set; } = null!;
}