using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.CRAutoCancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            new CRAutoCancellationHelper().RunCRAutoProcess();
        }
    }
}
