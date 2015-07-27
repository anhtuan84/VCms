using Core;
using Core.Dao;
using Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class LoginDao : GenericDao<Login>
    {
        protected override void VerifyOnUpdate(Login existedEntity, Login newEntity)
        {
 	        //TODO:balba

        }

        public bool authentication(String userName, String password)
        {
            var pwd = SecurityHelper.MD5(password);
            var query = this.DbSet.AsQueryable();
            var count = query.Where(u => u.UserName == userName &&
                                   u.Password == pwd)
                  .Count();
            
            
            return (count > 0);
        }
    }
}
