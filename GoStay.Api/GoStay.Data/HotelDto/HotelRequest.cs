using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelDto
{
    public class SetMapHotelRequest
    {
        public int HotelId { get; set; }
        public float LON { get; set; }
        public float LAT { get; set; }

    }
}
