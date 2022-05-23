using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DAL
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
            database.SaveChanges();
            database.Users.Add(new User { Username = "user", FullName = "Reqular User", IsAdmin = false });
            database.SaveChanges();
        }
        private void AddSchema(int floors, int sizeX, int sizeY)
        {
            database.ParkingSchema.Add(new ParkingSchema { Floors = floors, SizeX = sizeX, SizeY = sizeY });
            database.SaveChanges();
        }
        private void ParkingDatabaseInit(int f, int n, int m)
        {
            char val = 'A';
            for(int i = 0; i < f; i++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < m; k++)
                    {
                        var row = new ParkingLot
                        {
                            FloorNumber = i,
                            Name = $"{(char)(val + j)}{k + 1}"
                        };
                        database.ParkingLots.Add(row);
                        database.SaveChanges();
                    }
            if(database.ParkingSchema.Any() == false)
                database.ParkingSchema.Add(new ParkingSchema { Floors = f, SizeX = n, SizeY = m });
            else
            {
                var x = database.ParkingSchema.FirstOrDefault();
                x.Floors = f;
                x.SizeX = n;
                x.SizeY = m;
                var entity = database.ParkingSchema.Attach(x);
                entity.State = EntityState.Modified;
            }
            database.SaveChanges();
            if (database.Users.Any() == false) 
                AddUsers();
            
        }
        internal ConfigDTO StartParkingLotConfiguration()
        {
            var floor = Convert.ToInt32(config.GetSection("Floors").Value);
            int n = Convert.ToInt32(config.GetSection("FloorSize").GetSection("VehicleX").Value),
                m = Convert.ToInt32(config.GetSection("FloorSize").GetSection("VehicleY").Value);
            if (floor == 0) return new ConfigDTO { StatusCode = 500, Success = false, Message = "The parking place cannot exist with NO floors" };

            if (m == 0 || n == 0)
                return new ConfigDTO { StatusCode = 500, Success = false, Message = "The parking place cannot exist if the rows or the columns are 0" };
            if (m > 26) 
                return new ConfigDTO { StatusCode = 500, Success = false, Message = "FloorY is greater than 26. Required for naming conventions" };
                //throw new Exception("");
            var init = database.ParkingSchema.FirstOrDefault();
            if (init is not null && init.Floors == floor && init.SizeX == n && init.SizeY == m)
                return new ConfigDTO { StatusCode = 500, Success = false, Message = "No new changes to make" };

            if(init is null) 
                AddSchema(floor, n, m);

            if (database.ParkingLots.Any())
                database.ParkingLots.FromSqlRaw("TRUNCATE TABLE dbo.ParkingLots").FirstOrDefault();

            ParkingDatabaseInit(floor, n, m);
            database.SaveChanges();
            var query = from p in database.ParkingLots orderby p.Id select p;
            return new ConfigDTO {
                StatusCode = 203, 
                Success = true,
                Message = "ParkingLots Initialised", 
                parkingLots = query 
            };
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
