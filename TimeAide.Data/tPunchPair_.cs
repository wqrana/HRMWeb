using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Data
{
    public partial class tPunchPair : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
