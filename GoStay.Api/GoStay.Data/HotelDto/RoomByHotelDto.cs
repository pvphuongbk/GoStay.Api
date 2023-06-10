namespace GoStay.Data.HotelDto
{
	public class RoomByHotelDto
	{
		public int Id { get; set; }
		public int? Idhotel { get; set; }
		public string? Name { get; set; }
		public decimal? RoomSize { get; set; }
		public string? Description { get; set; }
		public int? Status { get; set; }
		public int? Deleted { get; set; }
		public byte? NumMature { get; set; }
		public byte? NumChild { get; set; }
		public byte? Palletbed { get; set; }
		public int? ViewDirection { get; set; }
		public DateTime? CheckInDate { get; set; }
		public DateTime? CheckOutDate { get; set; }
		public long? IntDate { get; set; }
		public List<string?> Pictures { get; set; } = new List<string?>();
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
		public double DailyBasePrice { get
			{
				if (DailyPrice > 0)
					return DailyPrice;
				else
					return (double)PriceValue;

            }
		}
        public double DailyPrice { get; set; }
        public double CurrentPrice { get 
			{
				return (double)(DailyBasePrice * (100 - Discount) / 100);

            } 
		}


    }
}
