
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoStay.Data.HotelDto
{
    public class HotelDetailDto
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public int? Rating { get; set; }
        public decimal? AvgNight { get; set; }
        public double Review_score { get; set; }
        public decimal? ServiceScore { get; set; }
        public decimal? ValueScore { get; set; }
        public decimal? SleepQualityScore { get; set; }
        public decimal? CleanlinessScore { get; set; }
        public decimal? LocationScore { get; set; }
        public decimal? RoomsScore { get; set; }
        public string? Content { get; set; }
        public string? TinhThanh { get; set; }
        public string? QuanHuyen { get; set; }
        public string? TinhThanh_url { get; set; }
        public string? QuanHuyen_url { get; set; }
        public double? Lat_map { get; set; }
        public double? Lon_map { get; set; }
        public double? Discount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public int? NumberReviewers { get; set; }
        public List<HotelRoomDto> Rooms { get; set; }
        public List<ServiceDetailHotelDto> Services { get; set; }
        public List<string> Pictures { get; set; } = new List<string>();
        [JsonIgnore]
        public double? ReviewScore { get; set; }
        public int TotalPicture { get; set; }
        public string Slug { get; set; }
        public int Ordered { get; set; }
    }
}
