using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealmPOC.DatabaseModel
{
   public  class Department:RealmObject
    {
       [PrimaryKey]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Address { get; set; }
    }
}
