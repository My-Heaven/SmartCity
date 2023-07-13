using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblEmployee
    {
        public TblEmployee()
        {
            TblBookingEmps = new HashSet<TblBookingEmp>();
            TblEmpSkills = new HashSet<TblEmpSkill>();
        }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Image { get; set; }
        public string? FullName { get; set; }
        public string? EmployeePhone { get; set; }
        public string? EmployeeEmail { get; set; }
        public int? Status { get; set; }
        public int? AccountId { get; set; }

        public virtual TblAccount? Account { get; set; }
        public virtual ICollection<TblBookingEmp> TblBookingEmps { get; set; }
        public virtual ICollection<TblEmpSkill>? TblEmpSkills { get; set; }
    }
}
