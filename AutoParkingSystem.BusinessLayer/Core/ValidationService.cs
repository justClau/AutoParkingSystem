using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.DAL.Repositories;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Domain;

namespace AutoParkingSystem.BusinessLayer.Core
{
    public class ValidationService : IValidationService
    {
        private readonly IUsersRepository users;
        private readonly IParkingLotsRepository parking;

        public ValidationService(IUsersRepository users, IParkingLotsRepository parking)
        {
            this.users = users;
            this.parking = parking;
        }

        //Verify if user is admin
        public ValidationResult IsAdmin(string Username)
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
                    Message = "No user specified!"
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

        //Verify Car Input Details
        public ValidationResult VerifyCarDetails(Vehicle Vehicle)
        {
            if (string.IsNullOrEmpty(Vehicle.VIN))
                return new ValidationResult
                {
                    Success = false,
                    Message = "Must Enter VIN!"
                };
            if (string.IsNullOrEmpty(Vehicle.PlateNumber))
                return new ValidationResult
                {
                    Success = false,
                    Message = "Must Enter Car's license plate number!"
                };
            return new ValidationResult
            {
                Success = true
            };
        }
        public ValidationResult ParkingLotName(int FloorNumber, string Name)
        {
            var variabila1 = parking.GetParkingConfiguration();
            if (variabila1.Floors <= FloorNumber)
                return new ValidationResult
                {
                    Success = false,
                    Message = "FloorNumber is out of range!"
                };
            var variabila = parking.GetByName(FloorNumber, Name);
            if (variabila == null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "Parking lot not found! Spellcheck the name!"
                };
            if (variabila.Vehicle is not null)
                return new ValidationResult
                {
                    Success = false,
                    Message = "The chosen parking lot is not empty!"
                };
            return new ValidationResult
            {
                Success = true,
                Message = "The Chosen Parking Lot is Free!"
            };
        }

    }
}
