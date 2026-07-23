using Puya.Data;
using Puya.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Core.Tests
{
    public partial class FinAccCg
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? LatinName { get; set; }
        public string? Parent { get; set; }
        public int Index { get; set; }
        public string Leaf { get; set; }
        public string Class { get; set; }
        public string Bench { get; set; }
        public string? Reference { get; set; }
        public string? Detailed { get; set; }
        public string? Controled { get; set; }
        public string? Projected { get; set; }
        public string? Documented { get; set; }
        public string? DocumentedAmount { get; set; }

        public string? Standard { get; set; }

        public string? Topic { get; set; }
        public string? Refine { get; set; }
        public string? ListStartedInsertType { get; set; }
        public string? DefaultComment { get; set; }
        public string ShowCurrency { get; set; }
        public int? CatalogCodingRefer { get; set; }
        public short ListStartedInsertType2 { get; set; }
        public string? ExternalCode { get; set; }
    }
    public class DbHelper
    {
        IDb GetDb()
        {
            var constrProvider = new DefaultConnectionStringProvider();
            var contextInfoProvider = new DefaultDbContextInfoProvider();
            var mapper = new DefaultMapper();

            constrProvider.SetConnectionString("Server=.\\I2k17;Database=Karmania-14031012;User Id=sa;Password=sql2k17pass123;TrustServerCertificate=true;MultipleActiveResultSets=true");

            var db = new SqlServerDb(constrProvider, contextInfoProvider, mapper);

            return db;
        }
        [Fact]
        public void TestMapper()
        {
            var db = GetDb();

            var data = db.ExecuteReaderSql<FinAccCg>("select * from FinAccCg");

            Assert.True(data.Count() > 0);
        }
    }
}
