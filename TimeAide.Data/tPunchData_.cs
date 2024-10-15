using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Data
{
    public partial class tPunchData : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
