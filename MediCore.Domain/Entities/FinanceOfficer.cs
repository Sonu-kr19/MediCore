using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Domain.Entities;

[Table("FinanceOfficer")]
public class FinanceOfficer
{
    [Key]
    public int FinanceOfficerID {get; set;}
    public int UserID {get; set;}
}
