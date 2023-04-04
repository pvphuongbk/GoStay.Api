using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TourDetail
    {
        public int Id { get; set; }
        public int? IdTours { get; set; }
        public byte? IdStyle { get; set; }
        public string? Title { get; set; }
        public string? Details { get; set; }
        public int? Deleted { get; set; }
        public byte? Stt { get; set; }
        public int? LangId { get; set; }

        public virtual TourDetailsStyle? IdStyleNavigation { get; set; }
        public virtual Tour? IdToursNavigation { get; set; }
    }

}
