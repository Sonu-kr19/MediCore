
using System;
using MediCore.Api.Utilities;

namespace MediCore.Api.Utilities.Helpers;

public static class PasswordHelper
{
    // Validates password meets minimum security requirements before hashing and storing.
    // Each rule throws a specific ArgumentException so the caller gets a clear error message.
    public static void Validate(string password)
    {
        // Reject null or empty before checking any rules
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException(ErrorMessages.PasswordEmpty);

        // Minimum length to reduce brute-force risk
        if (password.Length < 8)
            throw new ArgumentException(ErrorMessages.PasswordTooShort);

        // Require mixed case to increase character space
        if (!password.Any(char.IsUpper))
            throw new ArgumentException(ErrorMessages.PasswordNoUppercase);

        if (!password.Any(char.IsLower))
            throw new ArgumentException(ErrorMessages.PasswordNoLowercase);

        // Require at least one digit and one special character for stronger entropy
        if (!password.Any(char.IsDigit))
            throw new ArgumentException(ErrorMessages.PasswordNoDigit);

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new ArgumentException(ErrorMessages.PasswordNoSpecial);
    }
}
