using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Data
{
    public class NullDb : IDb
    {
        public IConnectionStringProvider ConnectionStringProvider
        {
            get;set;
        }
        public bool PersistConnection { get; set; }
        public bool AutoNullEmptyStrings { get; set; }

        public DbConnection GetConnection()
        {
            return new FakeDbConnection();
        }
    }
}
