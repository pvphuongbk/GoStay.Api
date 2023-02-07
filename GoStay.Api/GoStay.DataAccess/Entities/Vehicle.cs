using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            TourVehicles = new HashSet<TourVehicle>();
        }

        public byte Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TourVehicle> TourVehicles { get; set; }
    }
}
