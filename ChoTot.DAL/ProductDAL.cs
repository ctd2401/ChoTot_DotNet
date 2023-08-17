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
using System.Runtime.InteropServices;
using System.Collections;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ChoTot.DAL
{
    public class ProductDAL
    {
        string strCon = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["Settings:connectionstring"];
        SqlConnection SQLCon = null;
        public BaseResultMOD AddProductDAL(Product item)
        {
            BaseResultMOD Result = new BaseResultMOD();
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                SqlCommand checkproduct = new SqlCommand("Check_Product_Exist",SQLCon);
                checkproduct.CommandType = CommandType.StoredProcedure;
                checkproduct.Parameters.AddWithValue("@productcode", item.ProductCode);
                SQLCon.Open();
                checkproduct.Connection = SQLCon;
                using (SqlDataReader dr = checkproduct.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        if (Int32.Parse(dr[0].ToString()) > 0)
                        {
                            Result.Status = -1;
                            Result.Message = "This productcode has been registered";
                            return Result;
                        }
                    }
                }
                SqlCommand cmd = new SqlCommand("Add_Product",SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                cmd.Parameters.AddWithValue("@Description", item.ProductDescription);
                cmd.Parameters.AddWithValue("@ProductCategory", item.ProductCategory);
                cmd.Parameters.AddWithValue("@ProductPrice", item.ProductPrice);
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "Added Product Sucess";
                    Result.Data = 1;
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "Added Failed";
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Result;
        }
        public JsonArray ViewProductDAL(PasePagingParams page)
        {
            JsonArray products = new JsonArray();
            Product item = null;
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                SqlCommand cmd = new SqlCommand("Select_Product_By_Page", SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@keyword", ("'%").ToString()+page.Keyword+("%'").ToString());
                cmd.Parameters.AddWithValue("@productperpage", page.PageSize);
                cmd.Parameters.AddWithValue("@order_by_name", page.OrderByName);
                cmd.Parameters.AddWithValue("@order_by_option", page.OrderByOption);
                cmd.Parameters.AddWithValue("@limit", page.Limit);
                cmd.Parameters.AddWithValue("@offset", page.Offset);
                SQLCon.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new Product();
                    item.ProductName = reader.GetString(1);
                    item.ProductCode = reader.GetString(2);
                    item.ProductDescription = reader.GetString(3);
                    item.ProductCategory = reader.GetString(4);
                    item.ProductPrice = Convert.ToSingle(reader[5]);
                    var jsonString = JsonConvert.SerializeObject(item);
                    products.Add(jsonString);
                }
                reader.Close();    
            }
            catch (Exception e)
            {
                throw;
            }
            return products;
        }
        public BaseResultMOD DeleteProductDAL(string ProductCode)
        {
            BaseResultMOD Result = new BaseResultMOD();
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                SqlCommand cmd = new SqlCommand("Delete_Product",SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@productcode", ProductCode);
                SQLCon.Open();
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "Delete Product Sucess";
                    Result.Data = 1;
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "Delete Failed";
                }
                SQLCon.Close();

            }
            catch (Exception e)
            {
                throw;
            }
            return Result;
        }
        public BaseResultMOD UpdateProductDAL(string ProductCode, Product changevalue)
        {
            BaseResultMOD Result = new BaseResultMOD();
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                SqlCommand cmd = new SqlCommand("Update_Product",SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = SQLCon;
                cmd.Parameters.AddWithValue("@productname", changevalue.ProductName);
                cmd.Parameters.AddWithValue("@productcode", changevalue.ProductCode);
                cmd.Parameters.AddWithValue("@description", changevalue.ProductDescription);
                cmd.Parameters.AddWithValue("@productcategory", changevalue.ProductCategory);
                cmd.Parameters.AddWithValue("@productprice", changevalue.ProductPrice);
                SQLCon.Open();
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "Update Product Sucess";
                    Result.Data = 1;
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "Update Failed";
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Result;
        }
        public Product SearchProductDAL(string ProductCode)
        {
            Product item = null;
            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strCon);
                }
                //if(changevalue.)
                SqlCommand cmd = new SqlCommand("Check_Product_Exist", SQLCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@productcode", ProductCode);
                SQLCon.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    item = new Product();
                    item.ProductName = reader.GetString(1);
                    item.ProductCode = reader.GetString(2);
                    item.ProductDescription = reader.GetString(3);
                    item.ProductCategory = reader.GetString(4);
                    item.ProductPrice = Convert.ToSingle(reader[5]);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return item;
        }
    }
}
