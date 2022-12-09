using GoStay.Data.Base;
using GoStay.Data.HotelDto;

namespace GoStay.Services.Hotels
{
	public interface IHotelService
	{
        public ResponseBase GetListHotelTopFlashSale(int number);
        public ResponseBase GetListRoomByHotel(int hotelId);
        public ResponseBase GetListForSearchHotel(HotelSearchRequest filter);
        public ResponseBase GetListSuggestHotel(string searchText);
        public ResponseBase GetHotelDetail(int hotelId);
    }
}
