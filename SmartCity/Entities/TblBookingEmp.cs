using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblBookingEmp
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int EmpId { get; set; }
        public int AccountId { get; set; }
        public bool Confirm { get; set; }

        public virtual TblAccount Account { get; set; } = null!;
        public virtual TblBooking Booking { get; set; } = null!;
        public virtual TblEmployee Emp { get; set; } = null!;
    }
}
