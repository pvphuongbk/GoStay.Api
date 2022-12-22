using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public DateTime? ChechIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public byte? Num { get; set; }
        public DateTime? DateCreate { get; set; }
        public decimal? Price { get; set; }
        public double? Discount { get; set; }
        public string? MoreInfo { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Order IdOrderNavigation { get; set; } = null!;
        public virtual Tour IdProduct1 { get; set; } = null!;
        public virtual HotelRoom IdProductNavigation { get; set; } = null!;
    }
}
