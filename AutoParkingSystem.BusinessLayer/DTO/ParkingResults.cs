using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.BusinessLayer.DTO
{
    public class ParkingResults
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public IEnumerable<ParkingLot> ParkingLots { get; set; }
        public Vehicle? VehicleInfo { get; set; }
        public Bill? Bill { get; set; }
    }
}
