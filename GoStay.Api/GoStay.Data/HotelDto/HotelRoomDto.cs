using GoStay.Data.ServiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoStay.Data.HotelDto
{
    public class HotelRoomDto
    {
        public int? Id { get; set; }
        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public int? RoomStatus { get; set; }

        public byte? RemainNum { get; set; }
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public decimal? NewPrice { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public string? PalletbedText { get; set; }
        public string? ViewRoom { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public List<string> Pictures { get; set; } = new List<string>();
        [JsonIgnore]
        public string StrPictures { get; set; }
        public List<ServiceDetailHotelDto> Services { get; set; }

    }
    public class RoomAdminDto
    {
        public int Id { get; set; }
        public decimal NewPrice { get; set; }
        public decimal Price_Value { get; set; }
        public float Discount { get; set; }
        public decimal RoomSize { get; set; }
        public string Name { get; set; }
        public byte NumMature { get; set; }
        public byte NumChild { get; set; }
        public string SearchKey { get; set; }
        public int RoomStatus { get; set; }
        public int IDUser { get; set; }
        public string HotelName { get; set; }
        public string UserName { get; set; }
        public string Urls { get; set; }
        public int Total { get; set; }


    }
}
