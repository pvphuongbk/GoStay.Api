using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TypeHotel
    {
        public TypeHotel()
        {
            Hotels = new HashSet<Hotel>();
        }

        public int Id { get; set; }
        public string? Type { get; set; }
        public int? Deleted { get; set; }

        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
