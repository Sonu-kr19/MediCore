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
}
