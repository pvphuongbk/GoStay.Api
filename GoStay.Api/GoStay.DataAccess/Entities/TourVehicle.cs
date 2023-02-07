using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourVehicle
    {
        public int Id { get; set; }
        public int? IdTour { get; set; }
        public byte? IdVehicle { get; set; }

        public virtual Tour? IdTourNavigation { get; set; }
        public virtual Vehicle? IdVehicleNavigation { get; set; }
    }
}
