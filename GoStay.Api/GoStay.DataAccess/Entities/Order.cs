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
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public byte? Status { get; set; }
        public byte IdPtthanhToan { get; set; }
        public string? MoreInfo { get; set; }
        public string? Session { get; set; }

        public virtual OrderPhuongThucTt IdPtthanhToanNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual OrderStatus? StatusNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
