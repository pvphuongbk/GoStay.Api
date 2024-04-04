﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.News
{
    public class NewsTabHome
    {
        public Dictionary<int,List<NewsHomeData>> NewsForCategories { get; set; }
        public List<NewsHomeData> HotNews { get; set; }
        public List<NewsHomeData> LatestNews { get; set; }
    }
    public class NewsHomeData
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public DateTime DateCreate { get; set; }
        public int IdCategory { get; set; }
        public string? PictureTitle { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public string CategoryEng { get; set; }
        public string CategoryChi { get; set; }
        public string UserName { get; set; }
        public int Click { get; set; }
        public string Slug { get; set; }
    }
}
