using GoStay.Data.Base;
using GoStay.Data.HotelDto;

namespace GoStay.Services.Hotels
{
	public interface IHotelService
	{
        ResponseBase GetListHotelForHomePage();
        ResponseBase GetListHotelTopFlashSale(int number);
        ResponseBase GetListHotelForHotelPage();
        ResponseBase GetListRoomByHotel(int hotelId);
        ResponseBase GetListHotelForSearching(HotelSearchingRequest filter);
        ResponseBase GetListForSearchHotel(HotelSearchRequest filter);
        ResponseBase GetListLocationForDropdown(string searchText);
        ResponseBase GetListHotelForHomePageNew(SeachHomePageDto search);
        ResponseBase GetHotelDetail(int hotelId);
    }
}
