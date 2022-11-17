using AutoMapper;
using GoStay.Common.Helpers;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace GoStay.Services.Hotels
{
	public class HotelService : IHotelService
	{
		private readonly ICommonRepository<Hotel> _hotelRepository;
		private readonly ICommonRepository<HotelRoom> _hotelRoomRepository;
		private readonly IMapper _mapper;
		public HotelService(ICommonRepository<Hotel> hotelRepository, ICommonRepository<HotelRoom> hotelRoomRepository, IMapper mapper)
		{
			_hotelRepository = hotelRepository;
			_hotelRoomRepository = hotelRoomRepository;
			_mapper = mapper;
		}
		public List<HotelHomePageDto> GetListHotelForHomePage()
		{
			var now = DateTime.Now;
			var hotels = _hotelRepository.FindAll(x => x.Deleted != 1)
											.Include(x => x.HotelRooms.Where(x => (x.CheckInDate > now || x.CheckInDate == null) 
											|| (x.CheckOutDate < now || x.CheckInDate == null)))
											.Where(x => x.HotelRooms.Any(x => (x.CheckInDate > now || x.CheckInDate == null)
											|| (x.CheckOutDate < now || x.CheckInDate == null)))
											.Include(x => x.Pictures.Skip(5).Take(5))
									     .ToList();
			List<HotelHomePageDto> hotelDtos = new List<HotelHomePageDto>();
			foreach(var hotel in hotels)
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
				if(room != null)
				{
					dto.Discount = room.Discount;
					dto.OriginalPrice = room.PriceValue;
					dto.ActualPrice = CommonFunction.CalculateRoomPrice(room);
				}

				hotelDtos.Add(dto);
			}
			return hotelDtos;
		}

		public List<RoomByHotelDto> GetListRoomByHotel(int hotelId)
		{
			var rooms = _hotelRoomRepository.FindAll(x => x.Deleted != 1 && x.Idhotel == hotelId)
												.Include(x => x.Pictures.Skip(5).Take(5));
			var items = _mapper.Map<List<RoomByHotelDto>>(rooms.ToList());
			foreach(var room in rooms)
			{
				var item = items.Where(x => x.Id == room.Id).FirstOrDefault();
				if(item != null)
					item.Pictures = room.Pictures.Where(x => !string.IsNullOrEmpty(x.Url)).Select(x => x.Url).ToList();
			}

			return items;
		}

		public List<HotelHomePageDto> GetListHotelForSearching(HotelSearchRequest filter)
		{
			var now = DateTime.Now;
			var inDate = filter.CheckinDate == null ? now : (DateTime)filter.CheckinDate;
			var outDate = filter.CheckOutDate == null ? now : (DateTime)filter.CheckOutDate;
			var hotels = _hotelRepository.FindAll(x => x.Deleted != 1 && !string.IsNullOrEmpty(x.Address) && x.Address.ToLower().Replace(" ","").Contains(filter.AddressSearch.ToLower().Replace(" ", "")))
											.Include(x => x.HotelRooms.Where(x => (x.NumChild == filter.NumChild && x.NumMature == filter.NumMature) 
											&& ((x.CheckInDate > now || x.CheckInDate == null)
											|| (x.CheckOutDate < now || x.CheckInDate == null))))
											.Where(x => x.HotelRooms.Any(x => (x.NumChild == filter.NumChild && x.NumMature == filter.NumMature)
											&& ((x.CheckInDate > now || x.CheckInDate == null)
											|| (x.CheckOutDate < now || x.CheckInDate == null))))
											.Include(x => x.Pictures.Skip(5).Take(5))
										 .ToList();
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
					dto.ActualPrice = CommonFunction.CalculateRoomPrice(room);
				}

				hotelDtos.Add(dto);
			}
			return hotelDtos;
		}
	}
}
