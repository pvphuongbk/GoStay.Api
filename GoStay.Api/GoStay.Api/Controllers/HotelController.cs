using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoStay.Services;
using GoStay.Data.Base;
using GoStay.DataDto.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Hotel;
using Newtonsoft.Json;
using GoStay.Services.WebSupport;

namespace GoStay.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class HotelController : Controller
    {

        private readonly IHotelServices _hotelServices;
        private readonly IMapper _mapper;

        public HotelController(IHotelServices hotelServices, IMapper mapper)
        {
            _hotelServices = hotelServices;

            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("list")]
        public ResponseBase GetHotelList(RequestGetListHotel request)
        {
            var items = _hotelServices.GetHotelList(request);
            return items;
        }
        [HttpPost("add-room")]
        public ResponseBase AddRoom(string roomDto)
        {
            var room = JsonConvert.DeserializeObject<RoomAddDto>(roomDto);

            var hotelRoom = _mapper.Map<RoomAddDto, HotelRoom>(room);

            if (hotelRoom.PriceValue is null)
            {
                hotelRoom.PriceValue = 0;
            }
            if (hotelRoom.Discount == null)
            {
                hotelRoom.Discount = 0;
            }
            if (hotelRoom.Palletbed == null)
            {
                hotelRoom.Palletbed = 1;
            }
            hotelRoom.NewPrice = room.PriceValue * (100 - (decimal)hotelRoom.Discount) / 100;

            var result = _hotelServices.AddRoom(hotelRoom, room.ViewRoom, room.ServiceRoom);
            return result;
        }
    }
}
