using System;

namespace MediCore.Domain.Entities;

public class Audit
{
    public int AuditID { get; set; }
    public int AdminID { get; set; }
    public string Scope { get; set; }
    public string Findings { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
}
