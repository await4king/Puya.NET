using System.Data;
using System.Data.Common;

namespace Puya.Data
{
    public class FakeDbConnection : DbConnection
    {
        public override string ConnectionString { get; set; }

        public override string Database => "";

        public override string DataSource => "";

        public override string ServerVersion => "";

        public override ConnectionState State => ConnectionState.Closed;

        public override void ChangeDatabase(string databaseName)
        { }
        public override void Close()
        { }
        public override void Open()
        { }
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new FakeDbTransaction(this);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new FakeDbCommand("", this);
        }
    }
}
