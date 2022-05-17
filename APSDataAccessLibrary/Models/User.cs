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
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        public Vehicle? Vehicle { get; set; }
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required(ErrorMessage ="Must specify user rights")]
        public bool IsAdmin { get; set; }

    }
}
