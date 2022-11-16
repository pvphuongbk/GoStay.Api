using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class AspNetUserClaim
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? ClaimType { get; set; }
        public int ClaimValue { get; set; }
    }
}
