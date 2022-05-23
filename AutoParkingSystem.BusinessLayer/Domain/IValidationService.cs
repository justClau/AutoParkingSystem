using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Core;

namespace AutoParkingSystem.BusinessLayer.Domain
{
    public interface IValidationService
    {
        ValidationResult IsAdmin(string Username);
        ValidationResult UserValidation(User User);
        ValidationResult UserExists(string Username);
        ValidationResult SearchTerm(string SearchTerm);
        ValidationResult VerifyCarDetails(Vehicle Vehicle);
        ValidationResult ParkingLotName(int FloorNumber, string Name);
    }
}