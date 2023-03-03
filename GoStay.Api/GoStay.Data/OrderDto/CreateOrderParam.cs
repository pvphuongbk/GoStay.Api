using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.OrderDto
{
    public class CreateOrderParam
    {
        public OrderDto order { get; set; }
        public OrderDetailDto orderDetail { get; set; }
    }
    public class InsertOrderParam
    {
        public OrderDto order { get; set; }
        public OrderDetailDto orderDetail { get; set; }
    }
    public class OrderDto
    {
        public string? Title { get; set; }
        public string? Ordercode { get; set; }
        public int IdHotel { get; set; }
        public int IdUser { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public byte? Status { get; set; }
        public byte IdPaymentMethod { get; set; }
        public string? MoreInfo { get; set; }
        public string? Session { get; set; }
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public DateTime? ChechIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public byte? Num { get; set; }
        public DateTime? DateCreate { get; set; }
        public decimal? Price { get; set; }
        public double? Discount { get; set; }
        public string? MoreInfo { get; set; }
        public byte DetailStyle { get; set; }

    }

    public class InsertOrderDetailDto
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public DateTime? ChechIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public byte? Num { get; set; }
        public DateTime? DateCreate { get; set; }
        public decimal? Price { get; set; }
        public double? Discount { get; set; }
        public string? MoreInfo { get; set; }
        public bool IsDeleted { get; set; }
        public byte DetailStyle { get; set; }
    }
    public class OrderDetailShowDto
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int? IdRoom { get; set; }
        public int? IdTour { get; set; }
        public DateTime? ChechIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public byte? Num { get; set; }
        public DateTime? DateCreate { get; set; }
        public decimal? Price { get; set; }
        public double? Discount { get; set; }
        public string? MoreInfo { get; set; }
    }
}
