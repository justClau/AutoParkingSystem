using AutoParkingSystem.BusinessLayer.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillingService billing;
        private readonly IValidationService validation;

        public BillsController(IBillingService billing, IValidationService validation)
        {
            this.billing = billing;
            this.validation = validation;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var audit = billing.Audit();
            if (audit.Success == false)
                return BadRequest(audit);
            return Ok(audit);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetBillsForUser(string username)
        {
            var user = validation.UserExists(username);
            if(user.Success == false)
                return BadRequest(user);
            var bills = billing.GetUserBills(user.UserID);
            if(bills.Success == false)
                return NotFound(bills);
            return Ok(bills);
        }
    }
}
