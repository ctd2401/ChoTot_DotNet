using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ChoTot.DAL;
using ChoTot.MOD;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace ChoTot.BUS
{
    public class UserBUS
    {
        public BaseResultMOD Register(UserRegister item)
        {
            var Result = new BaseResultMOD();
                if(item== null || item.Name == null || item.Password == null || item.PhoneNumber==null)
                {
                    Result.Status = 0;
                    Result.Message = "Missing Field(s)";
                }
                return new UserDAL().RegisterDAL(item);
            return Result;
        }
        public BaseResultMOD Login(UserLogin user)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (user == null || user.Password == null || user.PhoneNumber == null)
                {
                    Result.Status = 0;
                    Result.Message = "Missing Field(s)";
                }
                else
                {
                    var UserLogin = new UserDAL().LoginDAL(user.PhoneNumber, user.Password);
                    if (UserLogin != null)
                    {
                        Result.Status = 1;
                        Result.Message = "Login successful";
                        Result.Data = UserLogin;
                    }
                    if (UserLogin == null)
                    {
                        Result.Status = 0;
                        Result.Message = "PhoneNumber or Password incorrect";
                    }
                    return Result;
                }
            }
            catch (Exception e)
            {
                Result.Status = -1;
                Result.Message = "Error";
                throw;
            }
            return Result;
        }
        public BaseResultMOD ViewUser(int quantity)
        {
            var Result = new BaseResultMOD();
            var UserReturn = new UserDAL().ViewUserDAL(quantity);
            if (UserReturn != null)
            {
                Result.Status = 1;
                Result.Message = "User(s) Found";
                Result.Data = UserReturn;
            }
            else
            {
                Result.Status = -1;
                Result.Message = "User Not Found";
            }
            return Result;
        }
        public BaseResultMOD ChangePassword(string oldpassword,string newpassword,string phonenumber)
        {
            var Result = new UserDAL().ChangePasswordDAL(oldpassword,newpassword,phonenumber);
            return Result;
        }
    }
}
