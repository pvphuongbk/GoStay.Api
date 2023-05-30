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
        public ResponseBase GetListScheduler(int month, int year, int RoomId);


    }
}
