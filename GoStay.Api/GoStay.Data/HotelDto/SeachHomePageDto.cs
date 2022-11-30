using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.HotelDto
{
    public class SeachHomePageDto
    {
        public int TypeAddress { get; set; }
        public int AddressId { get; set; }
        public int Palletbed { get; set; }
        public int NumChild { get; set; }
        public int NumMature { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
