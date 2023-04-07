using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.TourDto
{
    public class TourContentDto
    {
        public int Id { get; set; }
        public string? TourName { get; set; }

        public byte IdTourStyle { get; set; }
        public string TourStyle { get; set; }
        public byte IdTourTopic { get; set; }
        public string TourTopic { get; set; }

        public int? IdUser { get; set; }
        public string? Descriptions { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }

        public double Price { get; set; }
        public byte? Discount { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }
        public int TourSize { get; set; }
        public string? Locations { get; set; }
        public int Style { get; set; }
        public DateTime CreatedDate { get; set; }
        public double ActualPrice { get; set; }
        public byte Status { get; set; }

        public List<TourDetailDto> TourDetails { get; set; }

        public int IdDistrictFrom { get; set; }
        public string DistrictFrom { get; set; }
        public List<int> IdDistrictTo { get; set; }
        public List<string> DistrictTo { get; set; }

        public List<string> Pictures { get; set; }
        public string Slug { get; set; }
    }
}
