using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public class BillingService : IBillingService
    {
        private readonly IUnitOfWork unit;
        private readonly float price;

        public BillingService(IUnitOfWork unit)
        {
            this.unit = unit;
            this.price = 0.09f;
        }

        public BillingResult CreateBill(int UserID, Vehicle Vehicle, string ParkingLotName)
        {
            var bill = new Bill
            {
                User = unit.Users.Get(UserID),
                ParkingLot = ParkingLotName,
                VehiclePlate = Vehicle.PlateNumber,
                VehicleVIN = Vehicle.VIN,
                IssuedAt = DateTime.Now,
                ParkTime = Vehicle.ParkTime,
                BillValue = (DateTime.Now - Vehicle.ParkTime).TotalMinutes * this.price,
                IsPaid = true
            };
            unit.Bills.Add(bill);
            unit.Commit();
            return new BillingResult
            {
                Success = true,
                Bill = bill
            };
        }
        public BillingResult Audit()
        {
            var bills = unit.Bills.GetAll();
            return new BillingResult
            {
                Success = true,
                Message = "You have all the bills in the list",
                Bills = bills
            };
        }
        public BillingResult Bills(int PageIndex, int PageSize = 10)
        {
            return new BillingResult
            {
                Success = true,
                Message = $"You are on the page {PageIndex}",
                Bills = unit.Bills.GetBills(PageIndex, PageSize)
            };
        }
        public BillingResult ForUser(int UserID)
        {
            return new BillingResult
            {
                Success = true,
                Bills = unit.Bills.GetUserBills(UserID)
            };
        }

    }
    public class BillingResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Bill? Bill { get; set; }
        public IEnumerable<Bill>? Bills { get; set; }
    }
}
