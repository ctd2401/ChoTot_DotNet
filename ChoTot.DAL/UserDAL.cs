using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ChoTot.MOD;
using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;
using BCrypt.Net;
using System.Runtime.InteropServices;
using System.Collections;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
namespace ChoTot.DAL
{
    public class UserDAL
    {
        string strCon = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["Settings:connectionstring"];
        SqlConnection SQLCon = null;
        public BaseResultMOD RegisterDAL(UserRegister item)
        {
            var Result = new BaseResultMOD();
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(item.Password);
                if (SQLCon == null) {
                    SQLCon = new SqlConnection(strCon);                
                }
                SqlCommand checkexist = new SqlCommand("Check_User_Exist",SQLCon);
                checkexist.CommandType = CommandType.StoredProcedure;
                checkexist.Connection = SQLCon;
                checkexist.Parameters.AddWithValue("@phoneNumber", item.PhoneNumber);
                SQLCon.Open();
                using (SqlDataReader dr = checkexist.ExecuteReader())
                {
                    while (dr.Read())
                    {
                      
                        if(Int32.Parse(dr[0].ToString())>0)
                        {
                            Result.Status = -1;
                            Result.Message = "This phonenumber has been registered";
                            return Result;
                        }
                    }
                }
                SqlCommand cmd = new SqlCommand("User_Register",SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@username", item.Name);
                cmd.Parameters.AddWithValue("@phonenumber", item.PhoneNumber);
                cmd.Parameters.AddWithValue("@password", passwordHash);
                cmd.Parameters.AddWithValue("@role", 1);
                cmd.ExecuteNonQuery();
                if(SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "Register Sucess";
                    Result.Data = 1;
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "Register Failed";
                }
            }
            catch(Exception e)
            {
                throw;
            }
            return Result;
        }
        public UserInfoReturn LoginDAL(string PhoneNumber, string Password)
        {
            UserInfoReturn item = null;
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                if (SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Close();
                }
                SqlCommand cmd = new SqlCommand("Check_User_Exist",SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@phoneNumber", PhoneNumber);
                SQLCon.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string hashedPassword = reader.GetString(2);
                    if(BCrypt.Net.BCrypt.Verify(Password,hashedPassword))
                    {
                        item = new UserInfoReturn();
                        item.Role = reader.GetInt32(4);
                        item.UserName = reader.GetString(1);
                        item.PhoneNumber = reader.GetString(3);
                    }
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw;
            }
            return item;
        }
        public JsonArray ViewUserDAL(int quantity)
        {
            UserInfoReturn item = null;
            JsonArray users = new JsonArray();
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                if (SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Close();
                }
                SqlCommand cmd = new SqlCommand("Select_User",SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@quantity", quantity);
                SQLCon.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    item = new UserInfoReturn();
                    item.Role = reader.GetInt32(0);
                    item.UserName = reader.GetString(1);
                    item.PhoneNumber = reader.GetString(2);
                    var jsonString = JsonConvert.SerializeObject(item);
                    users.Add(jsonString);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw;
            }
            return users;
        }
        public BaseResultMOD ChangePasswordDAL(string oldpassword,string newpassword,string phonenumber)
        {
            BaseResultMOD Result = new BaseResultMOD();
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                if (SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Close();
                }
                SqlCommand cmd = new SqlCommand("Check_User_Exist", SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@phoneNumber", phonenumber);
                SQLCon.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                string hashedPassword = "";
                while (reader.Read())
                {
                    hashedPassword = reader.GetString(2);
                }
                reader.Close();
                if (BCrypt.Net.BCrypt.Verify(oldpassword, hashedPassword))
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(newpassword);
                    SqlCommand cmd2 = new SqlCommand("Change_Password", SQLCon);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Connection = SQLCon;
                    cmd2.Parameters.AddWithValue("@phoneNumber", phonenumber);
                    cmd2.Parameters.AddWithValue("@newpassword", passwordHash);
                    cmd2.ExecuteNonQuery();
                    if (cmd != null)
                    {
                        Result.Status = 1;
                        Result.Message = "Change Password success!";
                    }
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "Old Password Wrong";
                    return Result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Result;
        }

    }
}
