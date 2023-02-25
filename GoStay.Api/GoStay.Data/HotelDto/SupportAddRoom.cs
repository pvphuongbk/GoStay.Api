using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.HotelDto
{
    public class SupportAddRoom
    {
        public List<ViewDirection> views { get; set; }
        public List<Service> servicesRoom { get; set; }
        public List<Palletbed> palletbed { get; set; }
    }
}
