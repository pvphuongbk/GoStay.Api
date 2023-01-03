using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class News
    {
        public News()
        {
            NewsGalleries = new HashSet<NewsGallery>();
        }

        public int Id { get; set; }
        public int Idcat { get; set; }
        public int Idu { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateEdit { get; set; }
        public byte Status { get; set; }
        public string? Keysearch { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }

        public virtual NewsCategory IdcatNavigation { get; set; } = null!;
        public virtual ICollection<NewsGallery> NewsGalleries { get; set; }
    }
}
