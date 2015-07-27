using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Models.Domains
{

    public class Login : AbsEntity
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Password2 { get; set; }
        public String Password3 { get; set; }
        public String IpAddress { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
