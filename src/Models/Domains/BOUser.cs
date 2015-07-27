using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
namespace Models.Domains
{

    public class BOUser : AbsEntity
    {
        public String LoginId { get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Address { get; set; }
        public String DepartmentId { get; set; }
    }
}
