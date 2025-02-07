using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Dal.DayCareDB
{
    public sealed class DmlManager : BaseDmlManager
    {
        public DmlManager(string connString) : base(connString) { }

        public const string DBName = "DayCare";
    }
}
