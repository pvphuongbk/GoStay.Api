using GoStay.Common.Enums;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.Statistical;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Statistical;
using GoStay.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GoStay.Services.Statisticals
{
   
    public class SchedulerRoomPriceService : ISchedulerRoomPriceService
    {
        private readonly ICommonRepository<SchedulerRoomPrice> _schedulerRepository;
        private readonly ICommonRepository<HotelRoom> _roomRepository;
        private readonly ICommonUoW _commonUoW;

        public SchedulerRoomPriceService(ICommonRepository<SchedulerRoomPrice> schedulerRepository, ICommonRepository<HotelRoom> roomRepository, ICommonUoW commonUoW)
        {
            _schedulerRepository = schedulerRepository;
            _roomRepository = roomRepository;
            _commonUoW = commonUoW;
        }
        public ResponseBase Insert(SchedulerRoomPrice scheduler)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var check = _schedulerRepository.FindAll(x => x.RoomId == scheduler.RoomId &&
                                                            x.Start == scheduler.Start &&
                                                            x.End == scheduler.End &&
                                                            x.RecurrenceRule == scheduler.RecurrenceRule).Count();
                if(check>0)
                {
                    responseBase = Update(scheduler);
                    return responseBase;
                }    
                _schedulerRepository.Insert(scheduler);
                _commonUoW.Commit();
                responseBase.Data = scheduler.PriceId;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase Update(SchedulerRoomPrice scheduler)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                _schedulerRepository.Update(scheduler);
                _commonUoW.Commit();
                responseBase.Data = scheduler.PriceId;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase Destroy( int Id)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                _schedulerRepository.Remove(Id);
                _commonUoW.Commit();
                responseBase.Data = "success";
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetScheduler(int Id)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                var scheduler = _schedulerRepository.GetById(Id);
                _commonUoW.Commit();
                responseBase.Data = scheduler;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetListScheduler(int month, int year, int RoomId)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                var listscheduler = _schedulerRepository.FindAll(x => x.RoomId== RoomId).ToList();
                _commonUoW.Commit();
                responseBase.Data = listscheduler;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
    }
}
