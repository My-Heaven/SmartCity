using Microsoft.EntityFrameworkCore;
using SmartCity.Models;
using SmartCity.Models.ResponseModel;
using SmartCity.Services.BookingdetailService;
using SmartCity.Entities;

namespace SmartCity.Services.BookingDetailService
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly SmartCityContext _context;

        public BookingDetailService(SmartCityContext context)
        {
            _context = context;
        }
        public async Task<Response<List<TblBookingDetail>>> GetAllBookingDetail()
        {
            var bookingDetails = await _context.TblBookingDetails.ToListAsync();
            return new Response<List<TblBookingDetail>>(bookingDetails);
        }

        public async Task<Response<TblBookingDetail>> GetBookingDetailById(int id)
        {
            var bookingDetail = await _context.TblBookingDetails.FirstOrDefaultAsync(x => x.OrderDetailId == id);
            if(bookingDetail != null)
            {
                return new Response<TblBookingDetail>(bookingDetail) { };
            }
            return new Response<TblBookingDetail>(null)
            {
                StatusCode = 400,
                Message = "Not found booking detail"
            };
        }
    }
}
