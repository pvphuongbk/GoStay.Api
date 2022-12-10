using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourDetailsStyle
    {
        public TourDetailsStyle()
        {
            TourDetails = new HashSet<TourDetail>();
        }

        public byte Id { get; set; }
        public string? Style { get; set; }

        public virtual ICollection<TourDetail> TourDetails { get; set; }
    }
}
