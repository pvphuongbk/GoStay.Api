using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tour
    {
        public Tour()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Orders = new HashSet<Order>();
            Pictures = new HashSet<Picture>();
            TourDetails = new HashSet<TourDetail>();
            TourDistrictTos = new HashSet<TourDistrictTo>();
            TourVehicles = new HashSet<TourVehicle>();
        }

        public int Id { get; set; }
        public string? TourName { get; set; }
        public byte IdTourStyle { get; set; }
        public byte IdTourTopic { get; set; }
        public int? IdUser { get; set; }
        public string? Descriptions { get; set; }
        public int? InDate { get; set; }
        public DateTime StartDate { get; set; }
        public int? IdStartTime { get; set; }
        public int IdDistrictFrom { get; set; }
        public double Price { get; set; }
        public byte? Discount { get; set; }
        public int? Rating { get; set; }
        public string? Content { get; set; }
        public int TourSize { get; set; }
        public string? Locations { get; set; }
        public int Style { get; set; }
        public DateTime CreatedDate { get; set; }
        public double ActualPrice { get; set; }
        public byte Status { get; set; }
        public int Deleted { get; set; }
        public double? PriceChild { get; set; }
        public int? NumTour { get; set; }
        public int? Songuoidadat { get; set; }
        public string? SearchKey { get; set; }

        public virtual Quan IdDistrictFromNavigation { get; set; } = null!;
        public virtual TourStartTime? IdStartTimeNavigation { get; set; }
        public virtual TourStyle IdTourStyleNavigation { get; set; } = null!;
        public virtual TourTopic IdTourTopicNavigation { get; set; } = null!;
        public virtual TourRating? RatingNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<TourDetail> TourDetails { get; set; }
        public virtual ICollection<TourDistrictTo> TourDistrictTos { get; set; }
        public virtual ICollection<TourVehicle> TourVehicles { get; set; }
    }
}
