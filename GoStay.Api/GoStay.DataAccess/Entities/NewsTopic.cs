using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class NewsTopic
    {
        public int Id { get; set; }
        public int IdNews { get; set; }
        public int IdNewsTopic { get; set; }

        public virtual News IdNewsNavigation { get; set; } = null!;
        public virtual TopicNews IdNewsTopicNavigation { get; set; } = null!;
    }
}
