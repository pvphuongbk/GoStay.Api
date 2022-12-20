using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class OrderStatus
    {
        public byte Id { get; set; }
        public string? Status { get; set; }
    }
}
