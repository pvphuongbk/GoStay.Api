using GoStay.DataAccess.Base;
using GoStay.DataAccess.Interface;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Service : BaseEntity
    {
        public Service()
        {
            HotelMamenitis = new HashSet<HotelMameniti>();
            RoomMamenitis = new HashSet<RoomMameniti>();
        }


        public string Name { get; set; }
        public byte? AdvantageLevel { get; set; }
        public int? IdStyle { get; set; }
        public string? Icon { get; set; }
        public string? NameEng { get; set; }
        public string? NameChi { get; set; }

        public virtual ICollection<HotelMameniti> HotelMamenitis { get; set; }
        public virtual ICollection<RoomMameniti> RoomMamenitis { get; set; }
    }
}
