using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public interface IBillingService
    {
        BillingResult Audit();
        BillingResult Bills(int PageIndex, int PageSize = 10);
        BillingResult CreateBill(int UserID, Vehicle Vehicle, string ParkingLotName);
        BillingResult ForUser(int UserID);
    }
}