using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Language
    {
        public Language()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string? Language1 { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
