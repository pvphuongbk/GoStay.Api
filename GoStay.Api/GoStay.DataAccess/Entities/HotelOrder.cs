using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelOrder
    {
        public int? Id { get; set; }
        public int? IdRoom { get; set; }
        public int? NumberRoomNd { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? DateOrder { get; set; }
        public decimal? TotalMoney { get; set; }
        public int? Status { get; set; }
        public int? IsPaymented { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public int? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
