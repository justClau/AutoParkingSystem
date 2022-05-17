using APSDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.Context
{
    public class ParkingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSchema> ParkingSchema { get; set; }
        public ParkingContext(DbContextOptions options) : base(options) { }
    }
}
