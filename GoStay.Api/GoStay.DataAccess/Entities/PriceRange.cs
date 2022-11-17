using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class PriceRange
    {
        public PriceRange()
        {
            Hotels = new HashSet<Hotel>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public string? TitleVnd { get; set; }
        public byte? Stt { get; set; }
        public int? Deleted { get; set; }

        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
