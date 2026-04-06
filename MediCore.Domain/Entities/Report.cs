using System;

namespace MediCore.Domain.Entities;

public class Report
{
    public int ReportID { get; set; }
    public string Scope { get; set; }
    public string Metrics { get; set; } 
    public DateTime GeneratedDate { get; set; }
}
