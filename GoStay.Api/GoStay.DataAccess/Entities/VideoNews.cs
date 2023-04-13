using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class VideoNews
    {
        public int Id { get; set; }
        public string Video { get; set; } = null!;
        public string UrlVideo
        {
            get
            {
                return "https://cdn.realtech.com.vn" + Video;
            }
        }
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
    }
}
