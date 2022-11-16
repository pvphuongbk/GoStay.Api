using GoStay.DataAccess.Entities;

namespace GoStay.Common.Helpers
{
	public class CommonFunction
	{
		public static decimal CalculateRoomPrice(HotelRoom hotelRoom)
		{
			if (hotelRoom.PriceValue != null && hotelRoom.Discount == null)
				return (decimal)hotelRoom.PriceValue;
			if (hotelRoom.PriceValue == null || hotelRoom.Discount == null)
				return 0;
			var price = (decimal)hotelRoom.PriceValue - ((decimal)hotelRoom.PriceValue * (decimal)hotelRoom.Discount)/100;
			return price;
		}
	}
}
