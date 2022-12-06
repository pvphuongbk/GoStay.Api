using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelRoom
    {
        public HotelRoom()
        {
            OrderRooms = new HashSet<OrderRoom>();
            Pictures = new HashSet<Picture>();
            RoomMamenitis = new HashSet<RoomMameniti>();
        }

        public int Id { get; set; }
        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public byte? RemainNum { get; set; }
        public decimal PriceValue { get; set; }
        public int? Deleted { get; set; }
        public double? Discount { get; set; }
        public decimal? NewPrice { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public int? ViewDirection { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public long? IntDate { get; set; }
        public int? Iduser { get; set; }

        public virtual Hotel? IdhotelNavigation { get; set; }
        public virtual User? IduserNavigation { get; set; }
        public virtual ViewDirection? ViewDirectionNavigation { get; set; }
        public virtual ICollection<OrderRoom> OrderRooms { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<RoomMameniti> RoomMamenitis { get; set; }
    }
}
