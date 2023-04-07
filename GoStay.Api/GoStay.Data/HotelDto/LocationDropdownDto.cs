using GoStay.Data.Enums;

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
        public int? TinhThanhID { get; set; }
        public string? HotelTypeName { get; set; }
        public string? Slug { get;set;}
        public int? NumRecord { get; set; }
    }
}
