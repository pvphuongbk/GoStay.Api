using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Common.Helpers.Hotels
{
    public interface IHotelFunction
    {
        public HotelDetailDto CreateHotelDetailDto(Hotel hotel);

    }
}
