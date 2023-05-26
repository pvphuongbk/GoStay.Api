using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Users
{
    public class SetAuthorParam
    {
        public int UserId { get; set; }
        public int Usertype { get; set; }
    }
    public class CheckPermisionParam
    {
        public int[] Permission { get; set; }
        public int UserId { get; set; }
    }
    public class AppSettings
    {
        public string Secret { get; set; }
    }
}
