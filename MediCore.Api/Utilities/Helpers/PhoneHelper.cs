
using System.Text.RegularExpressions;
using MediCore.Api.Utilities;

namespace MediCore.Api.Utilities.Helpers;

public static class PhoneHelper
{
    // Validates phone number is exactly 10 digits (no spaces, dashes, or country code).
    // Throws ArgumentException if the format doesn't match.
    public static void Validate(string phone)
    {
        // Null/whitespace check disabled — regex below already rejects empty strings
        // since an empty string won't match ^\d{10}$. Re-enable if a separate
        // "phone is required" error message is needed.
        // if (string.IsNullOrWhiteSpace(phone))
        //     throw new ArgumentException(ErrorMessages.PhoneEmpty);

        // Enforce exactly 10 numeric digits — no formatting characters allowed
        if (!Regex.IsMatch(phone, @"^\d{10}$"))
            throw new ArgumentException(ErrorMessages.PhoneInvalidFormat);
    }
}