using System;

namespace MediCore.Api.DTOs.BillingDtos;

public class CreateBillDto
{
   
    public int PatientID { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
    public List<CreateBillItemDto> BillItems { get; set; }

}
