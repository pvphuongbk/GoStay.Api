using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class NewsCategory
    {
        public NewsCategory()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Category { get; set; } = null!;
        public string? Keysearch { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
