using AutoMapper;
using GoStay.Common.Extention;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.Extensions.DependencyInjection;

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
					TinhThanh = hotel.IdTinhThanhNavigation.TenTt,
					QuanHuyen = hotel.IdQuanNavigation.Tenquan,
					HotelName = hotel.Name!,
					Lat_map = hotel.LatMap,
					Lon_map = hotel.LonMap,
					Rating = hotel.Rating,
					Review_score = (hotel.ReviewScore == null || hotel.NumberReviewers == null) ? -1 :
									(double)(hotel.ReviewScore / hotel.NumberReviewers),
					Pictures = hotel.Pictures.Where(x => !string.IsNullOrEmpty(x.Url)).Select(x => x.Url).ToList(),


					NumberReviewers = hotel.NumberReviewers
                };

				var room = hotel.HotelRooms.Where(x => x.Discount != null).MaxBy(x =>x.Discount);
				if (room != null)
				{
					dto.Discount = room.Discount;
					dto.OriginalPrice = (decimal)room.PriceValue;
					var actualPrice = CalculateRoomPrice(room);
					dto.ActualPrice = actualPrice;
					dto.PalletbedRoom = room.Palletbed;

                }
				if (hotel.HotelRooms.Count>0)
				{
					hotelDtos.Add(dto);
				}
			}

			return hotelDtos;
		}

        public static List<HotelHomePageDto> CreateHotelFlashSaleDto(List<Hotel> hotels)
        {
            List<HotelHomePageDto> hotelDtos = new List<HotelHomePageDto>();
            foreach (var hotel in hotels)
            {
                var dto = new HotelHomePageDto
                {
                    Id = hotel.Id,
                    TinhThanh = hotel.IdTinhThanhNavigation.TenTt,
                    QuanHuyen = hotel.IdQuanNavigation.Tenquan,
                    HotelName = hotel.Name!,
                    Lat_map = hotel.LatMap,
                    Lon_map = hotel.LonMap,
                    Rating = hotel.Rating,
                    Review_score = (hotel.ReviewScore == null ) ? -1 :
                                    (double)(hotel.ReviewScore),
                    Pictures = hotel.Pictures.Where(x => !string.IsNullOrEmpty(x.Url)).Select(x => x.Url).ToList(),

					Slug= hotel.Name.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower(),
					NumberReviewers = hotel.NumberReviewers
                };

                var room = hotel.HotelRooms.Where(x => x.Discount != null).MaxBy(x => x.Discount);
                if (room != null)
                {
                    dto.Discount = room.Discount;
                    dto.OriginalPrice = (decimal)room.PriceValue;
                    dto.ActualPrice = (decimal)room.NewPrice;
                    dto.PalletbedRoom = room.Palletbed;

                }

                hotelDtos.Add(dto);
            }

            return hotelDtos;
        }

    }
}
