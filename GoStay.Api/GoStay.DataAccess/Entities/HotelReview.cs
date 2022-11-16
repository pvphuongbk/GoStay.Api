using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelReview
    {
        public int Id { get; set; }
        public int? Idhotel { get; set; }
        public int? Iduser { get; set; }
        public string? Title { get; set; }
        public string? Review { get; set; }
        public string? Tipgood { get; set; }
        public int? Idtrip { get; set; }
        public DateTime? Datego { get; set; }
        public DateTime? Datepost { get; set; }
        public decimal? Service { get; set; }
        public decimal? SleepQuality { get; set; }
        public decimal? Location { get; set; }
        public decimal? Swimmingpool { get; set; }
        public decimal? Value { get; set; }
        public decimal? Cleanliness { get; set; }
        public decimal? Rooms { get; set; }
        public decimal? Fitnessfacility { get; set; }
        public byte? Intmode { get; set; }

        public virtual Hotel? IdhotelNavigation { get; set; }
    }
}
