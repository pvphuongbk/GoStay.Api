using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Authen
{
    public class AuthenticateRequest
    {
        public int UserId { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
    public class AuthenticateResponse
    {
        public User? User { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
    }

}
