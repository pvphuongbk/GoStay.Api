using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class PriceRange : BaseEntity
    {
        public PriceRange()
        {
            Hotels = new HashSet<Hotel>();
        }
        public string? Title { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public string? TitleVnd { get; set; }
        public byte? Stt { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
