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
		public ResponseBase GetListHotelForHomePage()
		{
			var items = _hotelService.GetListHotelForHomePage();

			return items;
		}

        [HttpGet("hotel-top-flash-sale")]
        public ResponseBase GetListHotelTopFlashSale(int number)
        {
            var items = _hotelService.GetListHotelTopFlashSale(number);

            return items;
        }

        [HttpGet("hotel-page")]
        public ResponseBase GetListHotelForHotelPage()
        {
            var items = _hotelService.GetListHotelForHotelPage();

            return items;
        }
        [HttpGet("hotel-detail")]
        public ResponseBase GetHotelDetail(string hotelId)
        {
            var items = _hotelService.GetHotelDetail(int.Parse(hotelId) );

            return items;
        }

        [HttpGet("room-by-hotel/{id}")]
		public ResponseBase GetListRoomByHotel(int id)
		{

			var items = _hotelService.GetListRoomByHotel(id);

			return items;
		}

		[HttpPost("hotel-searching")]
		public ResponseBase GetListHotelForSearching(HotelSearchingRequest filter)
		{
			var items = _hotelService.GetListHotelForSearching(filter);

			return items;
		}


        [HttpPost("hotel-search")]
		public ResponseBase GetListForSearchHotel(HotelSearchRequest filter)
		{
			var items = _hotelService.GetListForSearchHotel(filter);

			return items;
		}

        [HttpGet("location-dropdown/{search}")]
        public ResponseBase GetPagingListForSearchHotel(string search)
        {
            var items = _hotelService.GetListLocationForDropdown(search);

            return items;
        }
        [HttpPost("search-home-page")]
        public ResponseBase GetListHotelForHomePageNew(SeachHomePageDto search)
        {
            var items = _hotelService.GetListHotelForHomePageNew(search);

            return items;
        }
    }
}