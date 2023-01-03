using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class ToursGallery
    {
        public int Id { get; set; }
        public int Idtours { get; set; }
        public int Idpic { get; set; }

        public virtual Picture IdpicNavigation { get; set; } = null!;
        public virtual Tour IdtoursNavigation { get; set; } = null!;
    }
}
