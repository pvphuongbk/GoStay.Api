using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.OrderDto
{
    public class OrderSearchDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string OrderCode { get; set; }
        public int Status { get; set; }
        public int Style { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class OrderSearchParam
    {
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? OrderCode { get; set; }
        public int? Status { get; set; }
        public int? Style { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class OrderSearchOutDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string? Ordercode { get; set; }
        public int? IdUser { get; set; }

        public int? IdTour { get; set; }
        public string? TourName { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? Status { get; set; }
        public int? Style { get; set; }
        public int? Total { get; set; }
        public string? RoomName { get; set; }
        public int? IdRoom { get; set; }
        public string? HotelName { get; set; }
        public int? IdHotel { get; set; }


        public string? PaymentMethod { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? Adult { get; set; }
        public int? Children { get; set; }
        public int? Infant { get; set; }
        public int? NumNight { get; set; }
        public int? NumRoom { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public string? Slug { get; set; }


    }
}
