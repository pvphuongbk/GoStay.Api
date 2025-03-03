using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Video
{
    public class CreateVideoModel
    {
        public int Id { get; set; }
        public string Video { get; set; } = null!;

        public int? IdCategory { get; set; }
        public string? Title { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateEdit { get; set; }
        public int? IdUser { get; set; }
        public int Deleted { get; set; }
        public string? PictureTitle { get; set; }
        public int? LangId { get; set; }
        public int Status { get; set; }
        public string? Name { get; set; }
        public string? Descriptions { get; set; }
        public string? KeySearch { get; set; }
        public int? Click { get; set; }
        public decimal? Lon { get; set; }
        public decimal? Lat { get; set; }
    }
}
