using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class DmTinTuc
    {
        public int Id { get; set; }
        public string? MaDmKenhTin { get; set; }
        public string? TieuDe { get; set; }
        public string? TieuDeSeo { get; set; }
        public string? NoiDung { get; set; }
        public string? NoiDungChiTiet { get; set; }
        public string? AnhDaiDien { get; set; }
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
