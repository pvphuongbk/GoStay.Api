using GoStay.Data.Base;
using GoStay.Data.Statistical;

namespace GoStay.Services.Statisticals
{
    public interface IStatisticalService
    {
        ResponseBase GetValueChart();
        ResponseBase GetRoomInMonthByDay(int month, int year);
    }
}
