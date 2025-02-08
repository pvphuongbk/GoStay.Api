using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelFlashSales
{
    public class HotelFlashSaleUpsertRequestModel
    {
        public int HotelId { get; set; }
        public bool IsPin {  get; set; }
        public bool IsDelete { get; set; }
    }
}
