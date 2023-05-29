using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Pricepolicy
    {
        public int Id { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int RoomId { get; set; }
        public bool? IsAllDay { get; set; }
        public string? Description { get; set; }
        public string? RecurrenceRule { get; set; }
        public int? Attendee { get; set; }
    }
}
