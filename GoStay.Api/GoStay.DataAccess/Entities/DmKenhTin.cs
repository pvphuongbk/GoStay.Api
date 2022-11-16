using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class DmKenhTin
    {
        public string MaKenhTin { get; set; } = null!;
        public string? TenDanhMuc { get; set; }
        public bool? IsHienThi { get; set; }
        public int? Stt { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public int? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public string? UidName { get; set; }
    }
}
