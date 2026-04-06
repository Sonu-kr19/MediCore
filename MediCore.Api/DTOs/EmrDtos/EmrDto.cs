using MediCore.Domain.Entities;

public class EmrDto
{
    public int EmrId { get; set; }
    public DateTime Date { get; set; }
    public string Diagnosis { get; set; }
    public string TreatmentPlan { get; set; }
    public List<Prescription> Prescriptions { get; set; }
}