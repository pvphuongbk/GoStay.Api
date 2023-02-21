using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto
{
    public class QuanDto
    {
        public int Id { get; set; }
        public int? IdTinhThanh { get; set; }
        public string? Tenquan { get; set; }
        public string? ProvinceName { get; set; }
        public int? Stt { get; set; }

    }
}
