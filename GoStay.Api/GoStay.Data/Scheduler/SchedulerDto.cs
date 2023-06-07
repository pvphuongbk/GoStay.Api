using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Scheduler
{
    public class RoomPriceParam
    {
        public int month { get; set; }

        public int year { get; set; }
        public List<int> RoomIds { get; set; }
        public int day { get; set; }
    }
}
