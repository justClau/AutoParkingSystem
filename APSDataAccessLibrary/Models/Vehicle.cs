using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public User Owner { get; set; }

        public string VIN { get; set; }

        public string PlateNumber { get; set; }

        public DateTime StartingTime { get; set; }

        public ParkingLot ParkingLot { get; set; }
    }
}
