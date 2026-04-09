using MediCore.Api.DTOs.Common;
using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Api.Repositories.PrescriptionRepo;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.PrescriptionServices
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;

        public PrescriptionService(IPrescriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponseDto<QueuedPrescriptionDto>>
            GetQueuedPrescriptionsAsync(int pageNumber, int pageSize)
        {
            //  Get data from repository
            List<Prescription> prescriptions =
                await _repository.GetQueuedPrescriptionsAsync(pageNumber, pageSize);

            int totalCount =
                await _repository.GetQueuedPrescriptionsCountAsync();

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
}