using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelRoom : BaseEntity
    {
        public HotelRoom()
        {
            Albums = new HashSet<Album>();
            OrderDetails = new HashSet<OrderDetail>();
            Orders = new HashSet<Order>();
            Pictures = new HashSet<Picture>();
            RoomMamenitis = new HashSet<RoomMameniti>();
            RoomViews = new HashSet<RoomView>();
        }

        public int Idhotel { get; set; }
        public string Name { get; set; } = null!;
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public byte? RemainNum { get; set; }
        public decimal PriceValue { get; set; }
        public double? Discount { get; set; }
        public decimal NewPrice { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public string? SearchKey { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public long? IntDate { get; set; }
        public int Iduser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RoomStatus { get; set; }
        public byte MinNight { get; set; }
        public int? DeadLinePreOrder { get; set; }
        public decimal? CurrentPrice { get; set; }

        public virtual Hotel IdhotelNavigation { get; set; } = null!;
        public virtual Palletbed? PalletbedNavigation { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<RoomMameniti> RoomMamenitis { get; set; }
        public virtual ICollection<RoomView> RoomViews { get; set; }
    }
}
