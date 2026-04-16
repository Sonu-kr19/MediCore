namespace MediCore.Api.DTOs.PrescriptionDtos
{
    // Represents ONE pending prescription shown to the pharmacist/admin
    public class QueuedPrescriptionDto
    {
        public int PrescriptionID { get; set; }

        public int DoctorID { get; set; }
        public string DoctorName { get; set; }

        public DateTime Date { get; set; }

        public List<PrescriptionMedicineDto> Medicines { get; set; }
    }
}