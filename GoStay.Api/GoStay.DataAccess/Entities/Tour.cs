﻿using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tour
    {
        public Tour()
        {
            TourDetails = new HashSet<TourDetail>();
        }

        public int Id { get; set; }
        public string? TourName { get; set; }
        public byte? IdTourStyle { get; set; }
        public byte? IdTourTopic { get; set; }
        public int? IdUser { get; set; }
        public string? Descriptions { get; set; }
        public int? InDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? IdProvinceFrom { get; set; }
        public int? IdProvinceTo { get; set; }
        public double Price { get; set; }
        public byte? Discount { get; set; }
        public double? CachedRating { get; set; }
        public string? Content { get; set; }
        public int? TourSize { get; set; }
        public string? Locations { get; set; }
        public int? Style { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual TourStyle? IdTourStyleNavigation { get; set; }
        public virtual TourTopic? IdTourTopicNavigation { get; set; }
        public virtual ICollection<TourDetail> TourDetails { get; set; }
    }
}
