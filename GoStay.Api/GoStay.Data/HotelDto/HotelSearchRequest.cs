namespace GoStay.Data.HotelDto
{
	public class HotelSearchingRequest
	{
		public string AddressSearch { get; set; }
		public DateTime? CheckinDate { get; set; }
		public DateTime? CheckOutDate { get; set; }
		public int NumMature { get; set; }
		public int NumChild { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
