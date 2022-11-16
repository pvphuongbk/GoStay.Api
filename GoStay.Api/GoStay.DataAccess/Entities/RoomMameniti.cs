using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class RoomMameniti
    {
        public int Id { get; set; }
        public int Idroom { get; set; }
        public int Idservices { get; set; }

        public virtual HotelRoom IdroomNavigation { get; set; } = null!;
        public virtual Service IdservicesNavigation { get; set; } = null!;
    }
}
