using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.Statistical
{
    public class RoomByDayDto
    {
        public RoomByDayDto()
        {
            RoomByDay = new Dictionary<string, ChartValue>();;
        }
        public int TotalRoom { get; set; }
        public Dictionary<string, ChartValue> RoomByDay { get; set; }
    }
}
