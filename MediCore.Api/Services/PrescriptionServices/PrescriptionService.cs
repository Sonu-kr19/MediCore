using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Domain.Entities;
using MediCore.Api.DTOs.Common;
using MediCore.Api.Repositories.PrescriptionRepo;

namespace MediCore.Api.Services.PrescriptionServices;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _repository;
    public PrescriptionService(IPrescriptionRepository repository)
    {
        _repository = repository;
    }
    public async Task<PrescriptionResponseDto> CreatePrescriptionAsync(PrescriptionRequestDto Request)
    {
        if (Request.PrescriptionItems == null || !Request.PrescriptionItems.Any())
            throw new ArgumentException("At least one prescription item is required.");
    
        foreach (var item in Request.PrescriptionItems)
        {
            if (string.IsNullOrWhiteSpace(item.Dosage) || !item.Dosage.Any(char.IsDigit))
                throw new ArgumentException($"Invalid dosage for {item.Medicine}.");
        }
    
        var newPrescription = new Prescription
        {
            EMRID    = Request.EmrID,
            DoctorID = Request.DoctorID,
            Date     = DateTime.UtcNow,
            Status   = true,
            PrescriptionItems = Request.PrescriptionItems.Select(m => new PrescriptionItem
            {
                Medicine  = m.Medicine,
                Dosage    = m.Dosage,
                Frequency = m.Frequency,
                Duration  = m.Duration
            }).ToList()
        };
    
        // Single save — EF Core inserts Prescription + all PrescriptionItems
        // in one transaction and wires up the FK (PrescriptionID) automatically.
        var savedPrescription = await _repository.CreatePrescriptionAsync(newPrescription);
    
        return new PrescriptionResponseDto
        {
        PrescriptionID = savedPrescription.PrescriptionID,
        EmrId = savedPrescription.EMRID,
        // TotalPrescriptionItems = savedPrescription.PrescriptionItems.Count,
        PrescriptionItems = savedPrescription.PrescriptionItems.Select(item => new PrescriptionItemRequestDto
        {
            Medicine = item.Medicine,
            Dosage = item.Dosage,
            Frequency = item.Frequency,
            Duration = item.Duration
        }).ToList()
    };
    }
    public async Task<PaginationResponseDto<QueuedPrescriptionDto>> GetQueuedPrescriptionsAsync(int pageNumber, int pageSize)
        {
            //  Get data from repository
            List<Prescription> prescriptions = await _repository.GetQueuedPrescriptionsAsync(pageNumber, pageSize);

            int totalCount = await _repository.GetQueuedPrescriptionsCountAsync();

            //  Convert Prescription entity to DTO manually
            List<QueuedPrescriptionDto> dtoList =
                new List<QueuedPrescriptionDto>();

            foreach (Prescription prescription in prescriptions)
            {
                QueuedPrescriptionDto dto =
                    new QueuedPrescriptionDto();

                dto.PrescriptionID = prescription.PrescriptionID;
                dto.EMRID = prescription.EMRID;
                dto.DoctorID = prescription.DoctorID;
                dto.Date = prescription.Date;

                dtoList.Add(dto);
            }

            //  Prepare paginated response
            PaginationResponseDto<QueuedPrescriptionDto> response =
                new PaginationResponseDto<QueuedPrescriptionDto>();

            response.PageNumber = pageNumber;
            response.PageSize = pageSize;
            response.TotalCount = totalCount;
            response.Data = dtoList;

            return response;
        }
}


