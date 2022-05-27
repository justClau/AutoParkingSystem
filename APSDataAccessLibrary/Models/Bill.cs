using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.Models
{
    public class Bill
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public string ParkingLot { get; set; }
        
        public string VehiclePlate { get; set; }
        
        public string VehicleVIN { get; set; }
        
        public DateTime IssuedAt { get; set; }
        
        public DateTime StartingTime { get; set; }
        
        public double BillValue { get; set; }
        
        public bool IsPaid { get; set; }

    }
}
