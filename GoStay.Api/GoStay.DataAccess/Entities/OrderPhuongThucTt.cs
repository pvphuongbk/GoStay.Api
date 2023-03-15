using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderPhuongThucTt
    {
        public OrderPhuongThucTt()
        {
            OrderTickets = new HashSet<OrderTicket>();
            Orders = new HashSet<Order>();
        }

        public byte Id { get; set; }
        public string? PhuongThuc { get; set; }

        public virtual ICollection<OrderTicket> OrderTickets { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
