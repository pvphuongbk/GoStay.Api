using GoStay.Common.Enums;
using GoStay.Data.Base;
using GoStay.Data.Statistical;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Statistical;

namespace GoStay.Services.Statisticals
{
    public interface ISchedulerRoomPriceService
    {
        public ResponseBase Insert(SchedulerRoomPrice scheduler);
        public ResponseBase Update(SchedulerRoomPrice scheduler);
        public ResponseBase GetScheduler(int Id);
        public ResponseBase GetListScheduler( int RoomId);
        public ResponseBase Destroy(int Id);
        public ResponseBase GetPrice(int month, int year, int RoomId, int day);
        public ResponseBase GetListRoomPrice(int month, int year, List<int> RoomIds, int day);
        public ResponseBase UpdateDailyPriceForAllRoom();
        public ResponseBase GetRoomPriceInFuture(DateTime futuretime, int RoomId);

    }
}
