using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Audit")]
public class Audit
{
    public int AuditID { get; set; }
    public int AdminID { get; set; }
    public string Scope { get; set; }
    public string Findings { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
}
