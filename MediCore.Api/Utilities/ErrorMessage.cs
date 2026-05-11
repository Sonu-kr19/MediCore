using System;

namespace MediCore.Api.Utilities;

public class ErrorMessage
{
        // User Update
        public const string UpdateUserRequest = "Update request cannot be null.";
        public const string InvalidUserId = "Invalid UserID.";
        public const string Success = "User updated successfully.";
        public const string InvalidRoleName = "Invalid RoleName.";
        public const string UserNotFound = "User not found.";
        public const string NameRequired = "Name is required.";
        public const string InvalidRoleId = "Invalid RoleID.";
        public const string UpdateFailedUser = "User update failed.";
        public const string InvalidAction = "Invalid Action";
        public const string InvalidEmail = "Invalid Email";
        public const string EmailAlreadyExists = "Email already exists";

        //User Delete
        public const string DelSuccess="User deleted successfully.";
        public const string UserAlreadyDeleted=" User is already deleted";

        //Cancel Appointment
        public const string AppointmentNotFound="Appointment Not Found with that ID";
        public const string AppointmentAlreadyCancelled="Appointment is already cancelled";
        public const string CompletedAppointmet="Completed appointments cannot be cancelled";
        public const string OngoingAppointment="Ongoing appointments cannot be cancelled";

        //Creating Bills
        public const string PatientNotFound="No patient exists with this ID.";
        public const string BillItemsNotNull="BillItems cannot be null.";
        public const string BillItemRequired="At least one BillItem is required.";
        public const string ItemNameNotEmpty="ItemName cannot be empty.";
        public const string RateGreaterThanZero="Rate must be greater than zero.";

        //Submit insurance claims
        public const string InvalidInsuranceId="Invalid InsuranceID";
        public const string BillNotFound="Bill not found";
        public const string DuplicateClaim="Duplicate claim for Bill";
        public const string NotAuthenticated="User not authenticated";
        
}
