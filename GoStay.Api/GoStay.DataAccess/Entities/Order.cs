using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public int IdUser { get; set; }
        public int? IdHotel { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
        public byte? Status { get; set; }
        public byte IdPaymentMethod { get; set; }
        public string? MoreInfo { get; set; }
        public string? Session { get; set; }
        public bool IsDeleted { get; set; }
        public string? Ordercode { get; set; }
        public int? Adult { get; set; }
        public int? Children { get; set; }
        public int? Infant { get; set; }
        public decimal? Prepayment { get; set; }
        public byte? Style { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? IdRoom { get; set; }
        public int? IdTour { get; set; }
        public decimal? Price { get; set; }
        public double? Discount { get; set; }
        public byte? NumNight { get; set; }
        public byte? NumRoom { get; set; }

        public string? MoreInfor { get; set; }

        public virtual OrderPhuongThucTt IdPaymentMethodNavigation { get; set; } = null!;
        public virtual HotelRoom? IdRoomNavigation { get; set; }
        public virtual Tour? IdTourNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual OrderStatus? StatusNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
