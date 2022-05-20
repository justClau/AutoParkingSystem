using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public interface IValidationService
    {
        ValidationResult isAdmin(string Username);
        ValidationResult UserValidation(User User);
        ValidationResult UserExists(string Username);
        ValidationResult SearchTerm(string SearchTerm);
        ValidationResult CarDetails(Vehicle Vehicle);
        ValidationResult ParkingLotName(int FloorNumber, string Name);
    }
}