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
        [Required(ErrorMessage = "Please enter VIN")]
        [MaxLength(17)]
        public string VIN { get; set; }
        [Required(ErrorMessage = "Please enter VIN")]
        [MaxLength(8)]
        public string PlateNumber { get; set; }
        [Required]
        public DateTime StartingTime { get; set; }

    }
}
