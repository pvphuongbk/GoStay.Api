
using GoStay.DataAccess.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Picture : BaseEntity
    {

        public string? Url { get; set; }
        public string UrlOut
        {
            get
            {
                return "https://cdn.realtech.com.vn" + Url;
            }
        }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Type { get; set; }
		public int? IdAlbum { get; set; }
        public int? IdType { get; set; }
        public int? IdGroup { get; set; }
        public DateTime? Datein { get; set; }
    }
    public class PictureDto
    {
        public List<IFormFile?> Img { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Type { get; set; }
        public int IdAlbum { get; set; }
        public int IdType { get; set; }
        public int? IdGroup { get; set; }
        public int width { get; set; }
        public string newAlbum { get; set; }
    }
}