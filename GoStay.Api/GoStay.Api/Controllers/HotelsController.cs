using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Services.Hotels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpGet("hotel-top-flash-sale")]
        public ResponseBase GetListHotelTopFlashSale(int number)
        {
            var items = _hotelService.GetListHotelTopFlashSale(number);
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

        [HttpGet("hotel-homepage")]
        public ResponseBase GetListHotelHomePage(int IdProvince)
        {
            var items = _hotelService.GetListHotelHomePage(IdProvince);
            return items;
        }
        [HttpPost("hotel-search")]
		public ResponseBase GetListHotelHomePage(HotelSearchRequest filter)
		{
            var items = _hotelService.GetListForSearchHotel(filter);
			return items;
		}

        [HttpGet("hotel-suggest")]
        public ResponseBase GetListSuggestHotel(string search)
        {
            var items = _hotelService.GetListSuggestHotel(search);
            return items;
        }

        [HttpGet("hotel-near")]
        public ResponseBase GetListNearHotel(float Lat, float Lon)
        {
            var items = _hotelService.GetListNearHotel(Lat, Lon);
            return items;
        }

        [HttpGet("type-hotel")]
        public ResponseBase GetAllTypeHotel()
        {
            var items = _hotelService.GetAllTypeHotel();
            return items;
        }
        [HttpGet("services-search")]
        public ResponseBase GetServicesSearch(int type)
        {
            var items = _hotelService.GetServicesSearch(type);
            return items;
        }
    }
}