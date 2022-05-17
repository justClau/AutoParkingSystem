﻿using System;
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
        public ParkingLot ParkingLot { get; set; }
        [Required(ErrorMessage = "Please enter VIN")]
        [MaxLength(17)]
        public string VIN { get; set; }
        [Required(ErrorMessage = "Please enter VIN")]
        [MaxLength(8)]
        public string PlateNumber { get; set; }
        [Required(ErrorMessage = "DATE REQUIRED FOR BILLING INFORMATION. PLEASE CONTACT DEV_TEAM")]
        public DateTime parkTime { get; set; }

    }
}
