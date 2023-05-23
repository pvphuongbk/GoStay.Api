using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Hotel : BaseEntity
    {
        public Hotel()
        {
            Albums = new HashSet<Album>();
            HotelMamenitis = new HashSet<HotelMameniti>();
            HotelRatings = new HashSet<HotelRating>();
            HotelReviews = new HashSet<HotelReview>();
            HotelRooms = new HashSet<HotelRoom>();
            NearbyHotels = new HashSet<NearbyHotel>();
            Pictures = new HashSet<Picture>();
        }


        public string Name { get; set; } = null!;
        public string? NameSeo { get; set; }
        public string? CodeCountry { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public decimal? AvgNight { get; set; }
        public int? Calender { get; set; }
        public bool? Criterion { get; set; }

        public int? Rating { get; set; }
        public string? Content { get; set; }
        public int? Type { get; set; }
        public int? IdPriceRange { get; set; }
        public bool? Meals { get; set; }
        public decimal? ServiceScore { get; set; }
        public decimal? ValueScore { get; set; }
        public decimal? SleepQualityScore { get; set; }
        public decimal? CleanlinessScore { get; set; }
        public decimal? LocationScore { get; set; }
        public decimal? RoomsScore { get; set; }
        public double? LatMap { get; set; }
        public double? LonMap { get; set; }

        public int? NumberReviewers { get; set; }
        public long? IntDate { get; set; }
        public int IdPhuong { get; set; }
        public int IdQuan { get; set; }
        public int IdTinhThanh { get; set; }
        public int? NumViews { get; set; }
        public string? SearchKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal? ReviewScore { get; set; }
        public DateTime? LastOrderTime { get; set; }

        public virtual Phuong IdPhuongNavigation { get; set; } = null!;
        public virtual PriceRange? IdPriceRangeNavigation { get; set; }
        public virtual Quan IdQuanNavigation { get; set; } = null!;
        public virtual TinhThanh IdTinhThanhNavigation { get; set; } = null!;
        public virtual TypeHotel? TypeNavigation { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<HotelMameniti> HotelMamenitis { get; set; }
        public virtual ICollection<HotelRating> HotelRatings { get; set; }
        public virtual ICollection<HotelReview> HotelReviews { get; set; }
        public virtual ICollection<HotelRoom> HotelRooms { get; set; }
        public virtual ICollection<NearbyHotel> NearbyHotels { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
