using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelRating
    {
        public int Id { get; set; }
        public int IdHotel { get; set; }
        public decimal Point { get; set; }
        public int IdCriteria { get; set; }
        public string? Description { get; set; }
        public int IdUser { get; set; }
        public DateTime? DateReviews { get; set; }
        public DateTime? DateUpdate { get; set; }

        public virtual HotelCriterion IdCriteriaNavigation { get; set; } = null!;
        public virtual Hotel IdHotelNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
