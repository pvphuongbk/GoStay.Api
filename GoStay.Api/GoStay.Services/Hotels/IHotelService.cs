using GoStay.Data.HotelDto;

namespace GoStay.Services.Hotels
{
	public interface IHotelService
	{
		List<HotelHomePageDto> GetListHotelForHomePage();
		List<RoomByHotelDto> GetListRoomByHotel(int hotelId);
		List<HotelHomePageDto> GetListHotelForSearching(HotelSearchRequest filter);
	}
}
