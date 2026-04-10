using MediCore.Domain.Entities;

namespace MediCore.Api.DTOs.EmrDtos;
public class EmrResponseDto
{
    public int EmrId { get; set; }
    public DateTime Date { get; set; }
    public string Diagnosis { get; set; }
    public string TreatmentPlan { get; set; }
    public List<Prescription> Prescriptions { get; set; }
}
