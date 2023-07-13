using System;
using System.Collections.Generic;

namespace SmartCity.Entities
{
    public partial class TblCommit
    {
        public int CommitId { get; set; }
        public string CommitName { get; set; } = null!;
        public string? Description { get; set; }
        public int? AccountId { get; set; }

        public virtual TblAccount? Account { get; set; }
    }
}
