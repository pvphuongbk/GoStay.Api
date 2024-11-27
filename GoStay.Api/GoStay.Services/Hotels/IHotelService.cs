using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.HotelFlashSales;

namespace GoStay.Services.Hotels
{
	public interface IHotelService
	{
        Task<ResponseBase> UpsertHotelTopFlashSale(List<HotelFlashSaleUpsertRequestModel> requestModel);
        ResponseBase UpsertHotelFlashSale(HotelFlashSaleUpsertRequestModel requestModel);

        public ResponseBase GetHotelFlashSalePresentData();
        public ResponseBase GetHotelFlashSaleSelectionData(int pageIndex, int pageSize, string? keyword = "");
        public ResponseBase GetListHotelTopFlashSale(int number);
        public ResponseBase GetListRoomByHotel(int hotelId);
        public ResponseBase GetListForSearchHotel(HotelSearchRequest filter);
        public ResponseBase GetListSuggestHotel(string searchText);
        public ResponseBase GetHotelDetail(int hotelId);
        public ResponseBase GetListNearHotel(int NumTop,float Lat, float Lon);
        public ResponseBase GetHotelDetailNew(int hotelId, int userId);
        public ResponseBase GetAllTypeHotel();
        ResponseBase GetServicesSearch(int type);
        public ResponseBase GetListHotelHomePage(int IdProvince);
    }
}
