using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;

namespace GoStay.Services.Hotels
{
	public interface IHotelService
	{
        public ResponseBase GetListHotelTopFlashSale(int number);
        public ResponseBase GetListRoomByHotel(int hotelId);
        public ResponseBase GetListForSearchHotel(HotelSearchRequest filter);
        public ResponseBase GetListSuggestHotel(string searchText);
        public ResponseBase GetHotelDetail(int hotelId);
        public ResponseBase GetListNearHotel(int NumTop,float Lat, float Lon);

        public ResponseBase GetAllTypeHotel();
        ResponseBase GetServicesSearch(int type);
        public ResponseBase GetListHotelHomePage(int IdProvince);
    }
}
