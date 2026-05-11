namespace MediCore.Api.DTOs.PatientDtos;

public class PatientDeleteResponseDto
{
    //for soft deleting patient we required patient id
    public int PatientID { get; set; }
    public string Message { get; set; } = null!;
}