using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoTot.Function
{
    public class Function
    {
        public string InitializeConnection()
        {
            string strCon = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["Settings:connectionstring"];
            SqlConnection SQLCon = null;
            return strCon;
        }
    }
}
