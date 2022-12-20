using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Order
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int IdUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public byte? Status { get; set; }
        public byte IdPtthanhToan { get; set; }
        public string? More { get; set; }
        public string? Session { get; set; }
    }
}
