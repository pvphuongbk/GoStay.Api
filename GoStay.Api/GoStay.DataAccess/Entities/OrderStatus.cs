using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            OrderTickets = new HashSet<OrderTicket>();
            Orders = new HashSet<Order>();
        }

        public byte Id { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<OrderTicket> OrderTickets { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
