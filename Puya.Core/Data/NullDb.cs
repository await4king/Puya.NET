using System.Data.Common;

namespace Puya.Data
{
    public class NullDb : DbBase
    {
        protected override DbConnection GetConnectionInternal(string connectionString)
        {
            var result = new FakeDbConnection();

            result.ConnectionString = connectionString;

            return result;
        }
        protected override void SetContextInfo(DbConnection con)
        {
        }
    }
}
