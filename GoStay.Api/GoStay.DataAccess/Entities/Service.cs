using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Service
    {
        public Service()
        {
            HotelMamenitis = new HashSet<HotelMameniti>();
            RoomMamenitis = new HashSet<RoomMameniti>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Deleted { get; set; }
        public int? IdStyle { get; set; }
        public string? Icon { get; set; }

        public virtual ICollection<HotelMameniti> HotelMamenitis { get; set; }
        public virtual ICollection<RoomMameniti> RoomMamenitis { get; set; }
    }
}
