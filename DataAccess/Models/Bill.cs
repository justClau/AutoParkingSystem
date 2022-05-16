using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ParkingLotId { get; set; }
        public string VehiclePlate { get; set; }
        public string VehicleVIN { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ParkTime { get; set; }
        public int BillValue { get; set; }
        public bool IsPaid { get; set; }

    }
}
