using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelFlashSales
{
    public class HotelFlashSalePresentViewModel
    {
        public List<HotelFlashSaleObjectModel> ListPresent { get; set; }
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class HotelFlashSaleSelectionViewModel
    {
        public List<HotelFlashSaleForSelection> ListSelection { get; set; }
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class HotelFlashSaleObjectModel
    {
        public int HotelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public bool IsPin {  get; set; }
    }
    public class HotelFlashSaleForSelection
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
    }
}
