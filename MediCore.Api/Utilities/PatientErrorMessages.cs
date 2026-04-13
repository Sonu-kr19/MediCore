using System;

namespace MediCore.Api.Utilities;

public class PatientErrorMessages
{
    //error messages for the patient module
     // UserID
    public const string UserNotFound = "UserID does not exist.";

    // InsuranceID
    public const string InsuranceNotFound = "InsuranceID does not exist.";
    public const string InsuranceAlreadyAssigned = "InsuranceID is already assigned to another patient.";

    // Patient
    public const string PatientNotFound = "Patient does not exist.";
    public const string PatientAlreadyDeleted = "Patient is already deleted.";
    public const string PatientsNotFound  = "No patients found.";
    public const string PatientAlreadyRegistered = "A patient record already exists for this UserID.";
    // DOB
    public const string DOBFutureDate = "Date of birth cannot be a future date.";

    public const string NameRequired = "Name is required.";
    public const string AddressRequired = "Address is required.";
    public const string GenderRequired = "Gender is required.";
    public const string InsuranceIDRequired = "InsuranceID is required.";

}
