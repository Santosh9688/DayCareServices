using DayCare.Core.DayCare;
using System.Reflection;

namespace DayCare.Core
{
    public sealed class DbContext
    {
        public static Dal.DayCareDB.DmlManager DayCareDB { get; internal set; }
        public static void Setup(Dictionary<string, string> connStrings) 
        {
            var dayCareKey = Dal.DayCareDB.DmlManager.DBName + "ConnString";

            DayCareDB = connStrings.ContainsKey(dayCareKey)
                ? new Dal.DayCareDB.DmlManager(connStrings[dayCareKey])
                : throw new ArgumentNullException(dayCareKey);
        }


    }
}