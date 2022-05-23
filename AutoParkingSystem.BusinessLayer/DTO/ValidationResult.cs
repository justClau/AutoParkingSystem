using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Core;

namespace AutoParkingSystem.BusinessLayer.DTO
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int UserID { get; set; }
        public bool Admin { get; set; }
        public SearchType SearchType { get; set; }
        public ParkingLot? ParkingLot { get; set; }
        
    }
}
