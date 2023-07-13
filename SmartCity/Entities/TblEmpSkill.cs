using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblEmpSkill
    {
        public int EmpSkillId { get; set; }
        public int? EmployeeId { get; set; }
        public int? SkillId { get; set; }

        public virtual TblEmployee? Employee { get; set; }
        public virtual TblSkill? Skill { get; set; }
    }
}
