using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartCity.Models;
using SmartCity.Models.AccountModel;
using SmartCity.Models.ResponseModel;
using System.Linq;
using SmartCity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using SmartCity.Models.BookingModel;
using System.Security.Principal;

namespace SmartCity.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly SmartCityContext _context;
        public AccountService(SmartCityContext context)
        {
            _context = context;
        }
        public async Task<Response<TblAccount>> CreateCustomer(CreateAccountModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                TblAccount account = new TblAccount()
                {
                    AccountEmail = model.AccountEmail,
                    AccountName = model.AccountName,
                    FullName = model.FullName,
                    Password = model.Password,
                    AccountPhone = model.AccountPhone,
                    RoleId = 2
                };
                _context.TblAccounts.Add(account);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new Response<TblAccount>(account)
                {
                    Message = "Create customer succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<TblAccount>(null)
                {
                    StatusCode = 400,
                    Message = "Create cutomer fail"
                };
            }
        }

        public async Task<Response<bool>> CreateTask(int userId, CreateTaskModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                bool checkExit = CheckEmpInTask(model.EmpID, model.BookingId);
                if (checkExit == true) return new Response<bool>(false)
                {
                    StatusCode = 400,
                    Message = "Emp has been selected in this booking"
                };
                var booking = await _context.TblBookings.FirstOrDefaultAsync(x => x.BookingId == model.BookingId);

                var bookingEmpQuantity = await _context.TblBookingEmps.Where(x => x.BookingId == model.BookingId).CountAsync();
                if (bookingEmpQuantity - booking.EmpQuantity == 0)
                {
                    return new Response<bool>(false)
                    {
                        StatusCode = 400,
                        Message = "Quantity of employee invalid"
                    };
                }
                    TblBookingEmp bookingEmp = new TblBookingEmp()
                    {
                        BookingId = model.BookingId,
                        EmpId = model.EmpID,
                        AccountId = userId,
                        Confirm = false
                    };
                    _context.TblBookingEmps.Add(bookingEmp);
                    await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new Response<bool>(true)
                {
                    Message = "Create task succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 400,
                    Message = "Create task fail"
                };
            }
        }

        private bool CheckEmpInTask(int empId, int bookingId)
        {
            var result = _context.TblBookingEmps.Where(x => x.EmpId == empId & x.BookingId == bookingId).FirstOrDefault();
            if(result != null)
            {
                return true;
            }
            return false;
        }

        public async Task<Response<List<TblAccount>>> GetAllCustomer()
        {
            var query = from c in _context.TblAccounts
                        select c;
            var result = await query.OrderByDescending(x => x.AccountId).Where(x => x.RoleId == 2).ToListAsync();

            if (result.Count == 0)
            {
                return new Response<List<TblAccount>>()
                {
                    StatusCode = 204,
                    Message = "No Customer"
                };
            }

            return new Response<List<TblAccount>>(result)
            {
                StatusCode = 200,
                Message = "List Customer"
            };

        }

        public async Task<Response<TblAccount>> GetCustomerById(int id)
        {
            var customer = await _context.TblAccounts.Where(x => x.RoleId == 2).FirstOrDefaultAsync(x => x.AccountId == id);
            if(customer != null)
            {
                return new Response<TblAccount>(customer) { };
            }
            return new Response<TblAccount>(null)
            {
                StatusCode = 400,
                Message = "Not Found Customer"
            };
        }

        public async Task<Response<object>> GetProfile(int userId, String roleName)
        {
            if(roleName.Equals("ADMIN") || roleName.Equals("CUSTOMER"))
            {
                var profile = await _context.TblAccounts.FirstOrDefaultAsync(x => x.AccountId == userId);
                return new Response<object>(profile);
            }else if (roleName.Equals("EMP")){
                var profile = await _context.TblEmployees.FirstOrDefaultAsync(x => x.EmployeeId == userId);
                return new Response<object>(profile);
            }
            return new Response<object>(null)
            {
                StatusCode = 400,
                Message = "Not login"
            };
        }

        public async Task<Response<bool>> UpdateAccount(int userId, UpdateAccountModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var account = await _context.TblAccounts.FirstOrDefaultAsync(x => x.AccountId == userId);
                if (account == null)
                {
                    return new Response<bool>(false)
                    {
                        StatusCode = 400,
                        Message = "Not Found Account"
                    };

                }

                account.AccountPhone = model.AccountPhone;
                account.AccountEmail = model.AccountEmail;
                account.FullName = model.FullName;
                account.Password = model.Password;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new Response<bool>(true)
                {
                    Message = "Update account succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 500,
                    Message = "Update account fail"
                };
            }
        }
    }
}
