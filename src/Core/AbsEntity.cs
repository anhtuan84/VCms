using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    abstract public class  AbsEntity
    {
        public String Id { get; set; }
        public Int64 Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 Deleted { get; set; }
        public String CreatedBy { get; set; }
        public String ModifiedBy { get; set; }

    }
}
