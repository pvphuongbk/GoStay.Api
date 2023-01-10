using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Picture
    {
        public int Id { get; set; }
		public Picture()
        {
            NewsGalleries = new HashSet<NewsGallery>();
        }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Type { get; set; }
        public int? Deleted { get; set; }
        public int? IdAlbum { get; set; }
        public int? IdType { get; set; }
        public int? IdGroup { get; set; }
        public DateTime? Datein { get; set; }
        public int? Size { get; set; }
        public int? HotelId { get; set; }
        public int? HotelRoomId { get; set; }
        public int? TourId { get; set; }
        public int? NewsId { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual HotelRoom? HotelRoom { get; set; }
        public virtual Album? IdAlbumNavigation { get; set; }
		public virtual Tour? Tour { get; set; }
        public virtual ICollection<NewsGallery> NewsGalleries { get; set; }
    }
}
