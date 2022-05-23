using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.BusinessLayer.DTO
{
    public class BillingResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Bill? Bill { get; set; }
        public IEnumerable<Bill>? Bills { get; set; }
    }
}
