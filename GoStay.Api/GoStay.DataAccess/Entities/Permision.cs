using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Permision
    {
        public Permision()
        {
            UserPermisions = new HashSet<UserPermision>();
        }

        public int Id { get; set; }
        public int Code { get; set; }
        public string Description { get; set; } = null!;
        public bool Deleted { get; set; }

        public virtual ICollection<UserPermision> UserPermisions { get; set; }
    }
}
