using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.OrderDto
{
    public class UpdateStatusOrderParam
    {
        public byte Status { get; set; }
        public int IdOder { get; set; }
    }
    public class UpdatePrePaymentOrderParam
    {
        public decimal PrePayment { get; set; }
        public int IdOder { get; set; }
    }
    public class UpdateTotalAmountOrderParam
    {
        public int IdOrder { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infant { get; set; }
        public decimal totalAmount { get; set; }
    }
}
