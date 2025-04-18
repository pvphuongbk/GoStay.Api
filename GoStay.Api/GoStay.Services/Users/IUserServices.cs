﻿using GoStay.Data.Base;
using GoStay.DataAccess.Entities;

namespace GoStay.Services.Users
{
	public interface IUserService
	{
        ResponseBase GetAllUser();
        ResponseBase SetAuthor(int UserId, int UserType);

        ResponseBase UserLogin(string email, string? password, Common.Enums.UserType enumType);
        ResponseBase CheckUserByPhone(string phoneNumber);
        ResponseBase RegisterUserPhone(User user);
        ResponseBase RegisterUserEmail (User user);
        ResponseBase CheckUserByEmail(string email);
        ResponseBase CheckUserByID(int ID);
        ResponseBase UpdateInfo(User user);
        ResponseBase CheckUserByAccount(string email, string password);
        public ResponseBase CheckUserByAccountAndGetToken(string email, string password);

        User GetById(int Id);


    }
}
