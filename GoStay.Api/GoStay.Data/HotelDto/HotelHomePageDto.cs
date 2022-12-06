namespace GoStay.Data.HotelDto
{
	public class HotelHomePageDto
	{
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string TinhThanh { get; set; }
        public string QuanHuyen { get; set; }
        public int? Rating { get; set; }
        public decimal? AvgNight { get; set; }
        public double Review_score { get; set; }
        public double? Lat_map { get; set; }
        public double? Lon_map { get; set; }
        public double? Discount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public int? NumberReviewers { get; set; }
        public int? PalletbedRoom { get; set; }
        public int Total { get; set; }
        private string Urls 
        {
            set
            {
				if (string.IsNullOrEmpty(value))
					Pictures = new List<string>();
				else
				{
					Pictures = value.Split(';').ToList();

				}
			}
        }
        public List<string> Pictures { get; set; } = new List<string>();
	}

}
