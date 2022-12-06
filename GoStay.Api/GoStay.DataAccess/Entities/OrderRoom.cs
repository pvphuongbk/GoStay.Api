using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderRoom
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdRoom { get; set; }
        public DateTime? ChechIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public byte? NumRoom { get; set; }
        public byte? Style { get; set; }
        public DateTime? DateCreate { get; set; }

        public virtual HotelRoom? IdRoomNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }
    }
}
