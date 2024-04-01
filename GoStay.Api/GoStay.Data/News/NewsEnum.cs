using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.News
{
    public enum NewsStatus
    {
        [Description("Draft")]
        Draft = 1,
        [Description("Waiting")]
        Waiting = 2,
        [Description("Accepted")]
        Accepted = 3,
        [Description("Reject")]
        Reject = 4
    }
}
