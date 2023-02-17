
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.Services.Users;
using Microsoft.AspNetCore.Mvc;
using ResponseBase = GoStay.Data.Base.ResponseBase;
namespace GoStay.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
        private readonly IUserService _userServices;

        public UserController(IUserService userServices)
		{
            _userServices=userServices;
        }
        [HttpGet("login")]
        public ResponseBase UserLogin(string email, string? password, Common.Enums.UserType enumType)
        {
            var items = _userServices.UserLogin(email,password, enumType);
            return items;
        }

        [HttpGet("check-user-by-phone")]
        public ResponseBase CheckUserByPhone(string phoneNumber)
        {
            var items = _userServices.CheckUserByPhone(phoneNumber);
            return items;
        }

        [HttpPost("register-user-phone")]
        public ResponseBase RegisterUserPhone(User user)
        {
            var items = _userServices.RegisterUserPhone(user);
            return items;
        }

        [HttpPost("register-user-email")]
        public ResponseBase RegisterUserEmail(User user)
        {
            var items = _userServices.RegisterUserEmail(user);
            return items;
        }

        [HttpGet("check-user-by-email")]
        public ResponseBase CheckUserByEmail(string email)
        {
            var items = _userServices.CheckUserByEmail(email);
            return items;
        }
        [HttpGet("check-user-by-id")]
        public ResponseBase CheckUserByID(int Id)
        {
            var items = _userServices.CheckUserByID(Id);
            return items;
        }
        [HttpPost("update-info")]
        public ResponseBase UpdateInfo(User user)
        {
            var items = _userServices.UpdateInfo(user);
            return items;
        }
        [HttpGet("check-user-by-account")]
        public ResponseBase CheckUserByAccount(string email, string password)
        {
            var items = _userServices.CheckUserByAccount(email,password);
            return items;
        }

    }
}