using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Enums
    {
        public enum Rate
        {
            VeryLow = -2,
            Low = -1,
            Default = 0,
            High = 1,
            VeryHigh = 2,
            Unvoted = -246111
        }
    }
}
