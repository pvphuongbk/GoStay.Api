
using GoStay.Common;
using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Hotel;
using GoStay.DataDto.HotelDto;
using Microsoft.AspNetCore.Http;

namespace GoStay.Services.WebSupport
{
    public interface IHotelService
    {
        public ResponseBase GetHotelList(RequestGetListHotel request);
        public ResponseBase AddRoom(HotelRoom data);
        public ResponseBase SupportAddRoom();
        public ResponseBase AddViewRoom(int IdRoom, List<int> idViews);
        public ResponseBase AddServiceRoom(int IdRoom, List<int> idServices);
        public ResponseBase AddAlbumRoom(int IdRoom, string albumName);
        public ResponseBase AddPictureRoom(int Obj, int IdAlbum, int type, int userId, UploadImagesResponse imagesResponse);
        public string AddNewPicture(Picture picture);
        public ResponseBase GetRoomList(RequestGetListRoom request);
        public ResponseBase GetHotelListByUser(int IdUser);
        public ResponseBase EditRoom(HotelRoom data, List<int> view, List<int> service, List<int> picture);
    }

}
