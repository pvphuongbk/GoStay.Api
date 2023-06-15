
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
        public string DeletePicture(int Id);

        public ResponseBase GetRoomList(RequestGetListRoom request);
        public ResponseBase GetHotelListByUser(int IdUser);
        public ResponseBase EditRoom(HotelRoom data, List<int> view, List<int> service, List<int> picture);
        public ResponseBase UpdateRoomStatus(UpdateStatusRoomParam param);
        public ResponseBase UpdateRoomDiscount(UpdateDiscountRoomParam param);
        ResponseBase GetPicturesRoom(int IdRoom);
        ResponseBase GetPicturesHotel(int IdHotel);
        ResponseBase GetServicesRoom(int IdRoom);
        ResponseBase SetMapHotel(SetMapHotelRequest param);
        public ResponseBase GetListRoomAdmin(RequestGetListRoomAdmin request);
        public ResponseBase ChangeRoomStatus(int IdRoom, int RoomStatus);
        public ResponseBase ChangeStatusRoom(int IdRoom, int RoomStatus);
        public ResponseBase MinimumNightRoom(int userId, int IdRoom, byte minNight);
        public ResponseBase DeadlinePreOrderRoom(int userId, int IdRoom, int numMonth);


    }

}
