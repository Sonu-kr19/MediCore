using MediCore.Api.DTOs.ComplianceDtos;
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
    }
}
