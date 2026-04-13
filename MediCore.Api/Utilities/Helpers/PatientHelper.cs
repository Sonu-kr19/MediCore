using System;
using MediCore.Domain.Enum;

namespace MediCore.Api.Utilities.Helpers;

public class PatientHelper
{
     public static void Validate(string name, string address, DateOnly dob, GenderOption gender, int? insuranceId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(PatientErrorMessages.NameRequired);

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException(PatientErrorMessages.AddressRequired);

        if (!Enum.IsDefined(typeof(GenderOption), gender))
            throw new ArgumentException(PatientErrorMessages.GenderRequired);

        if ( insuranceId < 0 )
            throw new ArgumentException(PatientErrorMessages.InsuranceIDRequired);

        
    }


}
