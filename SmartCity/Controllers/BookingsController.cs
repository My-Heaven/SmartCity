using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Models.BookingModel;
using SmartCity.Models.EmployeeModels;
using SmartCity.Services.AccountServices;
using SmartCity.Services.BookingService;
using System.Security.Claims;

namespace SmartCity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService, IAccountService accountService)
        {
            _bookingService = bookingService;
            _accountService = accountService;

        }


        [Route("")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBooking();
            return StatusCode(bookings.StatusCode, bookings);
        }

        [Route("{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetBookingById([FromRoute] int id)
        {
            var bookings = await _bookingService.GEtBookingById(id);
            return StatusCode(bookings.StatusCode, bookings);
        }

        [Route("account/{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetBookingsByAccountId([FromRoute] int id)
        {
            var bookings = await _bookingService.GetBookingsByAccountId(id);
            return StatusCode(bookings.StatusCode, bookings);
        }

        [Authorize(Roles = "CUSTOMER")]
        [Route("")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> CreateBooking([FromForm] CreateBookingModel model)
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
            var result = await _bookingService.CreateBooking(userId, model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "ADMIN")]
        [Route("create-task")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> CreateTask([FromBody] CreateTaskModel model)
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
            var result = await _accountService.CreateTask(userId, model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "ADMIN")]
        [Route("done-task/{id}")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DoneTask([FromRoute] int id)
        {
            var result = await _bookingService.DoneTask(id);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "ADMIN")]
        [Route("choose-emp/{id}")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetEmpByBookingSkill([FromRoute] int id)
        {
            var result = await _bookingService.GetEmpByBookingSkill(id);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPatch("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteEmployee([FromRoute] int id)
        {
            var result = await _bookingService.DeleteBooking(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
