using System;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.DTOs.DispenseDtos;
using MediCore.Api.Repositories.DispenseRepo;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.DispenseServices
{
    public class DispenseService : IDispenseService
    {
        private readonly IDispenseRepository _dispenseRepository;
        private readonly MediCoreDbContext _context;

        // Low stock threshold (as per test cases)
        private const int LowStockThreshold = 5;

        public DispenseService(
            IDispenseRepository dispenseRepository,
            MediCoreDbContext context)
        {
            _dispenseRepository = dispenseRepository;
            _context = context;
        }

        /// <summary>
        /// Dispenses medicines for the given prescription.
        /// Validates stock, updates inventory, updates prescription status,
        /// and returns low-stock warnings if applicable.
        /// </summary>
        public async Task<DispensePrescriptionResponseDto>
            DispensePrescriptionAsync(DispensePrescriptionRequestDto request)
        {
            // Basic request validation (TC_PD_012)
            if (request == null || request.PrescriptionID <= 0)
                throw new ArgumentException("prescriptionId is required");

            // Start transaction (TC_PD_005, TC_PD_020)
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            // Load prescription with items
            var prescription =
                await _dispenseRepository
                    .GetPrescriptionWithItemsAsync(request.PrescriptionID);

            // Prescription existence check (TC_PD_017)
            if (prescription == null)
                throw new KeyNotFoundException("Prescription not found");

            // Already-dispensed check (TC_PD_010, TC_PD_011)
            if (prescription.Status == true)
                throw new ArgumentException("Prescription already dispensed");

            // Must have prescription items
            if (prescription.PrescriptionItems == null ||
                !prescription.PrescriptionItems.Any())
            {
                throw new InvalidOperationException(
                    "Prescription has no medicines to dispense");
            }

            var warnings = new List<string>();

            // STEP 1: STOCK VALIDATION (NO UPDATES HERE)
            foreach (var item in prescription.PrescriptionItems)
            {
                // Extract quantity from duration
                int requiredQty = ExtractQuantity(item.Duration);

                // Fetch medicine by name
                var medicine =
                    await _dispenseRepository
                        .GetMedicineByNameAsync(item.Medicine);

                // Medicine must exist (TC_PD_016)
                if (medicine == null)
                    throw new KeyNotFoundException(
                        $"Medicine '{item.Medicine}' not found");

                // Insufficient stock check (TC_PD_004)
                if (medicine.Stock < requiredQty)
                    throw new InvalidOperationException(
                        $"Insufficient stock for medicine '{medicine.Name}'");
            }

            // STEP 2: STOCK DECREMENT (SAFE TO UPDATE NOW)
            foreach (var item in prescription.PrescriptionItems)
            {
                int requiredQty = ExtractQuantity(item.Duration);

                var medicine =
                    await _dispenseRepository
                        .GetMedicineByNameAsync(item.Medicine);

                // Decrement stock accurately
                medicine.Stock -= requiredQty;

                // Low-stock warning (TC_PD_007 → TC_PD_022)
                if (medicine.Stock <= LowStockThreshold)
                {
                    warnings.Add(
                        $"Low stock warning: {medicine.Name} stock is now {medicine.Stock}");
                }

                await _dispenseRepository.UpdateMedicineAsync(medicine);
            }

            // STEP 3: UPDATE PRESCRIPTION STATUS
            prescription.Status = true; // Dispensed
            await _dispenseRepository
                .UpdatePrescriptionAsync(prescription);

            // COMMIT TRANSACTION
            await _dispenseRepository.SaveChangesAsync();
            await transaction.CommitAsync();

            // SUCCESS RESPONSE
            return new DispensePrescriptionResponseDto
            {
                Success = true,
                Message = "Medicines dispensed successfully",
                Warnings = warnings
            };
        }

        /// <summary>
        /// Extracts numeric quantity from duration string.
        /// Examples:
        ///  "5 days" → 5
        ///  "3 days" → 3
        /// </summary>
        private static int ExtractQuantity(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                throw new ArgumentException("Invalid quantity");

            var digits =
                new string(duration.Where(char.IsDigit).ToArray());

            if (!int.TryParse(digits, out int qty) || qty <= 0)
                throw new ArgumentException("Invalid quantity");

            return qty;
        }
    }
}
