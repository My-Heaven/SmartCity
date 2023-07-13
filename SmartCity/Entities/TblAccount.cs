using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblAccount
    {
        public TblAccount()
        {
            TblBookingEmps = new HashSet<TblBookingEmp>();
            TblBookings = new HashSet<TblBooking>();
            TblCommits = new HashSet<TblCommit>();
            TblEmployees = new HashSet<TblEmployee>();
        }

        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public string? FullName { get; set; }
        public string? AccountPhone { get; set; }
        public string? AccountEmail { get; set; }
        public int? RoleId { get; set; }

        public virtual TblRole? Role { get; set; }
        public virtual ICollection<TblBookingEmp> TblBookingEmps { get; set; }
        public virtual ICollection<TblBooking> TblBookings { get; set; }
        public virtual ICollection<TblCommit> TblCommits { get; set; }
        public virtual ICollection<TblEmployee> TblEmployees { get; set; }
    }
}
