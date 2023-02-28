using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoStay.Data.Base;
using GoStay.DataDto.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Hotel;
using GoStay.Services.WebSupport;
using PartnerGostay.Models;
using GoStay.Services;
using Newtonsoft.Json;

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
        [HttpGet("list-by-user")]
        public ResponseBase GetHotelListByUser(int IdUser)
        {
            var items = _hotelServices.GetHotelListByUser(IdUser);
            return items;
        }

        [HttpPost("list-room-by-user")]
        public ResponseBase GetRoomList(RequestGetListRoom request)
        {
            var items = _hotelServices.GetRoomList(request);
            return items;
        }
        [HttpPost("add-room")]
        
        public ResponseBase AddRoomPartner(List<IFormFile> fileRooms, string bodyJson)
        {
            ResponseBase response = new ResponseBase();

            try
            {
                var roomDto = JsonConvert.DeserializeObject<AddRoomModel>(bodyJson);

                response.Message = "";
                var hotelRoom = _mapper.Map<AddRoomModel, HotelRoom>(roomDto);
                var viewroom = roomDto.ViewRoom;
                var serviceroom = roomDto.ServicesRooms;

                hotelRoom.NewPrice = hotelRoom.PriceValue * (100 - (decimal)hotelRoom.Discount) / 100;

                var resultAddroom = _hotelServices.AddRoom(hotelRoom);
                response.Message = response.Message + resultAddroom.Message;

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
                if (fileRooms != null)
                {
                    var imgres = _client.PostImgAndGetData(fileRooms, 1024, (int)resultAddroom.Data, roomDto.Iduser, 1);
                    var date = DateTime.Today.ToString("dd/MM/yyyy hh:mm");
                    var album = _hotelServices.AddAlbumRoom((int)resultAddroom.Data, $"Album{(int)resultAddroom.Data}{date}");
                    var resultAddPicRoom = _hotelServices.AddPictureRoom((int)resultAddroom.Data, (int)album.Data, 1, roomDto.Iduser, imgres);
                    response.Message = response.Message + " & " + resultAddPicRoom.Message;
                }
                response.Data = "True";
                response.Code = ErrorCodeMessage.Success.Key;
                return response;
            }
            catch
            {
                response.Data = "False";
                response.Code = ErrorCodeMessage.Exception.Key;
                return response;
            }
        }

        [HttpPost("edit-room")]

        public ResponseBase EditRoomPartner(List<IFormFile> fileRooms, string bodyJson)
        {
            ResponseBase response = new ResponseBase();

            try
            {
                var roomDto = JsonConvert.DeserializeObject<AddRoomModel>(bodyJson);

                response.Message = "";
                var hotelRoom = _mapper.Map<AddRoomModel, HotelRoom>(roomDto);
                var viewroom = roomDto.ViewRoom;
                var serviceroom = roomDto.ServicesRooms;

                hotelRoom.NewPrice = hotelRoom.PriceValue * (100 - (decimal)hotelRoom.Discount) / 100;

                var resultAddroom = _hotelServices.EditRoom(hotelRoom, viewroom, serviceroom);
                response.Message = response.Message + resultAddroom.Message;

                if (fileRooms != null)
                {
                    var imgres = _client.PostImgAndGetData(fileRooms, 1024, hotelRoom.Id, (int)hotelRoom.Iduser, 1);
                    var date = DateTime.Today.ToString("dd/MM/yyyy hh:mm");
                    var album = _hotelServices.AddAlbumRoom(hotelRoom.Id, $"Album{hotelRoom.Id}{date}");
                    var resultAddPicRoom = _hotelServices.AddPictureRoom(hotelRoom.Id, (int)album.Data, 1, (int)hotelRoom.Iduser, imgres);
                    response.Message = response.Message + " & " + resultAddPicRoom.Message;
                }
                response.Data = "True";
                response.Code = ErrorCodeMessage.Success.Key;
                return response;
            }
            catch
            {
                response.Data = "False";
                response.Code = ErrorCodeMessage.Exception.Key;
                return response;
            }
        }
        [HttpGet("support-add-room")]
        public ResponseBase SupportAddRoom()
        {
            var result = _hotelServices.SupportAddRoom();
            return result;
        }

    }
}
