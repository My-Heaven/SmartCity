using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblBookingDetail
    {
        public int OrderDetailId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public double? TotalPrice { get; set; }
        public int? BookingId { get; set; }
        public int? ServiceId { get; set; }

        public virtual TblBooking? Booking { get; set; }
        public virtual TblService? Service { get; set; }
    }
}
