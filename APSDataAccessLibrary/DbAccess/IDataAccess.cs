using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DbAccess
{
    public interface IDataAccess
    {
        ParkingLot AddParkingLotVehicle(Vehicle newVehicle);
        User AddUser(User user);
        Vehicle AddVehicle(Vehicle vehicle);
        int Commit();
        User DeleteUser(int id);
        Bill GetBillById(int id);
        IEnumerable<Bill> GetBills();
        IEnumerable<ParkingLot> GetFloorFreeParkingLots(int FloorNumber);
        IEnumerable<ParkingLot> GetFreeParkingLots();
        IEnumerable<Vehicle> GetParkedVehicles();
        ParkingLot GetParkingLotById(int id);
        ParkingLot GetParkingLotByName(int floorNumber, string name);
        ParkingLot GetParkingLotByVehicle(int id);
        IEnumerable<ParkingLot> GetParkingLots();
        IEnumerable<Bill> GetUserBills(int id);
        User GetUserById(int id);
        User GetUserByUsername(string name);
        Vehicle GetUserParkedVehicle(int id);
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetAllUsers();
        Vehicle GetVehicleById(int id);
        Vehicle GetVehicleByPlate(string Plate);
        Vehicle GetVehicleByVIN(string VIN);
        ParkingLot RemoveParkingLotVehicle(int VehicleID);
        User RemoveUserVehicle(int UserID);
        Vehicle RemoveVehicle(int VehicleID);
        Bill SaveBill(Bill bill);
        User SetAdmin(int UserID, bool AdminStatus);
        ParkingLot SetParkingLotVehicle(int ParkingLotID, Vehicle newVehicle);
        User SetUserVehicle(int UserID, Vehicle newVehicle);
        ConfigDTO STARTCONFIG();
    }
}