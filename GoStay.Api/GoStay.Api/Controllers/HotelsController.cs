using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Services.Hotels;
using Microsoft.AspNetCore.Mvc;

namespace GoStay.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HotelsController : ControllerBase
	{
		private readonly IHotelService _hotelService;
		public HotelsController(IHotelService hotelService)
		{
			_hotelService = hotelService;
		}

		[HttpGet("hotel-home-page")]
		public ResponseBase<List<HotelHomePageDto>> GetListHotelForHomePage()
		{
			var items = _hotelService.GetListHotelForHomePage();

			return items;
		}

		[HttpGet("room-by-hotel/{id}")]
		public ResponseBase<List<RoomByHotelDto>> GetListRoomByHotel(int id)
		{
			var items = _hotelService.GetListRoomByHotel(id);

			return items;
		}

		[HttpPost("hotel-searching")]
		public ResponseBase<List<HotelHomePageDto>> GetListHotelForSearching(HotelSearchRequest filter)
		{
			var items = _hotelService.GetListHotelForSearching(filter);

			return items;
		}
	}
}