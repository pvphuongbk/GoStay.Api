using GoStay.Common.Enums;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.Statistical;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Scheduler;
using GoStay.DataDto.Statistical;
using GoStay.Repository.DapperHelper;
using GoStay.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
        public ResponseBase GetListScheduler(int RoomId)
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

        public ResponseBase GetPrice(int month, int year, int RoomId, int day)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                double data = 0;
                var scheduler = _schedulerRepository.FindAll(x => x.RoomId == RoomId && x.Start.Year <= year && x.Start.Month <= month);
                if(scheduler.Count()>0)
                {
                    data = SchedulerRepository.GetPrice(scheduler, month, year, day);
                }    
                
                responseBase.Data = data;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase GetListRoomPrice(int month, int year, List<int> RoomIds, int day)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                Dictionary<int, double> ListRoomPrice = new Dictionary<int, double>();
                foreach (var idroom in RoomIds)
                {
                    var price = GetPrice(month, year, idroom, day);
                    var p = price.Data;
                    ListRoomPrice.Add(idroom, (double)p);
                }
                responseBase.Data = ListRoomPrice;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase UpdateDailyPriceForAllRoom()
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {

                var roomIds = _schedulerRepository.FindAll(x => x.Start.Year <= DateTime.Now.Year && x.Start.Month <= DateTime.Now.Month).Select(x => x.RoomId);
                roomIds= roomIds.Distinct();
                var schedulers = _schedulerRepository.FindAll(x => roomIds.Contains(x.RoomId));
                //Dictionary<int,double> roomprices = new Dictionary<int, double>();

                List<SchedulerRoomPriceDto> lst = new List<SchedulerRoomPriceDto>();
                foreach (var idroom in roomIds)
                {
                    var scheduler = schedulers.Where(x => x.RoomId == idroom);
                    var price = SchedulerRepository.GetPrice(scheduler, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Day);
                    if (price > 0)
                    {
                        lst.Add(new SchedulerRoomPriceDto { RoomId = idroom, Price = price });
                    }
                }
                HotelDapperExtensions.ScheduleRoomPrice(lst);
                responseBase.Data = lst;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Message = e.Message;
                return responseBase;

            }
        }
    }
}
