using System;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Domain.Entities;

public class AuditLog
{
    [Key]
    public int AuditLogID { get; set; }
    public int UserID { get; set; }
    public string Action { get; set; }
    public string Resource { get; set; }
    public DateTime Timestamp { get; set; }
}
