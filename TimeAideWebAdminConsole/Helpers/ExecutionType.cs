using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.AdminConsole.Helpers
{
    public enum ExecutionType
    {
        DataMigration=0,
        NewEmptyDatabase=1,
        NewClient=2,
        NewClientWithDefaultValues=3,
    }
}
