using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public int? VehicleId { get; set; }

        public Vehicle? Vehicle { get; set; }

        public string FullName { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<Bill> Bills { get; set; }
    }
}
