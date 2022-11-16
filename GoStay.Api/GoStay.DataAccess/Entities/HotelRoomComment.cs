using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelRoomComment
    {
        public int Id { get; set; }
        public int IdRoom { get; set; }
        public int IdHotel { get; set; }
        public Guid IdUser { get; set; }
        public int? ParentId { get; set; }
        public string? NoiDungCmt { get; set; }
        public string? TieuDe { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public int? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
