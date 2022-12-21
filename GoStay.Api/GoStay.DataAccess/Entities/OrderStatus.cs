using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public byte Id { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
