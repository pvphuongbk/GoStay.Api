using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelDto
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? Type { get; set; }
        public int? IdPriceRange { get; set; }
        public string? PriceRange { get; set; }
    }
    public class RequestGetListHotel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int? IdProvince { get; set; }
    }
}
