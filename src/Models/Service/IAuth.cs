using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service
{
    public interface IAuth
    {        
        bool Authenticate(String userName, String password, String scope, bool remember);
        dynamic GetUserDetails(String userName);
        List<String> GetUserPermissions(String userName);
    }
}
