using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartCity.Entities;
using SmartCity.Models;
using SmartCity.Models.Authentication;
using SmartCity.Services.AccountServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SmartCity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly SmartCityContext _context;
        private readonly IAccountService _accountService;
        public AuthenticationController(SmartCityContext context, IConfiguration configuration, IAccountService accountService)
        {
            _context = context;
            _configuration = configuration;
            _accountService = accountService;
        }

        [Authorize]
        [Route("profile/")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetProfile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userId;
            string roleName;
            try
            {
                userId = Convert.ToInt32(claim[3].Value);
                roleName = Convert.ToString(claim[7].Value);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "You are not login" });
            }

            var customers = await _accountService.GetProfile(userId, roleName);
            return StatusCode(customers.StatusCode, customers);
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginModel _userData)
        {
            if (_userData != null && _userData.UserName != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.UserName, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.AccountId.ToString()),
                        new Claim("DisplayName", user.FullName),
                        new Claim("UserName", user.AccountName),
                        new Claim("Email", user.AccountEmail),
                        new Claim(ClaimTypes.Role, user.RoleId == 1 ? "ADMIN" : "CUSTOMER")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(7),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("employees")]
        [HttpPost]
        public async Task<IActionResult> PostEmp(LoginModel _userData)
        {
            if (_userData != null && _userData.UserName != null && _userData.Password != null)
            {
                var user = await GetEmployee(_userData.UserName, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.EmployeeId.ToString()),
                        new Claim("DisplayName", user.FullName),
                        new Claim("UserName", user.EmployeeName),
                        new Claim("Email", user.EmployeeEmail),
                        new Claim(ClaimTypes.Role, "EMP")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(7),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<TblAccount> GetUser(string accountName, string password)
        {
            return await _context.TblAccounts.FirstOrDefaultAsync(u => u.AccountName == accountName && u.Password == password);
        }

        private async Task<TblEmployee> GetEmployee(string employeeName, string password)
        {
            return await _context.TblEmployees.FirstOrDefaultAsync(u => u.EmployeeName == employeeName && u.Password == password);
        }
    }
}

