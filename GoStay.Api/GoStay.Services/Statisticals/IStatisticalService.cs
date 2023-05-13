using GoStay.Common.Enums;
using GoStay.Data.Base;
using GoStay.Data.Statistical;
using GoStay.DataDto.Statistical;

namespace GoStay.Services.Statisticals
{
    public interface IStatisticalService
    {
        ResponseBase GetValueChart();
        ResponseBase GetRoomInMonthByDay(int month, int year);
        Task<ResponseBase> GetAllOrderPriceByUser(PriceDetailByUserRequest request);
        Task<ResponseBase> GetPriceChartByUser(PriceChartType type, int userID, int year, int month);
        Task<ResponseBase> GetAllOrderByUser(int userID, int pageIndex, int pageSize,int style);
    }
}
