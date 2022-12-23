using GoStay.Data.HotelDto;
using GoStay.Data.ServiceDto;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.OrderDto
{
    public class OrderGetInfoDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string? Status { get; set; }
        public string PaymentMethod{ get; set; }
        public string? MoreInfo { get; set; }
        public string? Session { get; set; }
        public bool IsDeleted { get; set; }
        public List<OrderDetailInfoDto> OrderDetails { get; set; }

    }

    public class OrderDetailInfoDto
    {
        public int Id { get; set; }
        public string DetailStyle { get; set; }
        public int? IdProduct { get; set; }
        public DateTime? ChechIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public byte? Num { get; set; }
        public decimal? Price { get; set; }
        public double? Discount { get; set; }
        public decimal NewPrice { get; set; }
        public string? MoreInfo { get; set; }

        public List<HotelRoomOrderDto> Rooms { get; set; }
        public List<TourOrderDto> Tours { get; set; }
    }

    public class HotelRoomOrderDto
    {
        public int? Id { get; set; }
        public int? Idhotel { get; set; }
        public string HotelName { get; set; }
        public string? Address { get; set; }
        public int? Rating { get; set; }
        public int? ReviewScore { get; set; }
        public int? NumberReviewers { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string? RoomName { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public decimal? RoomSize { get; set; }
        public string ViewDirection { get; set; }
        public byte? RemainNum { get; set; }
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public decimal? NewPrice { get; set; }

        public List<string> Pictures { get; set; } = new List<string>();
        public List<ServiceDetailHotelDto> Services { get; set; }

    }
}
