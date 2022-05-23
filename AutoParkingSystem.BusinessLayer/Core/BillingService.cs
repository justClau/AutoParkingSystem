using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Domain;
using Microsoft.Extensions.Configuration;

namespace AutoParkingSystem.BusinessLayer.Core
{
    public class BillingService : IBillingService
    {
        private readonly IUnitOfWork unit;
        private readonly float price;

        public BillingService(IUnitOfWork unit, IConfiguration config)
        {
            this.unit = unit;
            this.price = float.Parse(config.GetSection("ParkingSystem").GetSection("ParkingPrice").Value);
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
                StartingTime = Vehicle.StartingTime,
                BillValue = (DateTime.Now - Vehicle.StartingTime).TotalMinutes * this.price,
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
                Message = "You have all the bills in the following list",
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
        public BillingResult GetUserBills(int UserID)
        {
            return new BillingResult
            {
                Success = true,
                Bills = unit.Bills.GetBillsByUserId(UserID)
            };
        }

    }
}
