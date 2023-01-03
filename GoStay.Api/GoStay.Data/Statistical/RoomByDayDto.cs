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
            RoomByDayValue = new List<RoomByDayValue>();
        }
        public int TotalRoom { get; set; }
        public List<RoomByDayValue> RoomByDayValue { get; set; }
    }

    public class RoomByDayValue
    {
        public int Day { get; set; }

        public int Amount { get; set; }
    }
}
