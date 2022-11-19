﻿using GoStay.Data.HotelDto;
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

		public static List<HotelHomePageDto> CreateHotelHomePageDto(List<Hotel> hotels)
		{
			List<HotelHomePageDto> hotelDtos = new List<HotelHomePageDto>();
			foreach (var hotel in hotels)
			{
				var dto = new HotelHomePageDto
				{
					Id = hotel.Id,
					HotelAddress = hotel.Address!,
					HotelName = hotel.Name!,
					Lat_map = hotel.LatMap,
					Lon_map = hotel.LonMap,
					Rating = hotel.Rating,
					Review_score = (hotel.ReviewScore == null || hotel.NumberReviewers == null) ? -1 :
									(double)(hotel.ReviewScore / hotel.NumberReviewers),
					Pictures = hotel.Pictures.Where(x => !string.IsNullOrEmpty(x.Url)).Select(x => x.Url).ToList()
				};

				var room = hotel.HotelRooms.Where(x => x.PriceValue != null).MinBy(x => CommonFunction.CalculateRoomPrice(x));
				if (room != null)
				{
					dto.Discount = room.Discount;
					dto.OriginalPrice = room.PriceValue;
					var actualPrice = CalculateRoomPrice(room);
					dto.ActualPrice = actualPrice;
				}

				hotelDtos.Add(dto);
			}

			return hotelDtos;
		}
	}
}
