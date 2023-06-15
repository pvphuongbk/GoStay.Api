using GoStay.Api.Providers;
using GoStay.Services.Statisticals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GoStay.Common.Helpers
{
    public class UpdateTimer
    {
        private static ISchedulerRoomPriceService _schedulerRoomPriceService;
        public static void Init()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnUpdateHotelRoomPrice);
            var httpContext = (IHttpContextAccessor)StaticServiceProvider.Provider.GetService(typeof(IHttpContextAccessor));
            var _schedulerRoomPriceService = (ISchedulerRoomPriceService)httpContext.HttpContext.RequestServices.GetService(typeof(ISchedulerRoomPriceService));
            aTimer.Interval = 5 * 1000; // 1h
            aTimer.Enabled = true;
        }

        private static void OnUpdateHotelRoomPrice(object source, ElapsedEventArgs e)
        {
            if(DateTime.Now.Hour == 0)
            {
                _schedulerRoomPriceService.UpdateDailyPriceForAllRoom();
            }
        }
    }
}
