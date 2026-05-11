
namespace MediCore.Api.DTOs.PatientDtos;

//required dto for after uploading document and details to show to the user
public class PatientDocumentResponseDto
{
    public int DocumentId { get; set; }
    public string FileUri { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public class PatientDocumentDetailsDto
{
    public int PatientDocumentID { get; set; }
    public int PatientID { get; set; }
    public string DocType { get; set; } = null!;
    public string FileUri { get; set; } = null!;
    public DateTime UploadedDate { get; set; }
    public bool VerificationStatus { get; set; }
}