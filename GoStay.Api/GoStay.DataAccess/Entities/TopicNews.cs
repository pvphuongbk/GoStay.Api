using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TopicNews
    {
        public TopicNews()
        {
            NewsTopics = new HashSet<NewsTopic>();
        }

        public int Id { get; set; }
        public string? Topic { get; set; }
        public int? Iddomain { get; set; }

        public virtual ICollection<NewsTopic> NewsTopics { get; set; }
    }
}
