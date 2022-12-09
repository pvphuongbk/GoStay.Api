namespace GoStay.Data.HotelDto
{
    public class HotelSearchRequest
    {
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public List<int?>? Ratings { get; set; }
        public List<int>? IdQuans { get; set; }
        public List<int>? IdPhuong { get; set; }
        public List<int>? Types { get; set; }
        public double? ReviewScore { get; set; }
        public List<int>? Services { get; set; }
        public int? Palletbed { get; set; }
        public int? NumMature { get; set; }
        public int? NumChild { get; set; }
        public DateTime? CheckinDate { get; set; }
        public DateTime? CheckoutDate { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? IdTinhThanh { get; set; }

    }
}
