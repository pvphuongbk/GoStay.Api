using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourTopic
    {
        public TourTopic()
        {
            Tours = new HashSet<Tour>();
        }

        public byte Id { get; set; }
        public string? TourTopic1 { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
