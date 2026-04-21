using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Api.DTOs.Common;
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

        // Create new prescription (queued by default)
        public async Task<PrescriptionResponseDto> CreatePrescriptionAsync(
            PrescriptionRequestDto request)
        {
            if (request.PrescriptionItems == null || !request.PrescriptionItems.Any())
                throw new ArgumentException("At least one prescription item is required.");

            var newPrescription = new Prescription
            {
                DoctorID = request.DoctorID,
                Date = DateTime.UtcNow,
                Status = false,
                PrescriptionItems = request.PrescriptionItems.Select(item =>
                    new PrescriptionItem
                    {
                        Medicine = item.Medicine,
                        Dosage = item.Dosage,
                        Frequency = item.Frequency,
                        Duration = item.Duration
                    }).ToList()
            };

            await _repository.CreatePrescriptionAsync(newPrescription);

            return new PrescriptionResponseDto
            {
                PrescriptionID = newPrescription.PrescriptionID,
                DoctorID = newPrescription.DoctorID,
                TotalPrescriptionItems = newPrescription.PrescriptionItems.Count
            };
        }

        // Get queued prescriptions with optional doctor filter
        public async Task<PaginationResponseDto<QueuedPrescriptionDto>>
            GetQueuedPrescriptionsAsync(int pageNumber, int pageSize, int? doctorId)
        {
            var prescriptions =
                await _repository.GetQueuedPrescriptionsAsync(
                    pageNumber, pageSize, doctorId);

            int totalCount =
                await _repository.GetQueuedPrescriptionsCountAsync(doctorId);

            var dtoList = prescriptions.Select(p => new QueuedPrescriptionDto
            {
                PrescriptionID = p.PrescriptionID,
                DoctorID = p.DoctorID,
                DoctorName = p.Doctor != null ? p.Doctor.Name : string.Empty,
                Date = p.Date,
                Medicines = p.PrescriptionItems.Select(i =>
                    new PrescriptionMedicineDto
                    {
                        Medicine = i.Medicine,
                        MedicineName = i.Medicine.ToString(),
                        Dosage = i.Dosage,
                        Frequency = i.Frequency,
                        Duration = i.Duration
                    }).ToList()
            }).ToList();

            return new PaginationResponseDto<QueuedPrescriptionDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = dtoList
            };
        }
    }
}