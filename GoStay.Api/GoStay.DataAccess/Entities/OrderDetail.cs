using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdRoom { get; set; }
        public int IdTour { get; set; }
        public DateTime? ChechIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public byte? Num { get; set; }
        public DateTime? DateCreate { get; set; }
        public decimal Price { get; set; }
        public double? Discount { get; set; }
        public string? MoreInfo { get; set; }
        public bool IsDeleted { get; set; }
        public byte DetailStyle { get; set; }

        public virtual Order IdOrderNavigation { get; set; } = null!;
        public virtual HotelRoom IdRoomNavigation { get; set; } = null!;
        public virtual Tour IdTourNavigation { get; set; } = null!;
    }
}
