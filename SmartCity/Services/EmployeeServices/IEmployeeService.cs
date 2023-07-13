using SmartCity.Models;
using SmartCity.Models.EmployeeModels;
using SmartCity.Models.ResponseModel;
using SmartCity.Entities;

namespace SmartCity.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<Response<EmployeeViewModel>> GetEmpById(int id);
        Task<Response<List<EmployeeViewModel>>> GetAllEmp();
        Task<Response<TblEmployee>> CreateEmployee(int userId, CreateEmployeeModel model);
        Task<Response<bool>> ConfirmTask(int userId, ConfirmTaskModel model);
        Task<Response<bool>> DeleteEmployee(int EmpId);
        Task<Response<List<EmployeeViewModel>>> GEtEmpByStatus(int status);
        Task<Response<bool>> UpdateProfileEmp(int userId, UpdateEmpModel model);
    }
}
