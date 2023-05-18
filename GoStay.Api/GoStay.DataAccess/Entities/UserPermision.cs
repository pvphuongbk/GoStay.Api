using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class UserPermision
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdPermision { get; set; }
        public bool Deleted { get; set; }

        public virtual Permision IdPermisionNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
