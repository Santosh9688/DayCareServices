using Microsoft.Extensions.Configuration;

namespace DayCare.Service
{
    public class Bootstrapper
    {
        public static void BootupDBContext(Dictionary<string, string> connStrings)
        {
            Core.DbContext.Setup(connStrings);
            Core.DayCare.Context.Setup(connStrings);
        }
    }
}
