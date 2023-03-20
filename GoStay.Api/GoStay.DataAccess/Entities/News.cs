using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class News
    {
        public News()
        {
            Pictures = new HashSet<Picture>();
        }

        public int Id { get; set; }
        public int IdCategory { get; set; }
        public int IdUser { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateEdit { get; set; }
        public byte Status { get; set; }
        public string? Keysearch { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public int Deleted { get; set; }

        public virtual NewsCategory IdCategoryNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
