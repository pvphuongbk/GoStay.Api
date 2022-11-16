using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Album : BaseEntity
    {
        public int? TypeAlbum { get; set; }
        public int? IdType { get; set; }
        public string? Name { get; set; }
    }
}
