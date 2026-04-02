
using System;
using System.Text.RegularExpressions;
using MediCore.Api.Utilities;

namespace MediCore.Api.Utilities.Helpers;

public static class EmailHelper
{
    // Validates email is non-empty and has a valid format (local@domain.tld).
    // Throws ArgumentException with a specific message for each failure case.
    public static void Validate(string email)
    {
        // Reject null, empty, or whitespace before hitting the regex
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException(ErrorMessages.EmailEmpty);

        // Ensure basic email structure: something@something.something
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException(ErrorMessages.EmailInvalidFormat);
    }
}
