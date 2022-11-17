using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Album
    {
        public Album()
        {
            Pictures = new HashSet<Picture>();
        }

        public int Id { get; set; }
        public int? TypeAlbum { get; set; }
        public int? IdType { get; set; }
        public int? Deleted { get; set; }
        public string? Name { get; set; }

        public virtual Hotel? IdTypeNavigation { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
