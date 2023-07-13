using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartCity.Models;
using SmartCity.Models.EmployeeModels;
using SmartCity.Models.ResponseModel;
using System.Linq;
using SmartCity.Entities;

namespace SmartCity.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly SmartCityContext _context;

        public EmployeeService(SmartCityContext context)
        {
            _context = context;
        }

        public async Task<Response<bool>> ConfirmTask(int userId, ConfirmTaskModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                if(model.Confirm == true)
                {
                    var bookingemp = await _context.TblBookingEmps.FirstOrDefaultAsync(x => x.BookingId == model.BookingId & x.EmpId == userId);
                    if(bookingemp != null)
                    {
                        bookingemp.Confirm = true;

                        await _context.SaveChangesAsync();
                        var booking = await _context.TblBookings.FirstOrDefaultAsync(x => x.BookingId == model.BookingId);

                        int empBooking = await _context.TblBookingEmps.Where(x => x.BookingId == model.BookingId & x.Confirm == true).CountAsync();
                        
                        if(booking.EmpQuantity == empBooking)
                        {
                            var emp = await _context.TblEmployees.Where(x => x.EmployeeId == userId).FirstOrDefaultAsync();
                            emp.Status = 2;
                            booking.StatusBooking = 2;
                            await _context.SaveChangesAsync();

                        }
                        
                        await transaction.CommitAsync();
                        return new Response<bool>(true)
                        {
                            Message = "Confirm successfully"
                        };
                    }
                    else
                    {
                        return new Response<bool>(false)
                        {
                            StatusCode = 400,
                            Message = "Employee not valid"
                        };
                    }
                        
                }
                else
                {
                    var bookingemp = await _context.TblBookingEmps.FirstOrDefaultAsync(x => x.BookingId == model.BookingId & x.EmpId == userId);
                    if (bookingemp != null)
                    {

                        _context.TblBookingEmps.Remove(bookingemp);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return new Response<bool>(true)
                        {
                            Message = "Reject successfully"
                        };
                    }
                    else
                    {
                        return new Response<bool>(false)
                        {
                            StatusCode = 400,
                            Message = "Employee not valid"
                        };
                    }

                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 400,
                    Message = "Confirm task fail"
                };
            }
        }

        public async Task<Response<TblEmployee>> CreateEmployee(int userId, CreateEmployeeModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                TblEmployee emp = new TblEmployee()
                {
                    AccountId = userId,
                    EmployeeEmail = model.EmployeeEmail,
                    EmployeeName = model.EmployeeName,
                    EmployeePhone = model.EmployeePhone,
                    FullName = model.FullName,
                    Image = model.Image,
                    Password = model.Password,
                    Status = 1,
                };
                _context.TblEmployees.Add(emp);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new Response<TblEmployee>(emp)
                {
                    Message = "Create employee succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<TblEmployee>(null)
                {
                    StatusCode = 400,
                    Message = "Create employee fail"
                };
            }
        }

        public async Task<Response<bool>> DeleteEmployee(int EmpId)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var emp = await _context.TblEmployees.FirstOrDefaultAsync(x => x.EmployeeId == EmpId);
                if(emp == null)
                {
                    return new Response<bool>(false)
                    {
                        StatusCode = 400,
                        Message = "Not Found Employee"
                    };
                }
                emp.Status = 2;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new Response<bool>(true)
                {
                    Message = "Delete employee succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 400,
                    Message = "Delete employee fail"
                };
            }
        }

        public async Task<Response<List<EmployeeViewModel>>> GetAllEmp()
        {

            var employees = await _context.TblEmployees.Select(x => new EmployeeViewModel()
            {
                EmployeeId = x.EmployeeId,
                AccountId = x.AccountId,
                EmployeeEmail = x.EmployeeEmail,
                EmployeeName = x.EmployeeName,
                FullName = x.FullName,
                Image = x.Image,
                Status = x.Status
            }).ToListAsync();
            if (employees.Count == 0)
            {
                return new Response<List<EmployeeViewModel>>(employees)
                {
                    StatusCode = 204,
                    Message = "Not have Employee"
                };
            }
            return new Response<List<EmployeeViewModel>>(employees)
            {
                StatusCode = 200,
                Message = "Not have Employee"
            };
        }

        public async Task<Response<EmployeeViewModel>> GetEmpById(int id)
        {
            var employee = await _context.TblEmployees.FirstOrDefaultAsync(x => x.EmployeeId == id);

            if(employee != null)
            {
                EmployeeViewModel result = new EmployeeViewModel()
                {
                    EmployeeId = employee.EmployeeId,
                    AccountId = employee.AccountId,
                    EmployeeEmail = employee.EmployeeEmail,
                    EmployeeName = employee.EmployeeName,
                    FullName = employee.FullName,
                    Image = employee.Image,
                    Status = employee.Status,
                    EmployeePhone = employee.EmployeePhone
                };
                return new Response<EmployeeViewModel>(result) { };
            }
            return new Response<EmployeeViewModel>(null)
            {
                StatusCode = 400,
                Message = "Not Found Employee"
            };
        }

        public async Task<Response<List<EmployeeViewModel>>> GEtEmpByStatus(int status)
        {
            var emps = await _context.TblEmployees.Where(x => x.Status == status).Select(x => new EmployeeViewModel()
            {
                AccountId = x.AccountId,
                Status = x.Status,
                EmployeeEmail = x.EmployeeEmail,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.EmployeeName,
                FullName = x.FullName,
                Image = x.Image,
                EmployeePhone = x.EmployeePhone
            }).ToListAsync();
            return new Response<List<EmployeeViewModel>>(emps);
        }

        public async Task<Response<bool>> UpdateProfileEmp(int userId, UpdateEmpModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var emp = await _context.TblEmployees.FirstOrDefaultAsync(x => x.EmployeeId == userId);
                if (emp == null)
                {
                    return new Response<bool>(false)
                    {
                        StatusCode = 400,
                        Message = "Not Found Employee"
                    };
                    
                }

                emp.EmployeeEmail = model.EmployeeEmail;
                emp.EmployeeName = model.EmployeeName;
                emp.EmployeePhone = model.EmployeePhone;
                emp.FullName = model.FullName;
                emp.Password = model.Password;
                emp.Image = model.Image;

                List<int> listSkillIdOld = await _context.TblEmpSkills.Where(x => x.EmployeeId == userId).Select(x => x.EmpSkillId).ToListAsync();

                if(model.ListSkillId.Count >= 0)
                {
                    if(listSkillIdOld.Count > 0)
                    {
                        foreach(var empSkillId in listSkillIdOld)
                        {
                            var empSkill = await _context.TblEmpSkills.FirstOrDefaultAsync(x => x.EmpSkillId == empSkillId);
                            if(empSkill != null)
                            {
                                _context.TblEmpSkills.Remove(empSkill);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    if (model.ListSkillId.Count > 0)
                    {
                        foreach (var id in model.ListSkillId)
                        {
                            TblEmpSkill empSkill = new TblEmpSkill()
                            {
                                EmployeeId = userId,
                                SkillId = id.SkillId
                            };

                            _context.TblEmpSkills.Add(empSkill);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new Response<bool>(true)
                {
                    Message = "Update employee succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 500,
                    Message = "Update employee fail"
                };
            }
        }
    }
}
