using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Models.CommitModel;
using SmartCity.Models.EmployeeModels;
using SmartCity.Services.AccountServices;
using SmartCity.Services.BookingService;
using SmartCity.Services.EmployeeServices;
using System.Security.Claims;

namespace SmartCity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IBookingService _bookingService;
        public EmployeesController(IEmployeeService employeeService, IBookingService bookingService)
        {
            _employeeService = employeeService;
            _bookingService = bookingService;
        }

        [Route("")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllEmployees()
        {
            var emps = await _employeeService.GetAllEmp();
            return StatusCode(emps.StatusCode, emps);
        }

        [Route("status/{status}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetEmpsByStatus([FromRoute] int status)
        {
            var emps = await _employeeService.GEtEmpByStatus(status);
            return StatusCode(emps.StatusCode, emps);
        }

        [Route("{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetEmpById([FromRoute] int id)
        {
            var emp = await _employeeService.GetEmpById(id);
            return StatusCode(emp.StatusCode, emp);
        }

        [Authorize(Roles = "ADMIN")]
        [Route("")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> CreateEmployee([FromForm] CreateEmployeeModel model)
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
            var result = await _employeeService.CreateEmployee(userId, model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "EMP")]
        [Route("")]
        [HttpPut]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> UpdateProfileEmp([FromBody] UpdateEmpModel model)
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
            var result = await _employeeService.UpdateProfileEmp(userId, model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "EMP")]
        [Route("confirm-task")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ConfirmTask([FromForm] ConfirmTaskModel model)
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
            var result = await _employeeService.ConfirmTask(userId, model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteEmployee([FromRoute] int id)
        {
            var result = await _employeeService.DeleteEmployee(id);
            return StatusCode(result.StatusCode, result);
        }

        [Route("bookings")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetBookingsByEmployeeId()
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
            var bookings = await _bookingService.GetBookingsByEmpId(userId);
            return StatusCode(bookings.StatusCode, bookings);
        }
    }
}
