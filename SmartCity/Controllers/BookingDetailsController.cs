using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Services.BookingdetailService;
using SmartCity.Services.BookingDetailService;
using SmartCity.Services.BookingService;

namespace SmartCity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingDetailsController : ControllerBase
    {
        private readonly IBookingDetailService _bookingDetailService;

        public BookingDetailsController(IBookingDetailService bookingDetailService)
        {
            _bookingDetailService = bookingDetailService;
        }


        [Route("")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllBookingDetails()
        {
            var bookings = await _bookingDetailService.GetAllBookingDetail();
            return StatusCode(bookings.StatusCode, bookings);
        }

        [Route("{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetBookingDetailById([FromRoute] int id)
        {
            var bookings = await _bookingDetailService.GetBookingDetailById(id);
            return StatusCode(bookings.StatusCode, bookings);
        }

    }
}
