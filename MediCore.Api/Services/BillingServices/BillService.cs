using System;
using MediCore.Api.DTOs.BillingDtos;
using MediCore.Api.Repositories.BillingRepo;
using MediCore.Api.Repositories.PatientRepo;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services;

public class BillService:IBillService
{
   
    private readonly IBillRepository _repository;
     private readonly IPatientRepository _patrepo;

    public BillService(IBillRepository repository,IPatientRepository repo)
    {
        _repository = repository;
        _patrepo=repo;
    }
    
 public async Task<int> CreateBillAsync(CreateBillDto dto)
    {  
        bool patientExists = await _patrepo.PatientExistsAsync(dto.PatientID);
        if (patientExists == false)
        {
            throw new Exception(ErrorMessage.PatientNotFound);
        }

        //Validate BillItems
        if (dto.BillItems == null)
        {
            throw new Exception(ErrorMessage.BillItemsNotNull);
        }
        if (dto.BillItems.Count == 0)
        {
            throw new Exception(ErrorMessage.BillItemRequired);
        }
        decimal totalAmount = 0;
        List<BillItem> billItemEntities = new List<BillItem>();

        //Validate BillItems
        foreach (CreateBillItemDto itemDto in dto.BillItems)
        {
            if (string.IsNullOrWhiteSpace(itemDto.ItemName))
            {
                throw new Exception(ErrorMessage.ItemNameNotEmpty);
            }
            if (itemDto.Rate <= 0)
            {
                throw new Exception(ErrorMessage.RateGreaterThanZero);
            }
            BillItem billItem = new BillItem();
            billItem.ItemName = itemDto.ItemName;
            billItem.Rate = itemDto.Rate;
            billItemEntities.Add(billItem);
            totalAmount = totalAmount + itemDto.Rate;
        }

        //Create Bill entity
        Bill bill = new Bill();
        bill.PatientID = dto.PatientID;
        bill.Date = dto.Date;
        bill.Status = dto.Status;
        bill.Amount = totalAmount;
        bill.BillItems = billItemEntities;

        //Save asynchronously
        int billId = await _repository.CreateBillAsync(bill);
        return billId;
    }

}
