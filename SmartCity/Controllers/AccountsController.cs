using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Models.AccountModel;
using SmartCity.Models.EmployeeModels;
using SmartCity.Services.AccountServices;
using SmartCity.Services.EmployeeServices;
using System.Security.Claims;

namespace SmartCity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("customers/")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllCustomers()
        {
            var customers = await _accountService.GetAllCustomer();
            return StatusCode(customers.StatusCode, customers);
        }

        [Route("customers/{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetCustomersById([FromRoute] int id)
        {
            var customers = await _accountService.GetCustomerById(id);
            return StatusCode(customers.StatusCode, customers);
        }

        [Route("customer/")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> CreateCustomer([FromForm] CreateAccountModel model)
        {
            var result = await _accountService.CreateCustomer(model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [Route("")]
        [HttpPut]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> UpdateProfileAccount([FromForm] UpdateAccountModel model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userId;
            try
            {
                userId = Convert.ToInt32(claim[3].Value);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "You are not login" });
            }
            var result = await _accountService.UpdateAccount(userId, model);
            return StatusCode(result.StatusCode, result);
        }
    }
}
