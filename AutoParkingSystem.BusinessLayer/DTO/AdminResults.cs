using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.BusinessLayer.DTO
{
    public class AdminResults
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<User>? AllUsers { get; set; }
    }
}
