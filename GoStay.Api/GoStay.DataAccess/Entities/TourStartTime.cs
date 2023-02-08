using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourStartTime
    {
        public TourStartTime()
        {
            Tours = new HashSet<Tour>();
        }

        public int Id { get; set; }
        public string? StartDate { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
