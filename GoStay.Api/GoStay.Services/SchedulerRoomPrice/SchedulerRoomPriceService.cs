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

        public ResponseBase GetPrice(int month, int year, int RoomId, int day)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                double result = 0;
                _commonUoW.BeginTransaction();
                var t1 = new DateTime(year, month, day);
                var t2 = t1.DayOfWeek.ToString().Substring(0,1);
                var scheduler = _schedulerRepository.FindAll(x => x.RoomId == RoomId && x.Start.Year >= year&& x.Start.Month>= month);
                foreach(var item in scheduler)
                {
                    List<DateTime> tException = new List<DateTime>();

                    if (item.RecurrenceException != null)
                    {
                        var listEx = item.RecurrenceException.Split(",");
                        foreach (var ex in listEx)
                        {
                            var yEx = int.Parse(ex.Substring(0, 3));
                            var mEx = int.Parse(ex.Substring(4, 5));
                            var dEx = int.Parse(ex.Substring(6, 7));
                            var tEx = new DateTime(yEx, mEx, dEx);
                            tException.Add(tEx);
                        }
                    }
                    foreach(var tEx in tException)
                    {
                        if(t1==tEx)
                        {

                        }    
                    }    

                    if (item.RecurrenceRule !=null)
                    {
                        var RecurrenceRule = GetRecurrenceRule(item.RecurrenceRule);
                        var freq = "";
                        var byday = "";
                        var count = "";
                        var interval = "";
                        var until = "";



                        if (RecurrenceRule.TryGetValue("UNTIL", out until))
                        {
                            var y = int.Parse(until.Substring(0, 3));
                            var m = int.Parse(until.Substring(4, 5));
                            var d = int.Parse(until.Substring(6, 7));
                            var tUntil = new DateTime(y, m, d);
                            if (t1 <= tUntil)
                            {
                            }
                        }

                        if (RecurrenceRule.TryGetValue("BYDAY", out byday))
                        {
                            if (byday.Contains(t2))
                            {

                            }
                        }





                        //if (RecurrenceRule.TryGetValue("FREQ", out freq))
                        //{
                        //    if (value.Contains(t2))
                        //    {

                        //    }
                        //}

                        //if (RecurrenceRule.TryGetValue("COUNT", out count))
                        //{
                        //    if (value.Contains(t2))
                        //    {

                        //    }
                        //}

                        //if (RecurrenceRule.TryGetValue("INTERVAL", out interval))
                        //{
                        //    if (value.Contains(t2))
                        //    {

                        //    }
                        //}


                    }
                    else
                    {
                        if( t1 > item.Start && t1<item.End)
                        {
                            result = (double)item.Price;
                        }    
                    }    
                }    
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
        public Dictionary<string,string> GetRecurrenceRule(string Recurrence)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var t1 = Recurrence.Split(";");
            foreach(var item in t1)
            {
                var temp = item.Split("=");
                var key = temp[0];
                var value = temp[1];
                result.Add(key, value);
            }
            return result;
        }
        public int GetNearestDay(DateTime date, int dayofweek)
        {
            var t2 = ((int)date.DayOfWeek);
            if(dayofweek> t2)
            {
                return dayofweek - t2;
            }
            else
            {
                return t2- dayofweek;
            }    
        }
    }
}
