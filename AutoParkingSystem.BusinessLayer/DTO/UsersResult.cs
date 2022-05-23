using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.BusinessLayer.DTO
{
    public class UsersResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public User User { get; set; }
    }
}
