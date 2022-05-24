using APSDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.Context
{
    public class ParkingContext : DbContext
    {
        private readonly string connectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSchema> ParkingSchema { get; set; }
        public ParkingContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            this.connectionString = config.GetConnectionString("default");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
