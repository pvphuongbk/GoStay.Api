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
                Dictionary<DateTime,double> result = new Dictionary<DateTime, double>();
                var t1 = new DateTime(year, month, day);
                var t2 = t1.DayOfWeek.ToString().Substring(0,2).ToUpper();
                var scheduler = _schedulerRepository.FindAll(x => x.RoomId == RoomId && x.Start.Year <= year&& x.Start.Month<= month);
                foreach(var item in scheduler)
                {
                    List<DateTime> tException = new List<DateTime>();
                    bool skip=false;
                    if (item.RecurrenceException != null)
                    {
                        var listEx = item.RecurrenceException.Split(",");
                        foreach (var ex in listEx)
                        {
                            var yEx = int.Parse(ex.Substring(0, 4));
                            var mEx = int.Parse(ex.Substring(4, 2));
                            var dEx = int.Parse(ex.Substring(6, 2));
                            var tEx = new DateTime(yEx, mEx, dEx);
                            tException.Add(tEx);
                        }
                    }
                    foreach(var tEx in tException)
                    {
                        if(t1==tEx)
                        {
                            skip=true;
                            break;
                        }    
                    }    
                    if(skip==true)
                        continue;

                    if (item.RecurrenceRule !=null)
                    {
                        var RecurrenceRule = GetRecurrenceRule(item.RecurrenceRule);
                        var freq = "";
                        var byday = "";
                        var bymonthday = "";
                        var count = "";
                        var interval = "";
                        var until = "";

                        if (RecurrenceRule.TryGetValue("UNTIL", out until))
                        {
                            var y = int.Parse(until.Substring(0, 4));
                            var m = int.Parse(until.Substring(4, 2));
                            var d = int.Parse(until.Substring(6, 2));
                            var tUntil = new DateTime(y, m, d);
                            if (t1 > tUntil)
                            {
                                continue;
                            }
                        }

                        if (RecurrenceRule.TryGetValue("BYDAY", out byday))
                        {
                            if (byday.Contains(t2))
                            {
                                var DayStart = GetNearestDay(item.Start, (int)t1.DayOfWeek);
                                int countF = 0;
                                int intervalF = 1;
                                if (RecurrenceRule.TryGetValue("COUNT", out count)) 
                                {
                                    countF = int.Parse(count);
                                } 
                                    

                                if (RecurrenceRule.TryGetValue("INTERVAL", out interval)) 
                                {
                                    intervalF = int.Parse(interval);
                                }
                                if (countF == 0)
                                {
                                    while (DayStart <= t1)
                                    {
                                        if (DayStart == t1)
                                        {
                                            result.Add(item.DateCreate, item.Price);
                                            break;
                                        }

                                        DayStart = DayStart.AddDays(intervalF * 7);
                                    }
                                    continue;
                                }
                                else
                                {
                                    int i = 1;
                                    while (DayStart <= t1 && i<=countF)
                                    {
                                        if (DayStart == t1)
                                        {
                                            result.Add(item.DateCreate, item.Price);
                                            break;
                                        }

                                        DayStart = DayStart.AddDays(intervalF * 7 );
                                        i++;
                                    }
                                    continue;

                                }

                            }
                            else
                            {
                                continue;
                            }    
                        }
                        if (RecurrenceRule.TryGetValue("BYMONTHDAY", out bymonthday))
                        {
                            if (bymonthday.Contains(t1.Day.ToString()))
                            {
                                var MonthStart = item.Start.Month;
                                int countF = 0;
                                int intervalF = 1;
                                if (RecurrenceRule.TryGetValue("COUNT", out count))
                                {
                                    countF = int.Parse(count);
                                }

                                if (RecurrenceRule.TryGetValue("INTERVAL", out interval))
                                {
                                    intervalF = int.Parse(interval);
                                }
                                if (countF == 0)
                                {
                                    while (MonthStart <= t1.Month)
                                    {
                                        if (MonthStart == t1.Month)
                                        {
                                            result.Add(item.DateCreate, item.Price);
                                            continue;
                                        }

                                        MonthStart = MonthStart + intervalF;
                                    }
                                }
                                else
                                {
                                    int i = 1;
                                    while (MonthStart <= t1.Month && i <= countF)
                                    {
                                        if (MonthStart == t1.Month)
                                        {
                                            result.Add(item.DateCreate, item.Price);
                                            continue;
                                        }

                                        MonthStart = MonthStart + intervalF;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if( t1 >= item.Start && t1<=item.End)
                        {
                            result.Add(item.DateCreate, item.Price);
                            continue;

                        }
                    }    
                }
                double data = 0;

                if (result.Count > 0)
                {
                    result.TryGetValue(result.Max(x => x.Key), out data);
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
        public DateTime GetNearestDay(DateTime date, int dayofweek)
        {
            var t2 = ((int)date.DayOfWeek);
            var num = dayofweek - t2;
            var t3 = new DateTime();
            if (num >= 0)
            {
                t3 = date.AddDays(num);
            }
            else
            {
                t3 = date.AddDays(7 + num);
            }
            return t3;
        }
    }
}
