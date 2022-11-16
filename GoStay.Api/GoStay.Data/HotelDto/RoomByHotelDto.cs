namespace GoStay.Data.HotelDto
{
	public class RoomByHotelDto
	{
		public int? Idhotel { get; set; }
		public string? Name { get; set; }
		public decimal? RoomSize { get; set; }
		public string? Description { get; set; }
		public int? Status { get; set; }
		public decimal? PriceValue { get; set; }
		public double? Discount { get; set; }
		public byte? NumMature { get; set; }
		public byte? NumChild { get; set; }
		public byte? Palletbed { get; set; }
		public int? ViewDirection { get; set; }
		public DateTime? CheckInDate { get; set; }
		public DateTime? CheckOutDate { get; set; }
	}
}
