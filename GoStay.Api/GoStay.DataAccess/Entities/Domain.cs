using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Domain
    {
        public Domain()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string? Domain1 { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
