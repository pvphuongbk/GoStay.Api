using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class AspNetRoleModule
    {
        public int Id { get; set; }
        public string MenuId { get; set; } = null!;
        public Guid RoleId { get; set; }
        public bool? IsFull { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }
    }
}
