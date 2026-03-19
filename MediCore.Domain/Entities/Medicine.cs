using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("Medicine")]
public class Medicine
{
    [Key]
    public int MedicineID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Stock { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool Status { get; set; }
    
}
