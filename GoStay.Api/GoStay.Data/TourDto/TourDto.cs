using GoStay.DataAccess.Entities;
using GoStay.DataDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.TourDto
{
    public class TourAdminDto
    {
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
        public int[] IdDistrictTo { get; set; }
        public byte[] Vehicles { get; set; }

    }
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
    public class DataSupportTour
    {
        public List<TourStyleDto> Styles { get; set; }
        public List<VehicleDto> Vehicles { get; set; }
        public List<TourStartTimeDto> StartTimes { get; set; }
        public List<TourTopicDto> Topics { get; set; }
        public List<QuanDto> Districts { get; set; }
        public List<ProvinceDto> Provinces { get; set; }
        public List<TourRatingDto> Ratings { get; set; }
    }
    public class TourStyleDto
    {
        public byte Id { get; set; }
        public string? TourStyle1 { get; set; }
    }
    public class VehicleDto
    {
        public byte Id { get; set; }
        public string? Name { get; set; }
    }
    public class TourStartTimeDto
    {
        public int Id { get; set; }
        public string? StartDate { get; set; }
    }
    public class TourTopicDto
    {
        public byte Id { get; set; }
        public string? TourTopic1 { get; set; }
    }
    public class ProvinceDto
    {
        public int Id { get; set; }
        public string? TenTt { get; set; }
        public int? IdCountry { get; set; }
    }
    public class TourRatingDto
    {
        public int Id { get; set; }
        public string? Rating { get; set; }
    }
    public class TourAddDto
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }

        public string? TourName { get; set; }
        public byte IdTourStyle { get; set; }
        public byte IdTourTopic { get; set; }
        public string StartDateString { get; set; }
        public int IdDistrictFrom { get; set; }
        public int[] IdDistrictTo { get; set; }
        public double Price { get; set; }
        public byte? Discount { get; set; }
        public string? Content { get; set; }
        public int TourSize { get; set; }
        public string? Locations { get; set; }
        public int Style { get; set; }
        public double ActualPrice { get; set; }
        public int[]? Vehicle { get; set; }
        public int? Rating { get; set; }
        public string? Descriptions { get; set; }
        public int? IdStartTime { get; set; }

    }
    public class TourAddParam
    {
        public TourDto tourAddDto { get; set; }
        public int[] IdDistrictTo { get; set; }
        public int[] Vehicles { get; set; }
    }
    public class CompareTourParam
    {
        public string IdTour { get; set; }
        public int IdUser { get; set; }
        public string Session { get; set; }
    }
    public  class TourDto
    {
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

    }
}
