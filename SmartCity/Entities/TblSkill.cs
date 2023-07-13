using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblSkill
    {
        public TblSkill()
        {
            TblEmpSkills = new HashSet<TblEmpSkill>();
            TblServices = new HashSet<TblService>();
        }

        public int SkillId { get; set; }
        public string? SkillName { get; set; }

        public virtual ICollection<TblEmpSkill> TblEmpSkills { get; set; }
        public virtual ICollection<TblService> TblServices { get; set; }
    }
}
