using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.OrderDto
{
    public class AddOrderDetailParam
    {
        public int IdOrder { get; set; }
        public OrderDetailDto orderDetail { get; set; }
    }
}
