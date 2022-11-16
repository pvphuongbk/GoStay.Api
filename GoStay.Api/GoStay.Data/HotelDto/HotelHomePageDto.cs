﻿namespace GoStay.Data.HotelDto
{
	public class HotelHomePageDto
	{
		public int Id { get; set; }
		public string HotelName { get; set; }
		public string HotelAddress { get; set; }
		public int? Rating { get; set; }
		public double Review_score { get; set; }
		public double? Lat_map { get; set; }
		public double? Lon_map { get; set; }
		public double? Discount { get; set; }
		public decimal? OriginalPrice { get; set; }
		public decimal? ActualPrice { get; set; }
	}
}
