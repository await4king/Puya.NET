using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Data
{
    public class FakeDbCommand : DbCommand
    {
        public FakeDbCommand(DbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }
        public FakeDbCommand(string text, DbConnection dbConnection)
        {
            CommandText = text;
            DbConnection = dbConnection;
        }
        public FakeDbCommand(string text, DbConnection dbConnection, DbTransaction tran)
        {
            CommandText = text;
            DbConnection = dbConnection;
            Transaction = tran;
        }
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection => new FakeDbParameterCollection();
        protected override DbTransaction DbTransaction { get; set; }
        public override void Cancel()
        { }
        public override int ExecuteNonQuery()
        {
            return 0;
        }
        public override object ExecuteScalar()
        {
            return null;
        }
        public override void Prepare()
        { }
        protected override DbParameter CreateDbParameter()
        {
            return new FakeDbParameter();
        }
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new FakeDbDataReader();
        }
    }
}
