namespace MediCore.Api.Utilities;

public class PatientDocumentErrorMessages
{
    public const string FileRequired        = "File is required.";
    public const string DocTypeRequired     = "Document type is required.";
    public const string InvalidDocType      = "Invalid document type. Valid types: Passport, IDCard, InsuranceCard, MedicalReport, Other.";
    public const string PatientNotFound     = "Patient does not exist.";
    public const string DocumentNotFound    = "Document does not exist.";
    public const string FileDataNotFound    = "File data not found.";
    public const string DocumentNotOwnedByPatient = "Document does not belong to this patient.";
    public const string DocumentAlreadyVerified   = "Document is already verified.";
    public const string NoDocumentsFound    = "No documents found for this patient.";
}