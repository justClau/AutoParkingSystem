using APSDataAccessLibrary.DAL.Repositories;
using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IUsersRepository users;

        public ValidationService(IUsersRepository users)
        {
            this.users = users;
        }

        //Verify if user is admin
        public ValidationResult isAdmin(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                return new ValidationResult
                {
                    Success = false,
                    Message = "Username not specified!"
                };
            var user = users.GetByUsername(Username);
            if (user is null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "User not found!"
                };
            if (user.IsAdmin == false)
                return new ValidationResult
                {
                    Success = false,
                    Message = "You don't have the rights!"
                };
            return new ValidationResult
            {
                Success = true,
                Message = user.FullName
            };

        }

        //To Verify user input data
        public ValidationResult UserValidation(User User)
        {
            if (User == null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "No input data error!"
                };
            if (User.Username == null || User.FullName == null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "Username or Full Name fields are empty!"
                };
            if (User.Id != 0)
                return new ValidationResult
                {
                    Success = false,
                    Message = "Id field must be empty!"
                };
            if (User.Vehicle is not null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "User can park a vehicle after he is registered!"
                };
            var user = users.GetByUsername(User.Username);
            if (user is not null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "User already exists!",
                    UserID = user.Id,
                    Admin = user.IsAdmin
                };
            return new ValidationResult
            {
                Success = true
            };
        }

        //verify if user exists in database
        public ValidationResult UserExists(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                return new ValidationResult
                {
                    Success = false,
                    Message = "No user specidifed!"
                };
            var user = users.GetByUsername(Username);
            if (user is null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "User not found!"
                };
            return new ValidationResult
            {
                Success = true,
                UserID = user.Id
            };
        }

        //verify car search term
        public ValidationResult SearchTerm(string SearchTerm)
        {
            if (SearchTerm.Length != 17 && SearchTerm.Length != 8 && SearchTerm.Length != 7 && SearchTerm.Length != 6)
                return new ValidationResult
                {
                    Success = false,
                    Message = "The search criteria is not a valid License Plate Number or VIN"
                };
            return new ValidationResult
            {
                Success = true,
                SearchType = SearchTerm.Length == 17 ? SearchType.VIN : SearchType.PlateNumber
            };
        }

    }
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int UserID { get; set; }
        public bool Admin { get; set; }
        public SearchType SearchType { get; set; }
    }
}
