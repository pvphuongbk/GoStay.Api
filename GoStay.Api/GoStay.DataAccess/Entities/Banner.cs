using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Banner
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? Image { get; set; }
        public byte? Stt { get; set; }
    }
}
