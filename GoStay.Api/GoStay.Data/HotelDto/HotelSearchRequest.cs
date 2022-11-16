namespace GoStay.Data.HotelDto
{
	public class HotelSearchRequest
	{
		public string AddressSearch { get; set; }
		public DateTime? CheckinDate { get; set; }
		public DateTime? CheckOutDate { get; set; }
		public int NumMature { get; set; }
		public int NumChild { get; set; }
	}
}
