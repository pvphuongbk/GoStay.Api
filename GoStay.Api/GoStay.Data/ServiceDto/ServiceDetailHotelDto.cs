using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoStay.Data.ServiceDto
{
    public class ServiceDetailHotelDto
    {
        [JsonIgnore]
        public int IdRoom { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public byte? AdvantageLevel { get; set; }
        public string? NameEng { get; set; }
        public string? NameChi { get; set; }
    }
}
