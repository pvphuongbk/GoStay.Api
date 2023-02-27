using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Album : BaseEntity
    {
        public Album()
        {
            Pictures = new HashSet<Picture>();
        }


        public int? TypeAlbum { get; set; }
        public int? IdType { get; set; }
        public int? IdHotel { get; set; }
        public int? IdRoom { get; set; }
        public string? Name { get; set; }

        public virtual Hotel? IdHotelNavigation { get; set; }
        public virtual HotelRoom? IdRoomNavigation { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
