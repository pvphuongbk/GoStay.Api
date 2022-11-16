using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelRoom : BaseEntity
    {
        public HotelRoom()
        {
            RoomMamenitis = new HashSet<RoomMameniti>();
        }

        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public int? ViewDirection { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public virtual Hotel? IdhotelNavigation { get; set; }
        public virtual ViewDirection? ViewDirectionNavigation { get; set; }
        public virtual ICollection<RoomMameniti> RoomMamenitis { get; set; }
    }


}
