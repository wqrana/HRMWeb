using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminPanel.DAL
{
     public partial class TA7_ImportData_Context : DbContext
    {

        public TA7_ImportData_Context(string connectionString)
                                                  : base(connectionString)
        {
        }


    }
}
