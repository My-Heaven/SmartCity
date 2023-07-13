using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblBooking
    {
        public TblBooking()
        {
            TblBookingDetails = new HashSet<TblBookingDetail>();
            TblBookingEmps = new HashSet<TblBookingEmp>();
        }

        public int BookingId { get; set; }
        public double? TotalAmount { get; set; }
        public int? StatusBooking { get; set; }
        public DateTime? DateOfBooking { get; set; }
        public int? AccountId { get; set; }
        public int EmpQuantity { get; set; }

        public virtual TblAccount? Account { get; set; }
        public virtual ICollection<TblBookingDetail> TblBookingDetails { get; set; }
        public virtual ICollection<TblBookingEmp> TblBookingEmps { get; set; }
    }
}
