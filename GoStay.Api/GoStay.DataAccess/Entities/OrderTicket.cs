﻿using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderTicket
    {
        public OrderTicket()
        {
            OrderTicketDetails = new HashSet<OrderTicketDetail>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public int IdUser { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public byte Status { get; set; }
        public byte IdPtthanhToan { get; set; }
        public string? Ordercode { get; set; }
        public string? Session { get; set; }
        public string DataFlightSession { get; set; } = null!;
        public string FlightSession { get; set; } = null!;

        public virtual OrderPhuongThucTt IdPtthanhToanNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual OrderStatus StatusNavigation { get; set; } = null!;
        public virtual ICollection<OrderTicketDetail> OrderTicketDetails { get; set; }
    }
}
