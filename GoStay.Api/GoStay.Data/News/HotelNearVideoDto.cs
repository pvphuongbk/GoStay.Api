using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.News
{
    public class HotelNearVideoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Province { get; set; }=string.Empty;
        public string District { get; set; } = string.Empty;
        public DateTime? LastOrderTime { get; set; } 
        public string LastOrder 
        { get
            {
                if (LastOrderTime.HasValue)
                {
                    var timespan = DateTime.Now - LastOrderTime.Value;
                    if (timespan.TotalSeconds <= 60)
                        return "Vừa được đặt";
                    if (timespan.TotalSeconds > 60 && timespan.TotalMinutes < 60)
                        return $"Đặt cách đây {Math.Round(timespan.TotalMinutes,0)} phút";

                    if (timespan.TotalMinutes >= 60 && timespan.TotalHours < 24)
                        return $"Đặt cách đây {Math.Round(timespan.TotalHours, 0)} giờ";

                    if (timespan.TotalMinutes >= 24 && timespan.TotalDays < 7)
                        return $"Đặt cách đây {Math.Round(timespan.TotalDays, 0)} ngày";
                    return "Được đặt cách đây vài ngày";
                }
                else
                {
                    return "Được đặt cách đây vài ngày";
                }
            }
        }
        public decimal Price { get; set; }
        public double Discount {  get; set; } 
        public string Slug { get; set; } = string.Empty;
        public double Distance {  get; set; }
        public List<string> Picture { get; set; } = new List<string>();
    }
    public class HotelNearDto
    {
        public int Id { get; set; }
        public double Lon {  get; set; }
        public double Lat { get; set; }
        public double Distance { get; set; }
    }
}
