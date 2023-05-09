using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TicketPassenger
    {
        public string? FullName { get; set; }
        public string? Type { get; set; }
        public int Id { get; set; }
        public bool? Gender { get; set; }
        public string? Birthday { get; set; }
        public string? PassportExpiryDate { get; set; }
        public string? PassportIssueCountry { get; set; }
        public string? PassportNumber { get; set; }
        public int IdTicket { get; set; }
        public decimal Price { get; set; }

        public virtual OrderTicketDetail IdTicketNavigation { get; set; } = null!;
    }
}
