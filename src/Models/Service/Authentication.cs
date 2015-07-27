using Microsoft.Practices.Unity;
using Models.Dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service
{
    public class Authentication:IAuth
    {
        [Dependency]
        public LoginDao LoginDao
        {
            set;
            private get;
        }
        public bool Authenticate(String userName, String password, String scope, bool remember)
        {

            return LoginDao.authentication(userName, password);
           
        }

        public dynamic GetUserDetails(String userName)
        {
            var query = LoginDao.DbSet.AsQueryable();
            return query.Where(u => u.UserName == userName)
                 .FirstOrDefault();
        }
        public List<String> GetUserPermissions(String userName)
        {
             List<String> permits = new List<String>();
            permits.Add("super");
            return permits;
        }
    }
}
