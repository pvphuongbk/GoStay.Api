namespace GoStay.Data.HotelDto
{
	public class HotelSearchingRequest
	{
		public decimal? Price { get; set; }
		public List<int?>? Ratings { get; set; }
		public List<int>? IdQuans { get; set; }
		public List<int>? IdPhuong { get; set; }
		public List<int?>? Types { get; set; }
		public double? ReviewScore { get; set; }
		public List<int>? Services { get; set; }
	}
    public class HotelSearchingPaging
    {
        public decimal? Price { get; set; }
        public List<int?>? Ratings { get; set; }
        public List<int>? IdQuans { get; set; }
        public List<int>? IdPhuong { get; set; }
        public List<int?>? Types { get; set; }
        public double? ReviewScore { get; set; }
        public List<int>? Services { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
