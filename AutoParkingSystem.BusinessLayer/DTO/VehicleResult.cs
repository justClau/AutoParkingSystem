using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.BusinessLayer.DTO
{
    public class VehicleResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Vehicle? Vehicle { get; set; }
        public IEnumerable<Vehicle>? Vehicles { get; set; }
    }
}
