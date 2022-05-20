using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public interface IValidationService
    {
        ValidationResult isAdmin(string Username);
        ValidationResult UserValidation(User User);
        ValidationResult UserExists(string Username);
    }
}