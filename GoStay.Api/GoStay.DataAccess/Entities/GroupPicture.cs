using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class GroupPicture
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Deleted { get; set; }
    }
}
