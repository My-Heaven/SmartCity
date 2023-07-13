using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblService
    {
        public TblService()
        {
            TblBookingDetails = new HashSet<TblBookingDetail>();
        }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string? Image { get; set; }
        public string? Description { get; set; }
        public double? PriceOfHour { get; set; }
        public double? EmployeePrice { get; set; }
        public int? SkillId { get; set; }

        public virtual TblSkill? Skill { get; set; }
        public virtual ICollection<TblBookingDetail> TblBookingDetails { get; set; }
    }
}
