using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.TourDto
{
    public class TourOrderDto
    {
        public int Id { get; set; }
        public string? TourName { get; set; }
        public byte IdTourStyle { get; set; }
        public string TourStyle { get; set; }
        public byte IdTourTopic { get; set; }
        public string TourTopic { get; set; }
        public int? IdUser { get; set; }
        public string UserName { get; set; }
        public string? Descriptions { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public int IdDistrictFrom { get; set; }
        public string ProvinceFrom { get; set; }
        public double Price { get; set; }
        public double? PriceChild { get; set; }

        public byte? Discount { get; set; }
        public double ActualPrice { get; set; }
        public double Rating { get; set; }
        public string? Content { get; set; }
        public int TourSize { get; set; }
        public string? Locations { get; set; }
        public int Style { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<TourDetailDto> TourDetails { get; set; }
        public List<string> ProvinceTo { get; set; }
        public List<string> Pictures { get; set; } = new List<string>();

    }
    public class TourDetailDto
    {
        public int Id { get; set; }
        public int IdTours { get; set; }
        public byte? IdStyle { get; set; }
        public string? Title { get; set; }
        public string? Details { get; set; }
        public byte? Stt { get; set; }

    }
}
