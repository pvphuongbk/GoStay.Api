using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class NewsTopic
    {
        public NewsTopic()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string? Topic { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
