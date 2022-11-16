using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class AspNetUser
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public string? HoDem { get; set; }
        public string? Ten { get; set; }
        public string? MatKhau { get; set; }
        public string? AnhDaiDien { get; set; }
        public string? TinhTrangHonNhan { get; set; }
        public Guid? RoleId { get; set; }
        public int? Stt { get; set; }
        public int? KhoaTaiKhoan { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? AnhCmt { get; set; }
        public string? HoKhauTt { get; set; }
        public int? Deleted { get; set; }
        public int? Type { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
