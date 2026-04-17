namespace MediCore.Api.DTOs.PrescriptionDtos
{
    public class PrescriptionResponseDto
    {
        public int PrescriptionID { get; set; }

        public int DoctorID { get; set; }
        public string DoctorName { get; set; }

        public int TotalPrescriptionItems { get; set; }
    }
}