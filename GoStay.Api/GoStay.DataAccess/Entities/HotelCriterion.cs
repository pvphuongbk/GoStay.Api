using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelCriterion
    {
        public HotelCriterion()
        {
            HotelRatings = new HashSet<HotelRating>();
        }

        public int Id { get; set; }
        public string? Criteria { get; set; }

        public virtual ICollection<HotelRating> HotelRatings { get; set; }
    }
}
