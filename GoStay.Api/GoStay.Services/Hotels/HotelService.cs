using AutoMapper;
using GoStay.Common.Helpers;
using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

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
		public ResponseBase GetListHotelForHomePage()
		{
			ResponseBase responseBase = new ResponseBase();
			try
			{
				var now = DateTime.Now;
				var hotels = _hotelRepository.FindAll(x => x.Deleted != 1)
												.Include(x => x.Pictures.Skip(5).Take(5))
												.Include(x => x.HotelRooms.Where(x => (x.CheckInDate > now || x.CheckInDate == null)))
												//|| (x.CheckOutDate < now || x.CheckInDate == null)))
												.Where(x => x.HotelRooms.Any(x => (x.CheckInDate > now || x.CheckInDate == null)))
												//|| (x.CheckOutDate < now || x.CheckInDate == null)))
											 .ToList();
				var hotelDtos = CommonFunction.CreateHotelHomePageDto(hotels);
				responseBase.Data = hotelDtos;
				return responseBase;
			}
			catch (Exception e)
			{
				responseBase.Code = ErrorCodeMessage.Exception.Key;
				responseBase.Message = e.Message;
				return responseBase;
			}
		}

		public ResponseBase GetListRoomByHotel(int hotelId)
		{
			ResponseBase responseBase = new ResponseBase();
			try
			{
				var rooms = _hotelRoomRepository.FindAll(x => x.Deleted != 1 && x.Idhotel == hotelId)
													.Include(x => x.Pictures.Skip(5).Take(5));
				var items = _mapper.Map<List<RoomByHotelDto>>(rooms.ToList());
				foreach (var room in rooms)
				{
					var item = items.Where(x => x.Id == room.Id).FirstOrDefault();
					if (item != null)
						item.Pictures = room.Pictures.Where(x => !string.IsNullOrEmpty(x.Url)).Select(x => x.Url).ToList();
				}
				responseBase.Data = items;
				return responseBase;
			}
			catch (Exception e)
			{
				responseBase.Code = ErrorCodeMessage.Exception.Key;
				responseBase.Message = e.Message;
				return responseBase;
			}
		}

		public ResponseBase GetListHotelForSearching(HotelSearchRequest filter)
		{
			ResponseBase responseBase = new ResponseBase();
			try
			{
				var now = DateTime.Now;
				var inDate = filter.CheckinDate == null ? now : (DateTime)filter.CheckinDate;
				var outDate = filter.CheckOutDate == null ? now : (DateTime)filter.CheckOutDate;
				var hotels = _hotelRepository.FindAll(x => x.Deleted != 1 && !string.IsNullOrEmpty(x.Address) && x.Address.ToLower().Replace(" ", "").Contains(filter.AddressSearch.ToLower().Replace(" ", "")))
												.Include(x => x.HotelRooms.Where(x => (x.NumChild == filter.NumChild && x.NumMature == filter.NumMature)
												&& ((x.CheckInDate > now || x.CheckInDate == null)
												|| (x.CheckOutDate < now || x.CheckInDate == null))))
												.Include(x => x.Pictures.Skip(5).Take(5))
												.Where(x => x.HotelRooms.Any(x => (x.NumChild == filter.NumChild && x.NumMature == filter.NumMature)
												&& ((x.CheckInDate > now || x.CheckInDate == null)
												|| (x.CheckOutDate < now || x.CheckInDate == null))))
											 .ToList();
				var hotelDtos = CommonFunction.CreateHotelHomePageDto(hotels);
				responseBase.Data = hotelDtos;
				return responseBase;
			}
			catch (Exception e)
			{
				responseBase.Code = ErrorCodeMessage.Exception.Key;
				responseBase.Message = e.Message;
				return responseBase;
			}
		}

		public ResponseBase GetListForSearchHotel(HotelSearchingRequest filter)
		{
			ResponseBase responseBase = new ResponseBase();
			try
			{
				var hotelQueryables = _hotelRepository.FindAll(x => x.Deleted != 1)
												.Include(x => x.HotelMamenitis)
												.Include(x => x.HotelRooms)
												.Include(x => x.Pictures.Skip(5).Take(5))
												.Where(x => x.HotelRooms.Any())
												.AsQueryable();
				if (filter.Ratings != null && filter.Ratings.Count > 0)
					hotelQueryables = hotelQueryables.Where(x => filter.Ratings.Contains(x.Rating));
				if (filter.IdQuans != null && filter.IdQuans.Count > 0)
					hotelQueryables = hotelQueryables.Where(x => filter.IdQuans.Contains(x.IdQuan));
				if (filter.IdPhuong != null && filter.IdPhuong.Count > 0)
					hotelQueryables = hotelQueryables.Where(x => filter.IdPhuong.Contains(x.IdPhuong));
				if (filter.Types != null && filter.Types.Count > 0)
					hotelQueryables = hotelQueryables.Where(x => filter.Types.Contains(x.Type));
				if (filter.ReviewScore != null)
					hotelQueryables = hotelQueryables.Where(x => x.ReviewScore >= filter.ReviewScore);
				if (filter.Services != null && filter.Services.Count > 0)
					hotelQueryables = hotelQueryables.Where(x => x.HotelMamenitis.Select(y => y.Idservices).Any(z => filter.Services.Contains(z)));
				var hotels = hotelQueryables.ToList();
				if (filter.Price != null)
					hotels = hotels.Where(x => x.HotelRooms.Any(x => CommonFunction.CalculateRoomPrice(x) <= filter.Price)).ToList();
				var hotelDtos = CommonFunction.CreateHotelHomePageDto(hotels);
				responseBase.Data = hotelDtos;
				return responseBase;
			}
			catch (Exception e)
			{
				responseBase.Code = ErrorCodeMessage.Exception.Key;
				responseBase.Message = e.Message;
				return responseBase;
			}
		}
	}
}
