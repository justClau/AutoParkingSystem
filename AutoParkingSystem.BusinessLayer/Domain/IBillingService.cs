using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Core;

namespace AutoParkingSystem.BusinessLayer.Domain
{
    public interface IBillingService
    {
        BillingResult Audit();
        BillingResult Bills(int PageIndex, int PageSize = 10);
        BillingResult CreateBill(int UserID, Vehicle Vehicle, string ParkingLotName);
        BillingResult GetUserBills(int UserID);
    }
}