﻿using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class News
    {
        public News()
        {
            Pictures = new HashSet<Picture>();
        }

        public int Id { get; set; }
        public int IdCategory { get; set; }
        public int IdUser { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateEdit { get; set; }
        public byte Status { get; set; }
        public string? Keysearch { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public int Deleted { get; set; }
        public string? PictureTitle { get; set; }
        public int LangId { get; set; }
        public int IdTopic { get; set; }

        public virtual NewsCategory IdCategoryNavigation { get; set; } = null!;
        public virtual NewsTopic IdTopicNavigation { get; set; } = null!;
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual Language Lang { get; set; } = null!;
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
