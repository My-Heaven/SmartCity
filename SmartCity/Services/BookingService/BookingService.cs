using Microsoft.EntityFrameworkCore;
using SmartCity.Models;
using SmartCity.Models.ResponseModel;
using SmartCity.Entities;
using SmartCity.Models.BookingModel;
using Microsoft.EntityFrameworkCore.Storage;
using SmartCity.Helper;
using System.Reflection.Metadata.Ecma335;

namespace SmartCity.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly SmartCityContext _context;
        private readonly DateTime today = LocalDateTime.DateTimeNow();
        public BookingService(SmartCityContext context)
        {
            _context = context;
        }

        public async Task<Response<bool>> CreateBooking(int userId, CreateBookingModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var service = await _context.TblServices.FirstOrDefaultAsync(x => x.ServiceId == model.ServiceId);

                TblBooking booking = new TblBooking()
                {
                    StatusBooking = 1,
                    DateOfBooking = today,
                    EmpQuantity = model.EmpQuantity,
                    AccountId = userId,
                    TotalAmount = (model.Quantity * service.PriceOfHour) * model.EmpQuantity
                };
                _context.TblBookings.Add(booking);
                await _context.SaveChangesAsync();

                TblBookingDetail bookingDetail = new TblBookingDetail()
                {
                    BookingId = booking.BookingId,
                    Price = service.PriceOfHour,
                    Quantity = model.Quantity,
                    ServiceId = model.ServiceId,
                    TotalPrice = (model.Quantity * service.PriceOfHour) * model.EmpQuantity
                };

                _context.TblBookingDetails.Add(bookingDetail);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new Response<bool>(true)
                {
                    Message = "Create booking succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 400,
                    Message = "Create booking fail"
                };
            }
        }

        public async Task<Response<bool>> DeleteBooking(int bookingId)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var bookking = await _context.TblBookings.FirstOrDefaultAsync(x => x.BookingId == bookingId);
                if (bookking == null)
                {
                    return new Response<bool>(false)
                    {
                        StatusCode = 400,
                        Message = "Not Found Booking"
                    };
                }
                bookking.StatusBooking = 4;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new Response<bool>(true)
                {
                    Message = "Delete Booking succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 500,
                    Message = "Delete Booking fail"
                };
            }
        }

        public async Task<Response<bool>> DoneTask(int BookingId)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var booking = await _context.TblBookings.FirstOrDefaultAsync(x => x.BookingId == BookingId);

                if(booking.StatusBooking == 2)
                {
                    var bookingEmpId = await _context.TblBookingEmps.Where(x => x.BookingId == BookingId).Select(x => x.EmpId).ToListAsync();

                    foreach(var id in bookingEmpId)
                    {
                        var emp = await _context.TblEmployees.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
                        emp.Status = 1;
                        await _context.SaveChangesAsync();
                    }

                    booking.StatusBooking = 3;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return new Response<bool>(true)
                    {
                        Message = "Done Task succesfully"
                    };
                }
                else
                {
                    return new Response<bool>(false)
                    {
                        StatusCode = 400,
                        Message = "booking has not been processed"
                    };
                }

                
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 400,
                    Message = "Done task fail"
                };
            }
        }

        public async Task<Response<List<TblBooking>>> GetAllBooking()
        {
            var bookings = await _context.TblBookings.ToListAsync();
            foreach(var booking in bookings)
            {
                booking.TblBookingEmps = await _context.TblBookingEmps.Where(x => x.BookingId == booking.BookingId).ToListAsync();
            }
            return new Response<List<TblBooking>>(bookings);
        }

        public async Task<Response<TblBooking>> GEtBookingById(int id)
        {
            var booking = await _context.TblBookings.FirstOrDefaultAsync(x => x.BookingId == id);
            if (booking != null)
            {
                return new Response<TblBooking>(booking);
            }
            return new Response<TblBooking>(null)
            {
                StatusCode = 400,
                Message = "Not found booking"
            };
        }

        public async Task<Response<List<TblBooking>>> GetBookingsByAccountId(int accId)
        {
            var bookings = await _context.TblBookings.Where(x => x.AccountId == accId).ToListAsync();
            foreach (var booking in bookings)
            {
                booking.TblBookingDetails = (ICollection<TblBookingDetail>?)_context.TblBookingDetails.FirstOrDefault(x => x.BookingId == booking.BookingId);
            }
            return new Response<List<TblBooking>>(bookings);
        }

        public async Task<Response<List<TblBooking>>> GetBookingsByEmpId(int empId)
        {
            var listBookingEmpId = await _context.TblBookingEmps.Where(x => x.EmpId == empId).Select(x => x.Booking).ToListAsync();
            return new Response<List<TblBooking>>(listBookingEmpId);
        }

        public async Task<Response<List<TblEmployee>>> GetEmpByBookingSkill(int bookingId)
        {
            var skillId = await _context.TblBookingDetails.Where(x => x.BookingId == bookingId).Select(x => x.Service.SkillId).FirstOrDefaultAsync();
            var listEmp = await _context.TblEmpSkills.Where(x => x.SkillId == skillId && x.Employee.Status == 1).Select(x => x.Employee).ToListAsync();
            return new Response<List<TblEmployee>>(listEmp);
        }
    }
}
