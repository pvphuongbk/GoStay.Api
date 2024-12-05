using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class VideoNews
    {
		public VideoNews()
        {
            CommentVideos = new HashSet<CommentVideo>();
        }
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
        public string? Descriptions { get; set; }
        public string? KeySearch { get; set; }
        public int? Click { get; set; }
        public virtual NewsCategory? IdCategoryNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }
        public virtual Language? Lang { get; set; }
        public virtual ICollection<CommentVideo> CommentVideos { get; set; }
    }
}
