using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblAccounts = new HashSet<TblAccount>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<TblAccount> TblAccounts { get; set; }
    }
}
