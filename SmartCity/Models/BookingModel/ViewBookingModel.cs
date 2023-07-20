using SmartCity.Entities;

namespace SmartCity.Models.BookingModel
{
    public class ViewBookingModel
    {
        public int BookingId { get; set; }
        public double? TotalAmount { get; set; }
        public int? StatusBooking { get; set; }
        public DateTime? DateOfBooking { get; set; }
        public int? AccountId { get; set; }
        public int EmpQuantity { get; set; }
        public TblBookingDetail? TblBookingDetail { get; set; }
    }
}
