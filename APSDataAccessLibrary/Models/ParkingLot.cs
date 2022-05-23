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
        [Required]
        public int FloorNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
