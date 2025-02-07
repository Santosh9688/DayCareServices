using DayCare.Dal.DayCareDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.DayCare
{
    public sealed class Context
    {
        public static DmlManager DB { get; internal set; }
        public static void Setup(Dictionary<string, string> connStrings)
        {
            var daycareKey = DmlManager.DBName + "ConnString";

            DB = connStrings.ContainsKey(daycareKey)
                            ? new DmlManager(connStrings[daycareKey])
                            : throw new ArgumentNullException(daycareKey);
        }
    }
}
