using System;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Domain.Entities;

public class Notification
{
    [Key]
    public int NotificationID { get; set; }
    public int UserID { get; set; }
    public int PatientID { get; set; }
    public string Message { get; set; }
    public string Category { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
}
