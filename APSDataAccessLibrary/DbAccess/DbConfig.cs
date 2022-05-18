using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DbAccess
{
    public class DbConfig
    {
        private readonly IConfiguration config;
        private readonly ParkingContext database;

        public DbConfig(IConfiguration config, ParkingContext database)
        {
            this.config = config.GetSection("ParkingSystem");
            this.database = database;
        }
        private void AddUsers()
        { 
            database.Users.Add(new User { Username = "root", FullName = "Administrator", IsAdmin = true });
            database.Users.Add(new User { Username = "user", FullName = "Reqular User", IsAdmin = false });
        }
        private void ParkingDatabaseInit(int f, int m, int n)
        {
            char val = 'A';
            for(int i = 0; i < f; i++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < m; k++)
                    {
                        var row = new ParkingLot
                        {
                            Floor = i,
                            Name = $"{(char)(val + j)}{k + 1}"
                        };
                        database.ParkingLots.Add(row);
                    }
            database.ParkingSchema.Remove(new ParkingSchema { Id = 1});
            database.ParkingSchema.Add(new ParkingSchema { Floors = f, SizeX = n, SizeY = m });
            if(!database.Users.Any()) AddUsers();
            database.SaveChanges();
        }
        internal ConfigDTO StartParkingLotConfiguration()
        {
            var floor = Convert.ToInt32(config.GetSection("Floors").Value);
            int m = Convert.ToInt32(config.GetSection("FloorSize").GetSection("VehicleX").Value),
                n = Convert.ToInt32(config.GetSection("FloorSize").GetSection("VehicleY").Value);
            if (floor == 0) return new ConfigDTO { StatusCode = 500, Success = false, Message = "The parking place cannot exist with NO floors" };

            if (m == 0 || n == 0)
                return new ConfigDTO { StatusCode = 500, Success = false, Message = "The parking place cannot exist if the rows or the columns are 0" };
            if (m > 26) 
                return new ConfigDTO { StatusCode = 500, Success = false, Message = "FloorY is greater than 26. Required for naming conventions" };
                //throw new Exception("");
            var init = database.ParkingSchema.Find(1);
            if (init is not null && init.Floors == floor && init.SizeX == n && init.SizeY == m)
                return new ConfigDTO { StatusCode = 500, Success = false, Message = "No new changes to make" };
            if (database.ParkingLots.Any())
                database.ParkingLots.FromSqlRaw("TRUNCATE TABLE dbo.ParkingLots").FirstOrDefault();
            ParkingDatabaseInit(floor, m, n);
            var query = from p in database.ParkingLots orderby p.Id select p;
            return new ConfigDTO { StatusCode = 203, Success = true, Message = "ParkingLots Initialised", parkingLots = query };
        }
    }
    public class ConfigDTO
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<ParkingLot>? parkingLots { get; set; }
    }
}
