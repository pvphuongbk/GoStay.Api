using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Hotel : BaseEntity
    {
        public Hotel()
        {
            HotelMamenitis = new HashSet<HotelMameniti>();
            HotelReviews = new HashSet<HotelReview>();
            HotelRooms = new HashSet<HotelRoom>();
        }


        public string? Name { get; set; }
        public string? NameSeo { get; set; }
        public string? CodeCountry { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public decimal? AvgNight { get; set; }
        public int? Calender { get; set; }
        public int? Rating { get; set; }
        public string? Content { get; set; }
        public int? Type { get; set; }
        public int? IdPriceRange { get; set; }
        public bool? Meals { get; set; }
        public int? ReviewScore { get; set; }
        public decimal? ServiceScore { get; set; }
        public decimal? ValueScore { get; set; }
        public decimal? SleepQualityScore { get; set; }
        public decimal? CleanlinessScore { get; set; }
        public decimal? LocationScore { get; set; }
        public decimal? RoomsScore { get; set; }
        public double? LatMap { get; set; }
        public double? LonMap { get; set; }

        public int? NumberReviewers { get; set; }

        public virtual PriceRange? IdPriceRangeNavigation { get; set; }
        public virtual TypeHotel? TypeNavigation { get; set; }
        public virtual ICollection<HotelMameniti> HotelMamenitis { get; set; }
        public virtual ICollection<HotelReview> HotelReviews { get; set; }
        public virtual ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
