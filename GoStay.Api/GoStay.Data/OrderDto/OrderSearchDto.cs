using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.OrderDto
{
    public class OrderSearchDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string OrderCode { get; set; }
        public int Status { get; set; }
        public int Style { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class OrderSearchParam
    {
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? OrderCode { get; set; }
        public int? Status { get; set; }
        public int? Style { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class OrderSearchOutDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string? Ordercode { get; set; }
        public int? IdUser { get; set; }
        public string? UserName { get; set; }
        public string? Status { get; set; }
        public int? Style { get; set; }
        public double? TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
