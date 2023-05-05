using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelDto
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? Type { get; set; }
        public string? TypeHotel { get; set; }

        public int? IdPriceRange { get; set; }
        public string? PriceRange { get; set; }
        public int RoomCount { get; set; }
    }
    public class RequestGetListHotel
    {
        public int UserId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? IdProvince { get; set; }
        public string? NameSearch { get; set; }
    }
    public class RequestGetListRoom
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? IdHotel { get; set; }
        public int? IdUser { get; set; }
        public int? IdRoom { get; set; }
        public string? NameSearch { get; set; }
    }
    public class RequestGetListRoomAdmin
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? IdHotel { get; set; }
        public int? IdUser { get; set; }
        public int? RoomStatus { get; set; }
        public string? RoomName { get; set; }
        public string? HotelName { get; set; }
    }
}
