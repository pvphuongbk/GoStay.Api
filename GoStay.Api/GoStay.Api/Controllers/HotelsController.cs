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

        [HttpGet("hotel-top-home-page")]
        public ResponseBase GetListTopHotelForHomePage(int number)
        {
            var items = _hotelService.GetListTopHotelForHomePage(number);

            return items;
        }

        [HttpGet("hotel-page-paging")]
        public ResponseBase GetListHotelForHotelPage(HotelSearchingPaging filter)
        {
            var items = _hotelService.GetListHotelForHotelPage();

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
		public ResponseBase GetListHotelForSearching(HotelSearchRequest filter)
		{
			var items = _hotelService.GetListHotelForSearching(filter);

			return items;
		}

        [HttpPost("hotel-searching-paging")]
        public ResponseBase GetPagingListHotelForSearching(HotelSearchingPaging filter)
        {
            var items = _hotelService.GetPagingListHotelForSearching(filter);

            return items;
        }

        [HttpPost("hotel-search")]
		public ResponseBase GetListForSearchHotel(HotelSearchingRequest filter)
		{
			var items = _hotelService.GetListForSearchHotel(filter);

			return items;
		}

        [HttpPost("hotel-search-paging")]
        public ResponseBase GetPagingListForSearchHotel(HotelSearchingPaging filter)
        {
            var items = _hotelService.GetPagingListForSearchHotel(filter);

            return items;
        }
    }
}