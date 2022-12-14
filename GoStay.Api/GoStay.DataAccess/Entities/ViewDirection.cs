using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class ViewDirection
    {
        public ViewDirection()
        {
            HotelRooms = new HashSet<HotelRoom>();
        }

        public int Id { get; set; }
        public string? ViewDirection1 { get; set; }
        public int? Deleted { get; set; }

        public virtual ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
