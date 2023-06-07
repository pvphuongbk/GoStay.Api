using System;
using System.Collections;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class SchedulerRoomPrice
    {
        public int PriceId { get; set; }

        public string Title { get; set; }

        public double Price { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int RoomId { get; set; }
        public string? RecurrenceRule { get; set; } = null!;
        public bool? IsAllDay { get; set; }
        public string? Description { get; set; }
        public string? RecurrenceException { get; set; }
        public int? Attendees { get; set; }
        public int? RecurrenceId { get; set; }

        public string? EndTimezone { get; set; }
        public string? StartTimezone { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
