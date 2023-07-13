using SmartCity.Models;
using SmartCity.Models.ResponseModel;
using SmartCity.Entities;

namespace SmartCity.Services.BookingdetailService
{
    public interface IBookingDetailService
    {
        Task<Response<List<TblBookingDetail>>> GetAllBookingDetail();
        Task<Response<TblBookingDetail>> GetBookingDetailById(int id);
    }
}
