using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoTot.MOD
{
    public class Product
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public float ProductPrice { get; set; }
    }
    public class PasePagingParams
    {
        public string Keyword { get; set; } = ""; // tim kiem ten
        public string OrderByOption { get; set; } = "ASC"; // loc tu thap den cao hoac nguoc lai (abc)
        public string OrderByName { get; set; } = "";// loc theo truong nao
        public int PageSize { get; set; } = 10;// so spham tren 1 trang
        public int PageNumber { get; set; } = 1; // trang
        public int Offset { get { return (PageSize == 0 ? 10 : PageSize) * ((PageNumber == 0 ? 1 : PageNumber) - 1); } }
        public int Limit { get { return (PageSize == 0 ? 10 : PageSize); } }
        public int VaiTro { get; set; }
        public int? TrangThai { get; set; }
    }
}