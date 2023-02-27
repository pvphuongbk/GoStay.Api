using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoStay.Data.Base;
using GoStay.DataDto.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Hotel;
using GoStay.Services.WebSupport;
using PartnerGostay.Models;
using GoStay.Services;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController : ControllerBase
    {

        private readonly IHotelService _hotelServices;
        private readonly IMapper _mapper;
        private readonly IMyTypedClientServices _client;
        public HotelController(IHotelService hotelServices, IMapper mapper, IMyTypedClientServices client)
        {
            _hotelServices = hotelServices;
            _mapper = mapper;
            _client = client;
        }

        [HttpPost("list")]
        public ResponseBase GetHotelList(RequestGetListHotel request)
        {
            var items = _hotelServices.GetHotelList(request);
            return items;
        }
        [HttpPost("add-room")]
        
        public ResponseBase AddRoom([FromForm]AddRoomViewModel roomDto)
        {
            ResponseBase response = new ResponseBase();
            response.Message = "";
            var hotelRoom = _mapper.Map<AddRoomViewModel, HotelRoom>(roomDto);
            var viewroom = roomDto.ViewRoom;
            var serviceroom = roomDto.ServicesRooms;
            var namealbum = roomDto.NameAlbum;

            hotelRoom.NewPrice = hotelRoom.PriceValue * (100 - (decimal)hotelRoom.Discount) / 100;

            var resultAddroom = _hotelServices.AddRoom(hotelRoom);
            response.Message = response.Message+ resultAddroom.Message;

            if (viewroom != null)
            {
                var resultAddview = _hotelServices.AddViewRoom((int)resultAddroom.Data, viewroom);
                response.Message = response.Message + " & " + resultAddview.Message;
            }
            if (serviceroom != null)
            {
                var resultAddservice = _hotelServices.AddServiceRoom((int)resultAddroom.Data, serviceroom);
                response.Message = response.Message + " & " + resultAddservice.Message;
            }
            if (namealbum != null)
            {
                var resultAddalbum = _hotelServices.AddAlbumRoom((int)resultAddroom.Data, namealbum);
                response.Message = response.Message + " & " + resultAddalbum.Message;

                if (roomDto.filesAlbum != null)
                {
                    var imgres = _client.PostImgAndGetData(roomDto.filesAlbum, 1024, (int)resultAddroom.Data, roomDto.UserId, 1);
                    var resultAddPicAlbum = _hotelServices.AddPictureRoom((int)resultAddroom.Data, (int)resultAddalbum.Data, 1, imgres);
                   
                    response.Message = response.Message + " & " + resultAddPicAlbum.Message;
                }
            }
            if (roomDto.filesRoom != null)
            {
                var imgres = _client.PostImgAndGetData(roomDto.filesRoom, 1024, (int)resultAddroom.Data, roomDto.UserId, 1);

                var resultAddPicRoom = _hotelServices.AddPictureRoom((int)resultAddroom.Data, 0, 1,imgres);
                response.Message = response.Message + " & " + resultAddPicRoom.Message;
            }
            return response;
        }
        [HttpGet("support-add-room")]
        public ResponseBase SupportAddRoom()
        {
            var result = _hotelServices.SupportAddRoom();
            return result;
        }

    }
}
