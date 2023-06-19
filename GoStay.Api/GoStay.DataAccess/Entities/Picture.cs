
using GoStay.DataAccess.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Picture : BaseEntity
    {
        public int Id { get; set; }
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
        public byte? ImpLevel { get; set; }
        public DateTime? Datein { get; set; }
        public int? Size { get; set; }
        public int? HotelId { get; set; }
        public int? HotelRoomId { get; set; }
        public int? TourId { get; set; }
        public int? NewsId { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual HotelRoom? HotelRoom { get; set; }
        public virtual Album? IdAlbumNavigation { get; set; }
        public virtual News? News { get; set; }
        public virtual Tour? Tour { get; set; }
    }
    public class PictureDto
    {
        public List<IFormFile?> Img { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Type { get; set; }
        public int? IdAlbum { get; set; }
        public int IdType { get; set; }
        public int? IdGroup { get; set; }
        public int width { get; set; }
        public string newAlbum { get; set; }
    }
}
