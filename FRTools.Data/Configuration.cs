using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Runtime.Remoting.Messaging;

namespace FRTools.Data
{
    public class Configuration : DbConfiguration
    {
        public Configuration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => SuspendExecutionStrategy
                ? (IDbExecutionStrategy)new DefaultExecutionStrategy()
                : new SqlAzureExecutionStrategy());
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    }
}
