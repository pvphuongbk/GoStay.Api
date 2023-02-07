using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourRating
    {
        public TourRating()
        {
            Tours = new HashSet<Tour>();
        }

        public int Id { get; set; }
        public string? Rating { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
