using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.OrderDto
{
    public class ChartOdertByDayDto
    {
        public int Day { get; set; }
        public int Value { get; set; }
    }
    public class ChartOdertValue
    {
        public ChartOdertValue()
        {
            ChartOdertDetailValues = new List<ChartOdertDetailValue>();
        }
        public DateTime Date { get; set; }
        public int ID { get; set; }
        public List<ChartOdertDetailValue> ChartOdertDetailValues { get; set; }
    }

    public class ChartOdertDetailValue
    {
        public decimal? Price { get; set; }
        public Double? Discount { get; set; }
        public Double ActualPrice
        {
            get
            {
                if (Price == null)
                    return 0;
                else if (Discount == null)
                    return (Double)Price;
                return (double)((double)Price  - ((double)Price * (double)Discount) / 100);
            }
        }
    }
}
