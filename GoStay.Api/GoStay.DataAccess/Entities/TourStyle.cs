using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourStyle
    {
        public TourStyle()
        {
            Tours = new HashSet<Tour>();
        }

        public byte Id { get; set; }
        public string? TourStyle1 { get; set; }
        public string? TourStyleEng { get; set; }
        public string? TourStyleChi { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
