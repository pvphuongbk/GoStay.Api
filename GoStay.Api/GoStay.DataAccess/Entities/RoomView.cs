using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class RoomView
    {
        public int Id { get; set; }
        public int IdRoom { get; set; }
        public int IdView { get; set; }

        public virtual HotelRoom? IdRoomNavigation { get; set; }
        public virtual ViewDirection? IdViewNavigation { get; set; }
    }
}
