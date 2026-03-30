using System;

namespace MediCore.Api.Utilities;

public class ErrorMessages
{
    public const string PasswordsDoNotMatch ="Passwords do not match";
    public const string InvalidPassword ="Password must be at least 8 characters, include one uppercase letter and one number";
    public const string PasswordUpdatedSuccess = "Password updated successfully";
    public const string GenericError = "Something went wrong. Please try again.";
    public const string BadRequest = "Invalid Request";
    public const string UserNotFound = "User not found";
    public const string InvalidCredentials="Invalid username or password";
    public const string InvalidRefreshToken="Invalid refresh token";
    public const string InactiveUser = "User is deactivated.";

    //rahim helpers

    // Password
    public const string PasswordEmpty         = "Password cannot be empty.";
    public const string PasswordTooShort      = "Password must be at least 8 characters.";
    public const string PasswordNoUppercase   = "Password must contain at least one uppercase letter.";
    public const string PasswordNoLowercase   = "Password must contain at least one lowercase letter.";
    public const string PasswordNoDigit       = "Password must contain at least one number.";
    public const string PasswordNoSpecial     = "Password must contain at least one special character.";

    //  Email
    public const string EmailEmpty            = "Email cannot be empty.";
    public const string EmailInvalidFormat    = "Email format is invalid.";
    public const string EmailAlreadyExists    = "Email already registered.";

    //  Phone
    public const string PhoneEmpty            = "Phone number cannot be empty.";
    public const string PhoneInvalidFormat    = "Phone number must be exactly 10 digits.";

    //  Role
    public const string RolePatientOnly       = "Only Patient role allowed here.";
    public const string RolePatientEndpoint   = "Patients must register using patient endpoint.";

}
