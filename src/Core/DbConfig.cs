using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DbConfig : DbConfiguration
    {
        public DbConfig()
        {
            this.SetProviderServices("System.Data.SqlClient",
                        System.Data.Entity.SqlServer.SqlProviderServices.Instance);
           
        }
    }
}
