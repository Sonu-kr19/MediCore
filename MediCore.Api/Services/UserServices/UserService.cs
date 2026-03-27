

using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories.UserRepo;
using MediCore.Api.Utilities;
using MediCore.Api.Utilities.Helpers;
using MediCore.Domain.Entities;
using MediCore.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace MediCore.Api.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        // Resolves IUserRepository via DI and instantiates PasswordHasher<User> directly
        // because PasswordHasher has no dependencies of its own and is not expected
        // to be swapped out or mocked — keeping it out of the DI container avoids
        // unnecessary registration overhead.
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        // Entry point for patient self-registration (called from the patient endpoint).
        // Guards against the wrong role being submitted — if a non-Patient role slips
        // through the patient endpoint, it is rejected here before any DB work happens,
        // ensuring role assignment is always intentional and endpoint-driven.
        public async Task RegisterPatientAsync(UserRegisterDto dto)
        {
            if (dto.RoleName != RoleOption.Patient)
                throw new ArgumentException(ErrorMessages.RolePatientOnly);

            await RegisterUserAsync(dto);
        }

        // Entry point for staff registration (called from the staff endpoint).
        // Explicitly blocks the Patient role here so staff accounts can never be
        // accidentally downgraded to a patient role via the staff endpoint,
        // while still allowing any valid staff role (Doctor, Nurse, Admin, etc.)
        // to be registered without hardcoding each one.
        public async Task RegisterStaffAsync(UserRegisterDto dto)
        {
            if (dto.RoleName == RoleOption.Patient)
                throw new ArgumentException(ErrorMessages.RolePatientEndpoint);

            await RegisterUserAsync(dto);
        }

        // Core registration logic shared by both patient and staff flows.
        // Kept private so it can only be reached through the role-guarded public methods above,
        // preventing any caller from bypassing role validation.
        //
        // Order of operations matters here:
        // 1. Validate format of password, email, and phone before hitting the DB,
        //    so we fail fast on bad input without an unnecessary round-trip.
        // 2. Check for duplicate email to avoid a unique-constraint DB exception,
        //    returning a meaningful error instead of a raw database error.
        // 3. Build the User entity and hash the password after all validations pass,
        //    so we never store a user with an invalid or duplicate email.
        private async Task RegisterUserAsync(UserRegisterDto dto)
        {
            PasswordHelper.Validate(dto.Password);
            EmailHelper.Validate(dto.Email);
            PhoneHelper.Validate(dto.Phone);

            // Checks the DB for an existing account with the same email.
            // Throwing InvalidOperationException (vs ArgumentException) signals that
            // the input itself is valid but the operation cannot proceed due to a
            // business rule conflict — mapped to 409 Conflict in the controller.
            var exists = await _userRepository.GetByEmailAsync(dto.Email);
            if (exists != null)
                throw new InvalidOperationException(ErrorMessages.EmailAlreadyExists);

            var user = new User
            {
                Name     = dto.Name,
                Email    = dto.Email,
                RoleName = dto.RoleName,
                Phone    = dto.Phone,
                Status   = true
            };

            // Password is hashed after the User object is constructed because
            // HashPassword requires the user instance as a salt context (ASP.NET Identity
            // uses it internally for user-specific hashing strategies).
            // The plain-text password from the DTO is never persisted.
            user.Password = _passwordHasher.HashPassword(user, dto.Password);
            await _userRepository.RegisterUserAsync(user);
        }
    }
}