using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tour
    {
        public Tour()
        {
            TourDetails = new HashSet<TourDetail>();
            TourProvinceTos = new HashSet<TourProvinceTo>();
        }

        public int Id { get; set; }
        public string? TourName { get; set; }
        public byte IdTourStyle { get; set; }
        public byte IdTourTopic { get; set; }
        public int? IdUser { get; set; }
        public string? Descriptions { get; set; }
        public int? InDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IdProvinceFrom { get; set; }
        public double Price { get; set; }
        public byte? Discount { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }
        public int TourSize { get; set; }
        public string? Locations { get; set; }
        public int Style { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual TinhThanh IdProvinceFromNavigation { get; set; } = null!;
        public virtual TourStyle IdTourStyleNavigation { get; set; } = null!;
        public virtual TourTopic IdTourTopicNavigation { get; set; } = null!;
        public virtual ICollection<TourDetail> TourDetails { get; set; }
        public virtual ICollection<TourProvinceTo> TourProvinceTos { get; set; }
    }
}
