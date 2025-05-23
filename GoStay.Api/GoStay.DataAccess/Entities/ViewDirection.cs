﻿using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class ViewDirection
    {
        public ViewDirection()
        {
            RoomViews = new HashSet<RoomView>();
        }

        public int Id { get; set; }
        public string? ViewDirection1 { get; set; }
        public int? Deleted { get; set; }

        public virtual ICollection<RoomView> RoomViews { get; set; }
    }
}
