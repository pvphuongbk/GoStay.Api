using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TypeHotel : BaseEntity
    {
        public TypeHotel()
        {
            Hotels = new HashSet<Hotel>();
        }
        public string? Type { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
