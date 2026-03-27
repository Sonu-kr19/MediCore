
namespace MediCore.Api.Utilities;

public static class ErrorMessages
{
    // Password
    public const string PasswordEmpty = "Password cannot be empty.";
    public const string PasswordTooShort = "Password must be at least 8 characters.";
    public const string PasswordNoUppercase = "Password must contain at least one uppercase letter.";
    public const string PasswordNoLowercase = "Password must contain at least one lowercase letter.";
    public const string PasswordNoDigit = "Password must contain at least one number.";
    public const string PasswordNoSpecial  = "Password must contain at least one special character.";

    // Email
    public const string EmailEmpty = "Email cannot be empty.";
    public const string EmailInvalidFormat = "Email format is invalid.";
    public const string EmailAlreadyExists = "Email already registered.";

    // Phone
    public const string PhoneEmpty = "Phone number cannot be empty.";
    public const string PhoneInvalidFormat = "Phone number must be exactly 10 digits.";

    // Role
    public const string RolePatientOnly = "Only Patient role allowed here.";
    public const string RolePatientEndpoint = "Patients must register using patient endpoint.";
}