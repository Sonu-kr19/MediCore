
namespace MediCore.Api.DTOs.PrescriptionDtos
{
    //This represents ONE prescription shown to the pharmacist
    public class QueuedPrescriptionDto
    {
        public int PrescriptionID { get; set; }
        public int EMRID { get; set; }
        public int DoctorID { get; set; }
        public DateTime Date { get; set; }
    }
}
