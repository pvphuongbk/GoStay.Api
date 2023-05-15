using GoStay.Data.Enums;
using System.Xml.Linq;

namespace GoStay.Data.HotelDto
{
    public class LocationDropdownDto
    {
        public string Value { get; set; }
        public string TinhThanh { get; set; }
        public string TenQuan { get; set; }
        public string URL { get; set; }
        public int HotelType { get; set; }
        public LocationDropdown Type { get; set; }
        public int Id { get; set; }
        public int TinhThanhID { get; set; }
        public int? QuanID { get; set; }
        public string HotelTypeName { get; set; }
        public int NumRecord { get; set; }
        public decimal NewPrice { get; set; }
        public float HowFar { get; set; }
        public string Address { get; set; }
        public string PriceRange { get; set; }
        public string? Slug
        {
            get
            ;set;
        }
        public int HotelRoomID { get; set; }
        public float? Discount { get; set; }
        public float? Lat { get; set; }
        public float? Lon { get; set; }
        public int? Rating { get; set; }

    }
}
