using SmartCity.Models;
using SmartCity.Models.AccountModel;
using SmartCity.Models.ResponseModel;
using SmartCity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartCity.Models.BookingModel;

namespace SmartCity.Services.AccountServices
{
    public interface IAccountService
    {
        Task<Response<TblAccount>> CreateCustomer(CreateAccountModel model);
        Task<Response<List<TblAccount>>> GetAllCustomer();
        Task<Response<TblAccount>> GetCustomerById(int id);
        Task<Response<object>> GetProfile(int userId, string roleName);
        Task<Response<bool>> CreateTask(int userId, CreateTaskModel model);
        Task<Response<bool>> UpdateAccount(int userId, UpdateAccountModel model);
    }
}
