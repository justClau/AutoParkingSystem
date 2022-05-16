using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public ParkingLot ParkingLot { get; set; }
        public string VIN { get; set; }
        public string PlateNumber { get; set; }
        public DateTime parkTime { get; set; }

    }
}
