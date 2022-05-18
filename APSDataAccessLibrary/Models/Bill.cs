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
        public User User { get; set; }
        [Required]
        [MaxLength(10)]
        public string ParkingLot { get; set; }
        [Required]
        [MaxLength(8)]
        public string VehiclePlate { get; set; }
        [Required]
        [MaxLength(17)]
        public string VehicleVIN { get; set; }
        [Required]
        public DateTime IssuedAt { get; set; }
        [Required]
        public DateTime ParkTime { get; set; }
        [Required]
        public double BillValue { get; set; }
        [Required]
        public bool IsPaid { get; set; }

    }
}
