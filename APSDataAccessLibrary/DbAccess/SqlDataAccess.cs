using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DbAccess
{
    public class SqlDataAccess
    {
        private readonly ParkingContext database;
        private readonly ILogger logger;

        public SqlDataAccess(ParkingContext database, ILogger logger)
        {
            this.database = database;
            this.logger = logger;
        }
        
        public IEnumerable<Bill> GetBills()
        {
            var query = from bill in database.Bills
                        orderby bill.Id select bill;
            return query;
        }
        public IEnumerable<Bill> GetUserBills(int id)
        {
            var query = from bill in database.Bills
                        where bill.User.Id == id
                        orderby bill.BillValue select bill;
            return query;
        }
        public Bill GetBillById(int id) => database.Bills.Find(id);
        public Bill SaveBill(Bill bill)
        {
            database.Add(bill);
            return bill;
        }
        public IEnumerable<User> GetUsers()
        {
            var query = from user in database.Users
                        where user.IsAdmin == false
                        orderby user.Username select user;
            return query;
        }
        public User GetUserById(int id) => database.Users.Find(id);
        public User GetUserByUsername(string name) => (User)(from user in database.Users where user.Username == name select user);
        public User AddUser(User user)
        {
            database.Add(user);
            return user;
        }
        public User DeleteUser(int id)
        {
            var user = GetUserById(id);
            if(user is null) return null;
            if(user.Vehicle is not null)
            {
                logger.LogInformation("INFORMATION: UNABLE TO DELETE USER IF VEHICLE IS IN PARKING");
                return null;
            }
            database.Users.Remove(user);
            return user;
        }
        public User SetAdmin(int UserID, bool AdminStatus)
        {
            var user = GetUserById(UserID);
            if (user is null) return null;
            if (user.IsAdmin == AdminStatus) return user;
            user.IsAdmin = AdminStatus;
            var dbUser = database.Users.Attach(user);
            dbUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return user;
        }
        public User AddUserVehicle(int UserID, int VehicleID)
        {
            var user = GetUserById(UserID);
            if (user is null) return null;
            user.Vehicle.Id = VehicleID;
            var dbUser = database.Users.Attach(user);
            dbUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return user;
        }
        public Vehicle GetUserParkedVehicle(int id)
        {
            var usr = GetUserById(id);
            if (usr.Vehicle is null) return null;
            var query = from veh in database.Vehicles
                        where veh.Id == usr.Vehicle.Id
                        select veh;
            return query.FirstOrDefault();
        }
        public IEnumerable<Vehicle> GetParkedVehicles()
        {
            var query = from veh in database.Vehicles
                        orderby veh.parkTime select veh;
            return query;
        }
        public Vehicle GetVehicleById(int id) => database.Vehicles.Find(id);
        public Vehicle GetVehicleByVIN(string VIN) => database.Vehicles.Where(veh => veh.VIN == VIN).FirstOrDefault();
        public Vehicle GetVehicleByPlate(string Plate) => database.Vehicles.Where(veh => veh.PlateNumber == Plate).FirstOrDefault();
        public ParkingLot GetVehicleParkingLot(int id) => database.ParkingLots.Where(p => p.Vehicle.Id == id).FirstOrDefault();
        public ParkingLot GetParkingLotById(int id) => database.ParkingLots.Find(id);
        public ParkingLot GetParkingLotByName(string name) => database.ParkingLots.Where(veh => veh.Name.StartsWith(name)).FirstOrDefault();
        public IEnumerable<ParkingLot> GetParkingLots()
        {
            var query = from p in database.ParkingLots
                        orderby p.Name select p;
            return query;
        }
        public IEnumerable<ParkingLot> GetFreeParkingLots()
        {
            var query = from p in database.ParkingLots
                        where p.Vehicle == null
                        select p;
            return query;
        }
        public IEnumerable<ParkingLot> AddVehicle(Vehicle newVehicle, int userId, int parkingLotId)
        {
            var veh = database.Vehicles.Where(veh => veh.VIN == newVehicle.VIN || veh.PlateNumber == newVehicle.PlateNumber).FirstOrDefault();
            if (veh is not null) return null;
            database.Vehicles.Add(newVehicle);
            database.SaveChanges();
            var usr = database.Users.Find(userId);
            var parking = database.ParkingLots.Find(parkingLotId);
            if(usr is null || parking is null) return null;
            return null;

        }
    }
}
