using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.Models
{
    public class ParkingSchema
    {
        public int Id { get; set; }
        [Required]
        public int Floors { get; set; }
        [Required]
        public int SizeX { get; set; }
        [Required]
        public int SizeY { get; set; } 
    }
}
