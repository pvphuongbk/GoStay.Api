using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class NewsGallery
    {
        public int Id { get; set; }
        public int Idnews { get; set; }
        public int Idpic { get; set; }

        public virtual News IdnewsNavigation { get; set; } = null!;
        public virtual Picture IdpicNavigation { get; set; } = null!;
    }
}
