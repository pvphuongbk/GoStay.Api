using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class SchedulerRoomPrice
    {
        public int Id { get; set; }
        public double Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public string RecurrenceRule { get; set; } = null!;
        public bool? IsAllDay { get; set; }
        public string? Description { get; set; }
        public string? RecurrenceException { get; set; }
        public string? Attendees { get; set; }
        public int? RecurrenceId { get; set; }
    }
}
