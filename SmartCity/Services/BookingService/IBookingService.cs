using Microsoft.EntityFrameworkCore.Metadata;
using SmartCity.Models;
using SmartCity.Models.ResponseModel;
using SmartCity.Entities;
using SmartCity.Models.BookingModel;

namespace SmartCity.Services.BookingService
{
    public interface IBookingService
    {
        Task<Response<List<TblBooking>>> GetAllBooking();
        Task<Response<TblBooking>> GEtBookingById(int id);
        Task<Response<List<TblBooking>>> GetBookingsByEmpId(int empId);
        Task<Response<List<ViewBookingModel>>> GetBookingsByAccountId(int accId);
        Task<Response<bool>> CreateBooking(int userId, CreateBookingModel model);
        Task<Response<bool>> DoneTask(int BookingId);
        Task<Response<List<TblEmployee>>> GetEmpByBookingSkill(int bookingId);
        Task<Response<bool>> DeleteBooking(int bookingId);
    }
}
