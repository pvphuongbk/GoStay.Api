using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class DmMenuChinh
    {
        public string MaTrang { get; set; } = null!;
        public string? TenTrang { get; set; }
        public string? MoTa { get; set; }
        public int? Stt { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public int? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public string? UidName { get; set; }
        public int? Status { get; set; }
        public int? Level { get; set; }
        public string? MaTrangCha { get; set; }
        public string? Url { get; set; }
    }
}
