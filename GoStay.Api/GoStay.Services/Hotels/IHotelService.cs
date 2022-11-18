using GoStay.Data.Base;
using GoStay.Data.HotelDto;

namespace GoStay.Services.Hotels
{
	public interface IHotelService
	{
		ResponseBase<List<HotelHomePageDto>> GetListHotelForHomePage();
		ResponseBase<List<RoomByHotelDto>> GetListRoomByHotel(int hotelId);
		ResponseBase<List<HotelHomePageDto>> GetListHotelForSearching(HotelSearchRequest filter);
	}
}
