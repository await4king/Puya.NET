using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Data
{
    public class FakeDbTransaction : DbTransaction
    {
        public override IsolationLevel IsolationLevel => IsolationLevel.Unspecified;
        private DbConnection _con;
        protected override DbConnection DbConnection => _con;
        public FakeDbTransaction(DbConnection con)
        {
            _con = con;
        }

        public override void Commit()
        { }

        public override void Rollback()
        { }
    }
}
