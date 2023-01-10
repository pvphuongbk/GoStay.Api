using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourDistrictTo
    {
        public int Id { get; set; }
        public int IdTour { get; set; }
        public int IdDistrictTo { get; set; }
        public string? Description { get; set; }
        public int Deleted { get; set; }

        public virtual Quan IdDistrictToNavigation { get; set; } = null!;
        public virtual Tour IdTourNavigation { get; set; } = null!;
    }
}
