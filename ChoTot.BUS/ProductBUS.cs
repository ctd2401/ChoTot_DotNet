using ChoTot.DAL;
using ChoTot.MOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ChoTot.BUS
{
    public class ProductBUS
    {
        public BaseResultMOD AddProduct(Product item)
        {
            var Result = new BaseResultMOD();
                if (item == null || item.ProductName == null || item.ProductCode == null)
                {
                    Result.Status = 0;
                    Result.Message = "Missing Field(s)";
                }
                return new ProductDAL().AddProductDAL(item);
            return Result;
        }
        public JsonArray ViewProduct(PasePagingParams page)
        {
             return new ProductDAL().ViewProductDAL(page);
        }
        public BaseResultMOD DeleteProduct(string ProductCode)
        {
            return new ProductDAL().DeleteProductDAL(ProductCode);
        }
        public BaseResultMOD UpdateProduct(string ProductCode,Product changevalue)
        {
            return new ProductDAL().UpdateProductDAL(ProductCode,changevalue);
        }
        public BaseResultMOD SearchProduct(string ProductCode)
        {
            var Result = new BaseResultMOD();
            var SearchReturn = new ProductDAL().SearchProductDAL(ProductCode);
            if (SearchReturn != null)
            {
                Result.Status = 1;
                Result.Message = "Found Product";
                Result.Data = SearchReturn;
            }
            else
            {
                Result.Status = -1;
                Result.Message = "Product Not Found";
            }
            return Result;
        }
    }
}
