using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealmPOC.DatabaseModel
{
   public class User: RealmObject
    {
    [PrimaryKey]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Description { get; set; }

    }
}
