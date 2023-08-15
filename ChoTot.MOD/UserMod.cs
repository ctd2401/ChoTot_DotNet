using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoTot.MOD
{
    public class UserRegister
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        protected int Role { get; set; }
    }  
    public class UserLogin
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    public class UserInfoReturn
    {
        public int Role { get; set; }
        public string UserName { get;set; }
        public string PhoneNumber { get; set; }
    }
    public class FunctionPermission
    {
        public string FunctionName { get; set; }
        public string FunctionCode { get; set; }
        public int Role { get; set; }
    }
}
