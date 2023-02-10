using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelRating
    {
        public int Id { get; set; }
        public int IdHotel { get; set; }
        public decimal LocationScore { get; set; }
        public decimal ValueScore { get; set; }
        public decimal ServiceScore { get; set; }
        public decimal CleanlinessScore { get; set; }
        public decimal RoomsScore { get; set; }
        public string? Description { get; set; }
        public int IdUser { get; set; }
        public DateTime? DateReviews { get; set; }
        public DateTime? DateUpdate { get; set; }

        public virtual Hotel IdHotelNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
