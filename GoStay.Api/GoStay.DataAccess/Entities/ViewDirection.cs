using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class ViewDirection : BaseEntity
    {
        public ViewDirection()
        {
            HotelRooms = new HashSet<HotelRoom>();
        }


        public string? NameView { get; set; }


        public virtual ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
