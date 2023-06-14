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
        public int? IdHotel { get; set; }
        public string? TourNameId
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    IdTour = 0;
                    TourName = "";
                }
                else
                {
                    IdTour = int.Parse(value.Split("$")[1]);
                    TourName = value.Split("$")[0];
                }
            }
        }

        public int? IdTour { get; set; }
        public string? TourName { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? Status { get; set; }
        public string? HotelName { get; set; }

        public double? TotalPrice { get; set; }
        public decimal? TotalAmount { get; set; }

        public string? PaymentMethod { get; set; }
        public string? RoomIdsNames
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    RoomIdName = new List<string>();
                }
                else
                {
                    RoomIdName = value.Split(';').ToList();

                }
            }
        }
        public List<string>? RoomIdName
        {
            set
            {
                if (value==null)
                {
                    ListRoomNames = new List<string>();
                    ListRoomIds = new List<string>();
                }
                else
                {
                    foreach (var item in value)
                    {
                        ListRoomNames.Add(item.Split('$')[1]);
                        ListRoomIds.Add(item.Split('$')[0]);
                    }
                }
            }
        }

        public List<string> ListRoomNames { get; set; } = new List<string>();
        public List<string> ListRoomIds { get; set; } = new List<string>();

        public string? Slug { get; set; }
        public int? Style 
        { 
            get 
            {
                if (IdTour == 115)
                    return 1;
                else
                    return 2;
            } 
        }
        public int Total { get; set; }

    }
}
