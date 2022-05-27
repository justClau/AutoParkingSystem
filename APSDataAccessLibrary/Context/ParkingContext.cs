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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne<Vehicle>(u => u.Vehicle)
                .WithOne(v => v.Owner)
                .HasForeignKey<User>(u => u.VehicleId);
            modelBuilder.Entity<User>(user =>
            {
                user.Property(u => u.Username).HasMaxLength(50);
                user.Property(u => u.FullName).HasMaxLength(50);
            });

            modelBuilder.Entity<ParkingLot>()
                .HasOne<Vehicle>(p => p.Vehicle)
                .WithOne(v => v.ParkingLot)
                .HasForeignKey<ParkingLot>(p => p.VehicleId);
            modelBuilder.Entity<ParkingLot>(parkingLot =>
            {
                parkingLot.Property(p => p.Name).HasMaxLength(10);
            });

            modelBuilder.Entity<Vehicle>(vehicle =>
            {
                vehicle.Property(v => v.VIN).HasMaxLength(17);
                vehicle.Property(v => v.PlateNumber).HasMaxLength(8);
            });

            modelBuilder.Entity<Bill>()
                .HasOne<User>(b => b.User)
                .WithMany(u => u.Bills)
                .HasForeignKey(b=> b.UserId);
            modelBuilder.Entity<Bill>(bill => 
            {
                bill.Property(b => b.ParkingLot).HasMaxLength(10);
                bill.Property(b => b.VehiclePlate).HasMaxLength(8);
                bill.Property(b => b.VehicleVIN).HasMaxLength(17);
            });
        }
    }
}
