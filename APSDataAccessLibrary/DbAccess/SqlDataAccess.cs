using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8603
namespace APSDataAccessLibrary.DbAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly ParkingContext database;
        private readonly IConfiguration config;

        public SqlDataAccess(ParkingContext database, IConfiguration config)
        {
            this.database = database;
            this.config = config;
        }

        public IEnumerable<Bill> GetBills()
        {
            var query = from bill in database.Bills
                        orderby bill.Id
                        select bill;
            return query;
        }
        public IEnumerable<Bill> GetUserBills(int id)
        {
            var query = from bill in database.Bills
                        where bill.User.Id == id
                        orderby bill.BillValue
                        select bill;
            return query;
        }
        public Bill GetBillById(int id) => database.Bills.Find(id);
        public Bill SaveBill(Bill bill)
        {
            database.Bills.Add(bill);
            return bill;
        }
        public IEnumerable<User> GetUsers()
        {
            var query = from user in database.Users
                        where user.IsAdmin == false
                        orderby user.Username
                        select user;
            return query;
        }
        public IEnumerable<User> GetAllUsers()
        {
            var query = from user in database.Users
                        select user;
            return query;
        }
        public User GetUserById(int id) => database.Users.Find(id);
        public User GetUserByUsername(string name) => database.Users.Where(u => u.Username == name).FirstOrDefault();
        public User AddUser(User user)
        {
            database.Add(user);
            return user;
        }
        public User DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user is null) return null;
            if (user.Vehicle is not null)
                return null;
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
        public User SetUserVehicle(int UserID, Vehicle newVehicle)
        {
            var user = GetUserById(UserID);
            if (user is null) return null;
            user.Vehicle = newVehicle;
            var dbUser = database.Users.Attach(user);
            dbUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return user;
        }
        public User RemoveUserVehicle(int UserID)
        {
            var user = GetUserById(UserID);
            if (user is null) return null;
            user.Vehicle = null;
            var dbUser = database.Users.Attach(user);
            dbUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return user;
        }
        public Vehicle GetUserParkedVehicle(int id)
        {
            var usr = GetUserById(id);
            if (usr is null || usr.Vehicle is null) return null;
            var query = database.Vehicles.Where(veh => veh.Id == usr.Vehicle.Id).FirstOrDefault();
            return query;
        }
        public IEnumerable<Vehicle> GetParkedVehicles()
        {
            var query = from veh in database.Vehicles
                        orderby veh.parkTime
                        select veh;
            return query;
        }
        public Vehicle GetVehicleById(int id) => database.Vehicles.Find(id);
        public Vehicle GetVehicleByVIN(string VIN) => database.Vehicles.Where(veh => veh.VIN == VIN).FirstOrDefault();
        public Vehicle GetVehicleByPlate(string Plate) => database.Vehicles.Where(veh => veh.PlateNumber == Plate).FirstOrDefault();
        public ParkingLot GetParkingLotByVehicle(int id) => database.ParkingLots.Where(p => p.Vehicle.Id == id).FirstOrDefault();
        public ParkingLot GetParkingLotById(int id) => database.ParkingLots.Find(id);
        public ParkingLot GetParkingLotByName(string name) => database.ParkingLots.Where(veh => veh.Name.StartsWith(name)).FirstOrDefault();
        public IEnumerable<ParkingLot> GetParkingLots()
        {
            var query = from p in database.ParkingLots
                        orderby p.Name
                        select p;
            return query;
        }
        public IEnumerable<ParkingLot> GetFreeParkingLots()
        {
            var query = from p in database.ParkingLots
                        where p.Vehicle == null
                        select p;
            return query;
        }
        public IEnumerable<ParkingLot> GetFloorFreeParkingLots(int FloorNumber)
        {
            var schema = database.ParkingSchema.FirstOrDefault();
            if (FloorNumber > schema.Floors) return null;
            var query = from p in database.ParkingLots
                        where p.Floor == FloorNumber
                        orderby p.Id
                        select p;
            return query;
        }
        public ParkingLot AddParkingLotVehicle(Vehicle newVehicle)
        {
            var parkingLot = database.ParkingLots.Where(p => p.Vehicle == null).FirstOrDefault();
            if (parkingLot is null)
                return null;
            parkingLot.Vehicle = newVehicle;
            var dbParkingLot = database.ParkingLots.Attach(parkingLot);
            dbParkingLot.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return parkingLot;
        }
        public ParkingLot SetParkingLotVehicle(int ParkingLotID, Vehicle newVehicle)
        {
            var parkingLot = GetParkingLotById(ParkingLotID);
            if (parkingLot is null) return null;
            parkingLot.Vehicle = newVehicle;
            var dbParkingLot = database.ParkingLots.Attach(parkingLot);
            dbParkingLot.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return parkingLot;
        }
        public ParkingLot RemoveParkingLotVehicle(int VehicleID)
        {
            var parkingLot = GetParkingLotByVehicle(VehicleID);
            if (parkingLot == null) return null;
            parkingLot.Vehicle = null;
            var dbParkingLot = database.ParkingLots.Attach(parkingLot);
            dbParkingLot.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return parkingLot;
        }
        public Vehicle AddVehicle(Vehicle vehicle)
        {
            database.Vehicles.Add(vehicle);
            return vehicle;
        }
        public Vehicle RemoveVehicle(int VehicleID)
        {
            var vehicle = GetVehicleById(VehicleID);
            if (vehicle is not null) database.Vehicles.Remove(vehicle);
            return vehicle;
        }
        public ConfigDTO STARTCONFIG()
        {
            var cf = new DbConfig(config, database);
            return cf.StartParkingLotConfiguration();
        }
        public int Commit() => database.SaveChanges();
    }
}
