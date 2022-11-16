using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class AspNetRoleClaim
    {
        public int Id { get; set; }
        public Guid RoleId { get; set; }
        public string? ClaimType { get; set; }
        public int ModuleValue { get; set; }

        public virtual AspNetRole Role { get; set; } = null!;
    }
}
