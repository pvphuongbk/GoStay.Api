﻿using GoStay.Data.Base;
using GoStay.Data.HotelDto;

namespace GoStay.Services.Hotels
{
	public interface IHotelService
	{
		ResponseBase GetListHotelForHomePage();
		ResponseBase GetListRoomByHotel(int hotelId);
		ResponseBase GetListHotelForSearching(HotelSearchRequest filter);
		ResponseBase GetListForSearchHotel(HotelSearchingRequest filter);
		public ResponseBase GetListHotelForHotelPage();
		public ResponseBase GetHotelDetail(int hotelId);
		public ResponseBase GetPagingListForSearchHotel(HotelSearchingPaging filter); 
        public ResponseBase GetPagingListHotelForSearching(HotelSearchingPaging filter);
		public ResponseBase GetListTopHotelForHomePage(int number);

    }
}
