using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.Models
{
    public class ParkingLot
    {
        public int Id { get; set; }

        public int FloorNumber { get; set; }

        public string Name { get; set; }

        public int? VehicleId { get; set; }

        public Vehicle? Vehicle { get; set; }
    }
}
