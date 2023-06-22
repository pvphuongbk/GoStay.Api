using GoStay.DataAccess.Entities;

namespace GoStay.Repository.Repositories
{
    public class SchedulerRepository
    {
        public static Dictionary<string, int> DayOfWeekScheduler = new Dictionary<string, int>()
        {
            { "SU" , 0 },
            { "MO" , 1 },
            { "TU" , 2 },
            { "WE" , 3 },
            { "TH" , 4 },
            { "FR" , 5 },
            { "SA" , 6 }
        };

        public static double GetPrice(IQueryable<SchedulerRoomPrice> schedulerRoomPrices ,int month, int year, int day)
        {
            try
            {
                Dictionary<DateTime, double> result = new Dictionary<DateTime, double>();
                var t1 = new DateTime(year, month, day);
                var t2 = t1.DayOfWeek.ToString().Substring(0, 2).ToUpper();
                foreach (var item in schedulerRoomPrices)
                {
                    List<DateTime> tException = new List<DateTime>();
                    bool skip = false;
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
                    foreach (var tEx in tException)
                    {
                        if (t1 == tEx)
                        {
                            skip = true;
                            break;
                        }
                    }
                    if (skip == true)
                        continue;

                    if (item.RecurrenceRule != null)
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
                        RecurrenceRule.TryGetValue("FREQ", out freq);
                        if (freq == "DAILY")
                        {
                            //if(t1>item.End)
                            //{ continue; }
                            //else
                            //{
                                var DayStart = item.Start;
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
                                        DayStart = DayStart.AddDays(intervalF);
                                    }
                                    continue;
                                }
                                else
                                {
                                    int i = 1;
                                    while (DayStart <= t1 && i <= countF)
                                    {
                                        if (DayStart == t1)
                                        {
                                            result.Add(item.DateCreate, item.Price);
                                            break;
                                        }

                                        DayStart = DayStart.AddDays(intervalF);
                                        i++;
                                    }
                                    continue;

                                }
                        //}
                        }
                        if (freq == "WEEKLY")
                        {
                            if (RecurrenceRule.TryGetValue("BYDAY", out byday))
                            {
                                if (byday.Contains(t2))
                                {
                                    var DayStart = GetNearestActiveWeekly(item.Start, (int)t1.DayOfWeek);
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
                                        while (DayStart <= t1 && i <= countF)
                                        {
                                            if (DayStart == t1)
                                            {
                                                result.Add(item.DateCreate, item.Price);
                                                break;
                                            }

                                            DayStart = DayStart.AddDays(intervalF * 7);
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
                        }
                        if (freq == "MONTHLY")
                        {
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
                                                result.Add(t1, item.Price);
                                                break;
                                            }

                                            MonthStart = MonthStart + intervalF;
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        int i = 1;
                                        while (MonthStart <= t1.Month && i <= countF)
                                        {
                                            if (MonthStart == t1.Month)
                                            {
                                                result.Add(t1, item.Price);
                                                continue;
                                            }

                                            MonthStart = MonthStart + intervalF;
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
                        }
                    }
                    else
                    {
                        if (t1 >= item.Start && t1 <= item.End)
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
                
                return data;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static Dictionary<string, string> GetRecurrenceRule(string Recurrence)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var t1 = Recurrence.Split(";");
            foreach (var item in t1)
            {
                var temp = item.Split("=");
                var key = temp[0];
                var value = temp[1];
                result.Add(key, value);
            }
            return result;
        }

        public static Dictionary<DateTime, double> GetFutureDayPriceRoom(IQueryable<SchedulerRoomPrice> schedulerRoomPrices, DateTime EndDate)
        {
            try
            {
                Dictionary<DateTime, double> finalData = new Dictionary<DateTime, double>();

                foreach (var scheduler in schedulerRoomPrices)
                {
                    if (scheduler.RecurrenceRule == null)
                    {
                        var data = GetFutureDayNonFreq(scheduler, EndDate);
                        foreach(var item in data)
                        {
                            if(finalData.ContainsKey(item.Key)==false)
                                finalData.Add(item.Key, item.Value);
                        }    
                    }
                    else
                    {
                        var rule = GetRecurrenceRule(scheduler.RecurrenceRule);
                        string freq = "";
                        rule.TryGetValue("FREQ", out freq);
                        if (freq == "DAILY")
                        {
                            var data = GetFutureDayFreqIsDaily(scheduler, EndDate);
                            foreach (var item in data)
                            {
                                if (finalData.ContainsKey(item.Key) == false)
                                    finalData.Add(item.Key, item.Value);
                            }
                        }
                        if (freq == "WEEKLY")
                        {
                            var data = GetFutureDayFreqIsWeekly(scheduler, EndDate);
                            foreach (var item in data)
                            {
                                if (finalData.ContainsKey(item.Key) == false)
                                    finalData.Add(item.Key, item.Value);
                            }
                        }
                        if (freq == "MONTHLY")
                        {
                            var data = GetFutureDayFreqIsMonthly(scheduler, EndDate);
                            foreach (var item in data)
                            {
                                if (finalData.ContainsKey(item.Key) == false)
                                    finalData.Add(item.Key, item.Value);
                            }
                        }
                    }    
                }

                return finalData;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Dictionary<DateTime, double> GetFutureDayNonFreq(SchedulerRoomPrice scheduler, DateTime EndDate)
        {
            try
            {
                Dictionary<DateTime, double> result = new Dictionary<DateTime, double>();
                List<DateTime> tException = new List<DateTime>();
                if (scheduler.RecurrenceException != null)
                {
                    var listEx = scheduler.RecurrenceException.Split(",");
                    foreach (var ex in listEx)
                    {
                        var yEx = int.Parse(ex.Substring(0, 4));
                        var mEx = int.Parse(ex.Substring(4, 2));
                        var dEx = int.Parse(ex.Substring(6, 2));
                        var tEx = new DateTime(yEx, mEx, dEx);
                        tException.Add(tEx);
                    }

                }
                
                DateTime start = (DateTime.Today > scheduler.Start) ? DateTime.Today : scheduler.Start;
                DateTime end = (EndDate < scheduler.End) ? EndDate : scheduler.End;

                for (DateTime time = start; time <= end; time = time.AddDays(1))
                {
                    if (tException.Contains(time))
                        continue;
                    result.Add(time, scheduler.Price);
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Dictionary<DateTime, double> GetFutureDayFreqIsDaily(SchedulerRoomPrice scheduler, DateTime EndDate)
        {
            try
            {
                Dictionary<DateTime, double> result = new Dictionary<DateTime, double>();
                List<DateTime> tException = new List<DateTime>();
                if (scheduler.RecurrenceException != null)
                {
                    var listEx = scheduler.RecurrenceException.Split(",");
                    foreach (var ex in listEx)
                    {
                        var yEx = int.Parse(ex.Substring(0, 4));
                        var mEx = int.Parse(ex.Substring(4, 2));
                        var dEx = int.Parse(ex.Substring(6, 2));
                        var tEx = new DateTime(yEx, mEx, dEx);
                        tException.Add(tEx);
                    }

                }
                var RecurrenceRule = new Dictionary<string, string>();
                var count = "";
                var interval = "";
                var until = "";
                DateTime Until = DateTime.Today.AddDays(-1);
                int countF = 0;
                int intervalF = 1;
                if (scheduler.RecurrenceRule != null)
                {
                    RecurrenceRule = GetRecurrenceRule(scheduler.RecurrenceRule);
                    if (RecurrenceRule.TryGetValue("UNTIL", out until))
                    {
                        var y = int.Parse(until.Substring(0, 4));
                        var m = int.Parse(until.Substring(4, 2));
                        var d = int.Parse(until.Substring(6, 2));
                        Until = new DateTime(y, m, d);
                    }
                    if (RecurrenceRule.TryGetValue("COUNT", out count))
                    {
                        countF = int.Parse(count);
                    }
                    if (RecurrenceRule.TryGetValue("INTERVAL", out interval))
                    {
                        intervalF = int.Parse(interval);
                    }
                }
                DateTime start = (DateTime.Today > scheduler.Start) ? DateTime.Today: scheduler.Start;
                start = GetNearestActiveDayly(scheduler.Start, intervalF, start);
                DateTime end = EndDate ;
                if(until != null)
                {
                    end = (end < Until) ? end : Until;
                }
                if(countF>0)
                {
                    end = scheduler.Start.AddDays(intervalF * (countF - 1));
                }    
                for ( DateTime time = start; time <= end; time = time.AddDays((1> intervalF)? 1: intervalF))
                {
                    if (tException.Contains(time))
                        continue;
                    result.Add(time, scheduler.Price);
                }    
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static Dictionary<DateTime, double> GetFutureDayFreqIsWeekly(SchedulerRoomPrice scheduler, DateTime EndDate)
        {
            try
            {
                Dictionary<DateTime, double> result = new Dictionary<DateTime, double>();
                List<DateTime> tException = new List<DateTime>();
                if (scheduler.RecurrenceException != null)
                {
                    var listEx = scheduler.RecurrenceException.Split(",");
                    foreach (var ex in listEx)
                    {
                        var yEx = int.Parse(ex.Substring(0, 4));
                        var mEx = int.Parse(ex.Substring(4, 2));
                        var dEx = int.Parse(ex.Substring(6, 2));
                        var tEx = new DateTime(yEx, mEx, dEx);
                        tException.Add(tEx);
                    }

                }
                var RecurrenceRule = new Dictionary<string, string>();
                var byday = "";
                var count = "";
                var interval = "";
                var until = "";
                DateTime Until = DateTime.Today.AddDays(-1);
                int countF = 0;
                int intervalF = 1;
                if (scheduler.RecurrenceRule != null)
                {
                    RecurrenceRule = GetRecurrenceRule(scheduler.RecurrenceRule);
                    if (RecurrenceRule.TryGetValue("UNTIL", out until))
                    {
                        var y = int.Parse(until.Substring(0, 4));
                        var m = int.Parse(until.Substring(4, 2));
                        var d = int.Parse(until.Substring(6, 2));
                        Until = new DateTime(y, m, d);
                    }
                    if (RecurrenceRule.TryGetValue("COUNT", out count))
                    {
                        countF = int.Parse(count);
                    }
                    if (RecurrenceRule.TryGetValue("INTERVAL", out interval))
                    {
                        intervalF = int.Parse(interval);
                    }
                    RecurrenceRule.TryGetValue("BYDAY", out byday);
                }
                var listDOWstring = byday.Split(",");
                var listDOWint = new List<int>();
                foreach(var item in listDOWstring)
                {
                    int value = 7;
                    DayOfWeekScheduler.TryGetValue(item, out value);
                    if(value<7)
                        listDOWint.Add(value);
                }

                foreach (var dowint in listDOWint)
                {
                    DateTime startRule = GetNearestActiveWeekly(scheduler.Start, dowint);
                    DateTime start = startRule;

                    while(DateTime.Today> start)
                    {
                        start = start.AddDays(7 * intervalF);
                    }
                    if (DateTime.Today <= startRule)
                    {
                        start = startRule;
                    }

                    //start = (DateTime.Today>start)? DateTime.Today;


                    DateTime end = EndDate;
                    DateTime endCount = end;
                    if (until != null)
                    {
                        end = (end < Until) ? end : Until;
                    }
                    if (countF > 0)
                    {
                        endCount = GetEndDateActiveWeekly(scheduler.Start, countF, intervalF);
                        end = (end < endCount) ? end : endCount;
                    }
                    
                    for (DateTime time = start; time <= end; time = time.AddDays((1 > intervalF) ? 7 : intervalF * 7))
                    {
                        if (tException.Contains(time))
                            continue;
                        result.Add(time, scheduler.Price);

                    }
                    
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static Dictionary<DateTime, double> GetFutureDayFreqIsMonthly(SchedulerRoomPrice scheduler, DateTime EndDate)
        {
            try
            {
                Dictionary<DateTime, double> result = new Dictionary<DateTime, double>();
                List<DateTime> tException = new List<DateTime>();
                if (scheduler.RecurrenceException != null)
                {
                    var listEx = scheduler.RecurrenceException.Split(",");
                    foreach (var ex in listEx)
                    {
                        var yEx = int.Parse(ex.Substring(0, 4));
                        var mEx = int.Parse(ex.Substring(4, 2));
                        var dEx = int.Parse(ex.Substring(6, 2));
                        var tEx = new DateTime(yEx, mEx, dEx);
                        tException.Add(tEx);
                    }

                }
                var RecurrenceRule = new Dictionary<string, string>();
                var bymonthday = "";
                var count = "";
                var interval = "";
                var until = "";
                DateTime Until = DateTime.Today.AddDays(-1);
                int countF = 0;
                int intervalF = 1;
                if (scheduler.RecurrenceRule != null)
                {
                    RecurrenceRule = GetRecurrenceRule(scheduler.RecurrenceRule);
                    if (RecurrenceRule.TryGetValue("UNTIL", out until))
                    {
                        var y = int.Parse(until.Substring(0, 4));
                        var m = int.Parse(until.Substring(4, 2));
                        var d = int.Parse(until.Substring(6, 2));
                        Until = new DateTime(y, m, d);
                    }
                    if (RecurrenceRule.TryGetValue("COUNT", out count))
                    {
                        countF = int.Parse(count);
                    }
                    if (RecurrenceRule.TryGetValue("INTERVAL", out interval))
                    {
                        intervalF = int.Parse(interval);
                    }
                    RecurrenceRule.TryGetValue("BYMONTHDAY", out bymonthday);
                }
                int date = int.Parse(bymonthday);
                DateTime startRule = scheduler.Start;

                if (date >= scheduler.Start.Day)
                {
                    startRule = scheduler.Start.AddDays(date - scheduler.Start.Day);
                }
                else
                {
                    startRule = new DateTime(scheduler.Start.Year, scheduler.Start.Month + 1, date);
                }  
                DateTime start = startRule;
                DateTime starttemp = DateTime.Today;

                while (starttemp.Month > start.Month)
                {
                    start = start.AddMonths(intervalF);
                }
                start = (start< starttemp)? start.AddMonths(intervalF) : start;

                DateTime end =  EndDate;
                DateTime endCount = EndDate;

                if (until != null)
                {
                    end = (EndDate < Until) ? EndDate : Until;
                }
                if(countF>0)
                {
                    endCount = startRule.AddMonths(intervalF * (countF - 1));
                    end = (EndDate < endCount) ? EndDate : endCount;
                }    
                for (DateTime time = start; time <= end; time = time.AddMonths(intervalF))
                {
                    if (tException.Contains(time))
                        continue;
                    result.Add(time, scheduler.Price);
                }
                
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DateTime GetNearestActiveDayly (DateTime startday, int interval, DateTime time)
        {
            var temp1 = time- startday;
            var temp2 = temp1.Days % interval;
            if(temp2==0)
            {
                return time;
            }
            else
            {
                return time.AddDays(interval - temp2);
            }    
        }
        public static DateTime GetNearestActiveWeekly(DateTime date, int dayofweek)
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
        public static DateTime GetEndDateActiveWeekly(DateTime startdate, int count, int interval)
        {
            return startdate.AddDays(7 * interval * (count - 1));
        }
    }
}
