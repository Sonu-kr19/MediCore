using MediCore.Api.DTOs.ComplianceDtos;
using MediCore.Api.DTOs.PatientDtos;
using MediCore.Api.Repositories.ComplianceRepo;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.ComplianceServices
{
    public class ComplianceService : IComplianceService
    {
        private readonly IComplianceRepository _complianceRepository;

        public ComplianceService(IComplianceRepository complianceRepository)
        {
            _complianceRepository = complianceRepository;
        }

        /// Creates a new compliance record.
        public async Task<CreateComplianceRecordResponseDto>
            CreateComplianceRecordAsync(CreateComplianceRecordRequestDto request)
        {
            // Defensive validation (extra safety)
            if (request == null)
                throw new ArgumentException("Invalid request");

            //  Map DTO to entity
            var complianceRecord = new ComplianceRecord
            {
                PatientID = request.PatientID,
                Type = request.Type,
                Result = request.Result,
                Note = request.Note,
                Date = DateTime.UtcNow
            };

            // Persist data
            await _complianceRepository.AddComplianceRecordAsync(complianceRecord);
            await _complianceRepository.SaveChangesAsync();

            //  Return ComplianceRecordID
            return new CreateComplianceRecordResponseDto
            {
                ComplianceId = complianceRecord.ComplianceRecordID,
                Message = "Compliance record created successfully"
            };
        }

         public async Task LogComplianceEventAsync(int patientId, string type)
        {
            await _complianceRepository.CreateAsync(patientId, type);
        }

        public async Task<List<ComplianceResponseDto>> GetAllPendingAsync()
        {
            var records = await _complianceRepository.GetAllPendingAsync();

            if (records.Count == 0)
                throw new Exception("No pending compliance records found.");

            return records.Select(c => new ComplianceResponseDto
            {
                ComplianceRecordID = c.ComplianceRecordID,
                PatientID = c.PatientID,
                Type = c.Type,
                Result = c.Result,
                Date = c.Date,
                Note = c.Note
            }).ToList();
        }

        public async Task<ComplianceResponseDto> VerifyAsync(int complianceRecordId, ComplianceVerifyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Result))
                throw new ArgumentException("Result is required.");

            if (dto.Result is not "Approved" and not "Rejected")
                throw new ArgumentException("Result must be either 'Approved' or 'Rejected'.");

            var record = await _complianceRepository.GetByIdAsync(complianceRecordId);
            if (record == null)
                throw new KeyNotFoundException("Compliance record does not exist.");

            if (record.Result != "Pending")
                throw new InvalidOperationException("Compliance record is already verified.");

            record.Result = dto.Result;
            record.Note = dto.Note ?? string.Empty;
            record.Date = DateTime.UtcNow;

            await _complianceRepository.UpdateAsync(record);

            return new ComplianceResponseDto
            {
                ComplianceRecordID = record.ComplianceRecordID,
                PatientID = record.PatientID,
                Type = record.Type,
                Result = record.Result,
                Date = record.Date,
                Note = record.Note
            };
        }
 
    }
}
