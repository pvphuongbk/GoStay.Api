
using GoStay.Common;
using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.HotelDto;

namespace GoStay.Services.WebSupport
{
    public interface IHotelServices
    {
        public ResponseBase GetHotelList(RequestGetListHotel request);
        public ResponseBase AddRoom(HotelRoom data, int[] view, int[] service);

    }

}
