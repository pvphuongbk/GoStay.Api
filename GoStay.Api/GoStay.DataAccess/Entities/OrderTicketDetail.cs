using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderTicketDetail
    {
        public OrderTicketDetail()
        {
            TicketPassengers = new HashSet<TicketPassenger>();
        }

        public int Id { get; set; }
        public int IdOrder { get; set; }
        public DateTime DateCreate { get; set; }
        public decimal Price { get; set; }
        public double? Discount { get; set; }
        public string StartPoint { get; set; } = null!;
        public string EndPoint { get; set; } = null!;
        public DateTime DepartureDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AirlineCode { get; set; } = null!;
        public string AirlineName { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public int Duration { get; set; }
        public string? Barrage { get; set; }
        public string? Class { get; set; }
        public double ServiceFee { get; set; }
        public double IssueFee { get; set; }

        public virtual OrderTicket IdOrderNavigation { get; set; } = null!;
        public virtual ICollection<TicketPassenger> TicketPassengers { get; set; }
    }
}
