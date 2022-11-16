using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
        }

        public Guid Id { get; set; }
        public string MaQuyen { get; set; } = null!;
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public int? Stt { get; set; }
        public int? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }

        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }
    }
}
