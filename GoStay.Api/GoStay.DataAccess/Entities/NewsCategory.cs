using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class NewsCategory
    {
        public NewsCategory()
        {
            News = new HashSet<News>();
            VideoNews = new HashSet<VideoNews>();
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Category { get; set; } = null!;
        public string? Keysearch { get; set; }
        public int? Iddomain { get; set; }
        public string? CategoryEng { get; set; }
        public string? CategoryChi { get; set; }

        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<VideoNews> VideoNews { get; set; }
    }
}
