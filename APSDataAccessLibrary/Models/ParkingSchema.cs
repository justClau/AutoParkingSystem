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

        public int Floors { get; set; }

        public int SizeX { get; set; }

        public int SizeY { get; set; } 
    }
}
