using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourProvinceTo
    {
        public int Id { get; set; }
        public int IdTour { get; set; }
        public int IdProvinceTo { get; set; }

        public virtual TinhThanh IdProvinceToNavigation { get; set; } = null!;
        public virtual Tour IdTourNavigation { get; set; } = null!;
    }
}
